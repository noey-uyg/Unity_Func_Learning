using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// ���� https://blog.naver.com/dkdlelrktlfj/222252867893
/// </summary>

public class CellularAutomata : MapGenerators
{
    private int _cellDenseRatio; // �� ���е�
    private int _wallCount; // �ֺ��� ���� n�� �̻��� �� ������ ����� ����
    private string _seed;

    public void Setting(int cellDenseRatio, int wallCount, string seed = "")
    {
        _cellDenseRatio = cellDenseRatio;
        _wallCount = wallCount;
        _seed = seed;
    }

    /// <summary>
    /// �� ���� ����
    /// </summary>
    public override void GenerateMap()
    {
        base.GenerateMap();

        StartCoroutine(IERefreshMap());
    }

    /// <summary>
    /// 1�ʸ��� _refreshCount��ŭ �� ������Ʈ
    /// </summary>
    IEnumerator IERefreshMap()
    {
        for(int i = 0; i < _refreshCount; i++)
        {
            yield return _defaultCoroutineForSeconds;
            RefreshMap();
        }

        _isMapGenerating = false;
        _finishAction?.Invoke();
    }

    /// <summary>
    /// �ʱ� �� ����
    /// </summary>
    protected override void GenerateGrid()
    {
        _tileGrid = new bool[_mapWidth, _mapHeight];

        System.Random pseudoRandomSeed = string.IsNullOrEmpty(_seed)
            ? new System.Random(System.DateTime.Now.Ticks.GetHashCode())
            : new System.Random(_seed.GetHashCode());


        for(int x = 0; x < _mapWidth; x++)
        {
            for(int y = 0; y < _mapHeight; y++)
            {
                if (x == 0 || x == _mapWidth - 1 || y == 0 || y == _mapHeight - 1)// �����ڸ�
                {
                    _tileGrid[x, y] = false;
                }
                else
                {
                    _tileGrid[x, y] = (pseudoRandomSeed.Next(0, 100) < _cellDenseRatio) ? false : true;
                }
                SetTile(x, y);
            }
        }
    }

    /// <summary>
    ///  �� �籸��
    /// </summary>
    private void RefreshMap()
    {
        for(int x = 0; x < _mapWidth; x++)
        {
            for(int y = 0; y < _mapHeight; y++)
            {
                int nearWallCount = GetNearWallCount(x, y);

                if(nearWallCount > _wallCount) // ���� ���� �ֺ� �� ������ _wallCount���� �ʰ��� �� ���� ���� ������
                {
                    _tileGrid[x, y] = false;
                }
                else if(nearWallCount < _wallCount) // ���� ���� �ֺ� �� ������ _wallCount�� �̸��� �� ���� ���� ���
                {
                    _tileGrid[x, y] = true;
                }
                SetTile(x, y);
            }
        }
    }

    /// <summary>
    /// �ֺ� �� ����
    /// </summary>
    private int GetNearWallCount(int originX, int originY)
    {
        int count = 0;

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x==0 && y == 0) // �ڱ� �ڽ��� ����
                {
                    continue;
                }

                if(originX + x < 0 || originX + x >=_mapWidth || originY + y < 0 || originY +y >= _mapHeight) // ���� ����� ��
                {
                    count++;
                }
                else
                {
                    if(!_tileGrid[originX+x,originY+y]) // ���� ��
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    /// <summary>
    /// Ÿ�� ����
    /// </summary>
    private void SetTile(int x, int y)
    {
        int halfWidth = -_mapWidth / 2;
        int halfHeight = -_mapHeight / 2;
        Vector3Int tilePosition = new Vector3Int((halfWidth + x), (halfHeight + y), 0);
        if (_tileGrid[x, y])
        {
            _tilemap.SetTile(tilePosition, _roadTile); // �� Ÿ�� ����
        }
        else
        {
            _tilemap.SetTile(tilePosition, _wallTile); // �� Ÿ�� ����
        }
    }
}
