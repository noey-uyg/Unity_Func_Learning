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
    protected int _refreshCount; // �� �籸�� Ƚ��
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
    /// �ʱ� �� ����
    /// </summary>
    protected virtual void GenerateGrid()
    {

    }

    /// <summary>
    /// ī�޶� �� ����
    /// </summary>
    private void CameraSetting()
    {
        if(_camera == null)
        {
            _camera = Camera.main;
        }

        //�ػ�
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        //����
        float aspectRatio = screenWidth / screenHeight;

        // ���ο� ���� ������ �� �� ū ���� ī�޶� ������
        float widthSize = _mapWidth / (2 * aspectRatio);
        float heightSize = _mapHeight / 2f;
        float orthographicSize = widthSize > heightSize ? widthSize : heightSize;

        _camera.orthographicSize = orthographicSize;
    }

    /// <summary>
    /// Ÿ�� �ʱ�ȭ
    /// </summary>
    private void ClearTileMap()
    {
        _tilemap.ClearAllTiles(); // Ÿ�ϸ��� ��� Ÿ�� ����
    }

    /// <summary>
    /// ���� ����
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
