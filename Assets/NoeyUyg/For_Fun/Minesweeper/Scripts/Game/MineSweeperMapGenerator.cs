using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperMapGenerator : Singleton<MineSweeperMapGenerator>
{
    [SerializeField] private RectTransform _map;
    private Dictionary<Vector2, Tile> _tiles = new Dictionary<Vector2, Tile>();
    private List<Tile> _mineTiles = new List<Tile>();

    private void Start()
    {
        MapGenerate();
    }

    // 맵 생성
    public void MapGenerate()
    {
        Init();
        float width = _map.rect.width;
        float height = _map.rect.height;

        float squareWidth = width / MineSweeperGameManager.Instance.BoardSizeX;
        float squareHeight = height / MineSweeperGameManager.Instance.BoardSizeY;

        for (int i = 0; i < MineSweeperGameManager.Instance.BoardSizeX; i++)
        {
            for (int j = 0; j < MineSweeperGameManager.Instance.BoardSizeY; j++)
            {
                // 자리 잡기
                Tile square = TilePool.Instance.GetTile();
                square.Init();
                square.SetRectSize(squareWidth, squareHeight);
                square.transform.SetParent(_map);
                square.transform.localScale = Vector3.one;
                square.transform.localPosition = new Vector2(i * squareWidth, -j * squareHeight);

                // 컬러 세팅
                int lightType = (i + j) % 2;
                square.SetLightType((MinesweeperTileLightType)lightType);
                square.SetOpenType(MinesweeperTileOpenType.Close);
                
                // 맵 정보, 지뢰 세팅
                int fieldNum = MineSweeperGameManager.Instance.GetField(i, j);
                square.SetNeighbourMine(fieldNum);
                square.SetPos(i, j);
                switch (fieldNum)
                {
                    case -1:
                        {
                            square.SetTileType(MinesweeperTileType.Mine);
                            square.SetText("X");
                            _mineTiles.Add(square);
                        }
                        break;
                    case 0:
                        {
                            square.SetTileType(MinesweeperTileType.None);
                            square.SetText(string.Empty);
                        }
                        break;
                    default:
                        {
                            square.SetTileType(MinesweeperTileType.None);
                            square.SetText(fieldNum.ToString());
                        }
                        break;
                }

                _tiles.Add(new Vector2(i, j), square);
            }
        }
    }

    // 초기화
    private void Init()
    {
        StopAllCoroutines();
        foreach(var v in _tiles)
        {
            TilePool.Instance.ReleaseTile(v.Value);
        }
        _tiles.Clear();
        _mineTiles.Clear();
    }

    public Tile GetTile(Vector2 pos)
    {
        return _tiles[pos];
    }

    // 모든 지뢰 공개
    public void OpenMine()
    {
        MineSweeperGameManager.Instance.SetMineOpen(true);
        StartCoroutine(MineOpenDelay());
    }

    IEnumerator MineOpenDelay()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);

        foreach(var v in _tiles)
        {
            if (v.Value.IsOpen())
                continue;                

            if(v.Value.GetTileType() == MinesweeperTileType.Mine)
                v.Value.SetColor(Color.magenta);

            yield return waitForSeconds;
        }
    }


    //모든 지뢰를 찾았는지
    public void IsAllMineFind()
    {
        for(int i = 0; i < _mineTiles.Count; i++)
        {
            if (!_mineTiles[i].IsGuessMine)
                return;
        }
        OpenTile();
    }

    // 모든 타일 공개
    public void OpenTile()
    {
        MineSweeperGameManager.Instance.SetClear(true);
        MineSweeperGameManager.Instance.SetMineOpen(true);
        StartCoroutine(TileOpenDelay());
    }

    IEnumerator TileOpenDelay()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);

        foreach (var v in _tiles)
        {
            if (v.Value.IsOpen())
                continue;

            v.Value.SetOpenType(MinesweeperTileOpenType.Open);
            yield return waitForSeconds;
        }
    }
}
