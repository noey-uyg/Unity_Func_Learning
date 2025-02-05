using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 참고 https://blog.naver.com/dkdlelrktlfj/222252867893
/// </summary>

public class CellularAutomata : MonoBehaviour
{
    [SerializeField] private int _map_width;
    [SerializeField] private int _map_height;
    [Range(0,100)]
    [SerializeField] private int _cellDenseRatio; // 셀 조밀도
    [Range(0,8)]
    [SerializeField] private int _wallCount; // 주변에 벽이 n개 이상일 때 벽으로 만들기 위함
    [SerializeField] private int _refreshCount; // 맵 재구성 횟수
    [SerializeField] private string seed;
    [SerializeField] private Transform _tileParent;
    [SerializeField] private float _tileSize;

    private bool[,] _tileGrid;
    private MapGeneratorTile[,] _tiles;

    private bool _isMapGenerating;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isMapGenerating)
        {
            GenerateMap();
        }   
    }

    /// <summary>
    /// 맵 생성 시작
    /// </summary>
    public void GenerateMap()
    {
        ClearTiles();
        GenerateGrid();

        StartCoroutine(IERefreshMap());
    }

    /// <summary>
    /// 1초마다 _refreshCount만큼 맵 업데이트
    /// </summary>
    IEnumerator IERefreshMap()
    {
        _isMapGenerating = true;
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        for(int i = 0; i < _refreshCount; i++)
        {
            yield return waitForSeconds;
            RefreshMap();
        }

        _isMapGenerating = false;
    }

    /// <summary>
    /// 초기 맵 정의
    /// </summary>
    private void GenerateGrid()
    {
        _tileGrid = new bool[_map_width, _map_height];

        System.Random pseudoRandomSeed = string.IsNullOrEmpty(seed)
            ? new System.Random(System.DateTime.Now.Ticks.GetHashCode())
            : new System.Random(seed.GetHashCode());


        for(int x = 0; x < _map_width; x++)
        {
            for(int y = 0; y < _map_height; y++)
            {
                if (x == 0 || x == _map_width - 1 || y == 0 || y == _map_height - 1)// 가장자리
                {
                    _tileGrid[x, y] = false;
                }
                else
                {
                    _tileGrid[x, y] = (pseudoRandomSeed.Next(0, 100) < _cellDenseRatio) ? false : true;
                }

                CreateTile(x,y);
            }
        }
    }

    /// <summary>
    ///  맵 재구성
    /// </summary>
    private void RefreshMap()
    {
        for(int x = 0; x < _map_width; x++)
        {
            for(int y = 0; y < _map_height; y++)
            {
                int nearWallCount = GetNearWallCount(x, y);

                if(nearWallCount > _wallCount) // 현재 셀의 주변 벽 개수가 _wallCount값을 초과할 때 현재 셀은 벽으로
                {
                    _tileGrid[x, y] = false;
                    _tiles[x, y].SetColor(Color.white);
                }
                else if(nearWallCount < _wallCount) // 현재 셀의 주변 벽 개수가 _wallCount값 미만일 때 현재 셀은 길로
                {
                    _tileGrid[x, y] = true;
                    _tiles[x, y].SetColor(Color.black);
                }
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

                if(originX + x < 0 || originX + x >=_map_width || originY + y < 0 || originY +y >= _map_height) // 맵을 벗어났을 때
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
    /// 타일 초기화
    /// </summary>
    private void ClearTiles()
    {
        if(_tiles != null)
        {
            for (int x = 0; x < _tiles.GetLength(0); x++)
            {
                for (int y = 0; y < _tiles.GetLength(1); y++)
                {
                    MapGeneratorTilePool.Instance.ReleaseMapGeneratorTile(_tiles[x, y]);
                }
            }
        }

        _tiles = new MapGeneratorTile[_map_width, _map_height];
    }

    /// <summary>
    /// 타일 생성
    /// </summary>
    private void CreateTile(int x, int y)
    {
        int halfWidth = -_map_width / 2;
        int halfHeight = -_map_height / 2;
        Color color = _tileGrid[x, y] ? Color.white : Color.black;

        var tile = MapGeneratorTilePool.Instance.GetMapGeneratorTile();

        tile.SetColor(color);
        tile.SetSize(_tileSize, _tileSize);
        tile.transform.SetParent(_tileParent);
        tile.transform.position = new Vector2((halfWidth + x) * _tileSize, (halfHeight + y) * _tileSize);

        _tiles[x,y] = tile;
    }
}
