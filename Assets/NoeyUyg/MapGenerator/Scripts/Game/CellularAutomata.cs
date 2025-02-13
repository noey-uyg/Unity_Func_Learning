using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 참고 https://blog.naver.com/dkdlelrktlfj/222252867893
/// </summary>

public class CellularAutomata : MapGenerators
{
    private int _cellDenseRatio; // 셀 조밀도
    private int _wallCount; // 주변에 벽이 n개 이상일 때 벽으로 만들기 위함
    private string _seed;

    public void Setting(int cellDenseRatio, int wallCount, string seed = "")
    {
        _cellDenseRatio = cellDenseRatio;
        _wallCount = wallCount;
        _seed = seed;
    }

    /// <summary>
    /// 맵 생성 시작
    /// </summary>
    public override void GenerateMap()
    {
        base.GenerateMap();

        StartCoroutine(IERefreshMap());
    }

    /// <summary>
    /// 1초마다 _refreshCount만큼 맵 업데이트
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
    /// 초기 맵 정의
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
                if (x == 0 || x == _mapWidth - 1 || y == 0 || y == _mapHeight - 1)// 가장자리
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
    ///  맵 재구성
    /// </summary>
    private void RefreshMap()
    {
        for(int x = 0; x < _mapWidth; x++)
        {
            for(int y = 0; y < _mapHeight; y++)
            {
                int nearWallCount = GetNearWallCount(x, y);

                if(nearWallCount > _wallCount) // 현재 셀의 주변 벽 개수가 _wallCount값을 초과할 때 현재 셀은 벽으로
                {
                    _tileGrid[x, y] = false;
                }
                else if(nearWallCount < _wallCount) // 현재 셀의 주변 벽 개수가 _wallCount값 미만일 때 현재 셀은 길로
                {
                    _tileGrid[x, y] = true;
                }
                SetTile(x, y);
            }
        }
    }

    /// <summary>
    /// 주변 벽 개수
    /// </summary>
    private int GetNearWallCount(int originX, int originY)
    {
        int count = 0;

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x==0 && y == 0) // 자기 자신은 제외
                {
                    continue;
                }

                if(originX + x < 0 || originX + x >=_mapWidth || originY + y < 0 || originY +y >= _mapHeight) // 맵을 벗어났을 때
                {
                    count++;
                }
                else
                {
                    if(!_tileGrid[originX+x,originY+y]) // 벽일 때
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    /// <summary>
    /// 타일 설정
    /// </summary>
    private void SetTile(int x, int y)
    {
        int halfWidth = -_mapWidth / 2;
        int halfHeight = -_mapHeight / 2;
        Vector3Int tilePosition = new Vector3Int((halfWidth + x), (halfHeight + y), 0);
        if (_tileGrid[x, y])
        {
            _tilemap.SetTile(tilePosition, _roadTile); // 길 타일 설정
        }
        else
        {
            _tilemap.SetTile(tilePosition, _wallTile); // 벽 타일 설정
        }
    }
}
