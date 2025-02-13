using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MapGenerators : MonoBehaviour
{
    [Header("ForTile")]
    [SerializeField] protected Tilemap _tilemap;
    [SerializeField] protected RuleTile _wallTile;
    [SerializeField] protected RuleTile _roadTile;
    [SerializeField] protected RuleTile _roomTile;
    [SerializeField] protected RuleTile _outTile;

    protected Camera _camera;

    protected int _mapWidth;
    protected int _mapHeight;
    protected int _refreshCount; // 맵 재구성 횟수
                                 // 
    protected bool[,] _tileGrid;
    protected bool _isMapGenerating;
    protected Action _startAction;
    protected Action _finishAction;
    protected WaitForSeconds _defaultCoroutineForSeconds = new WaitForSeconds(1f);

    public virtual void GenerateMap()
    {
        if (_isMapGenerating)
            return;

        _startAction?.Invoke();

        _isMapGenerating = true;
        CameraSetting();
        ClearTileMap();
        GenerateGrid();
    }

    /// <summary>
    /// 초기 맵 정의
    /// </summary>
    protected virtual void GenerateGrid()
    {

    }

    /// <summary>
    /// 카메라 줌 조절
    /// </summary>
    private void CameraSetting()
    {
        if(_camera == null)
        {
            _camera = Camera.main;
        }

        //해상도
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        //비율
        float aspectRatio = screenWidth / screenHeight;

        // 가로와 세로 사이즈 중 더 큰 값을 카메라 값으로
        float widthSize = _mapWidth / (2 * aspectRatio);
        float heightSize = _mapHeight / 2f;
        float orthographicSize = widthSize > heightSize ? widthSize : heightSize;

        _camera.orthographicSize = orthographicSize;
    }

    /// <summary>
    /// 타일 초기화
    /// </summary>
    private void ClearTileMap()
    {
        _tilemap.ClearAllTiles(); // 타일맵의 모든 타일 제거
    }

    /// <summary>
    /// 공통 세팅
    /// </summary>
    public void CommonSetting(int mapWidth, int mapHeight, int refreshCount)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
        _refreshCount = refreshCount;
    }

    public void SetMapGeneratorStartAndFinishAction(Action start, Action finish)
    {
        _startAction = start;
        _finishAction = finish;
    }
}
