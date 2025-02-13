using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorManager : Singleton<MapGeneratorManager>
{
    #region CommonLimitValue
    private readonly int _rMinWidth = 20;
    private readonly int _rMaxWidth = 150;
    private readonly int _rMinHeight = 20;
    private readonly int _rMaxHeight = 150;
    private readonly int _rMinRefreshCount = 2;
    private readonly int _rMaxRefreshCount = 15;

    public int MinWidth { get { return _rMinWidth; } }
    public int MaxWidth { get { return _rMaxWidth; } }
    public int MinHeight { get { return _rMinHeight; } }
    public int MaxHeight { get { return _rMaxHeight; } }
    public int MinRefreshCount { get { return _rMinRefreshCount; } }
    public int MaxRefreshCount { get { return _rMaxRefreshCount; } }
    #endregion

    #region CellularLimitValue
    private readonly int _cellDenseRatioMin = 0;
    private readonly int _cellDenseRatioMax = 100;
    private readonly int _wallCountMin = 0;
    private readonly int _wallCountMax = 8;

    public int CellDenSeRatioMin { get { return _cellDenseRatioMin; } }
    public int CellDenSeRatioMax { get { return _cellDenseRatioMax; } }
    public int WallCountMin { get { return _wallCountMin; } }
    public int WallCountMax { get { return _wallCountMax; } }
    #endregion

    [System.Serializable]
    public class GeneratorMapping
    {
        public MapGeneratorType generatorType;
        public MapGenerators mapGenerators;
    }

    [SerializeField] private List<GeneratorMapping> _mapping = new List<GeneratorMapping>();

    private MapGenerators _curMapGenerator;
    private MapGeneratorType _curGeneratorType;
    private Dictionary<MapGeneratorType, MapGenerators> _generatorDict = new Dictionary<MapGeneratorType, MapGenerators>();

    public MapGenerators CurMapGenerator { get { return _curMapGenerator; } }
    public MapGeneratorType CurGeneratorType { get { return _curGeneratorType; } }

    protected override void OnAwake()
    {
        base.OnAwake();
        foreach (var v in _mapping)
        {
            _generatorDict[v.generatorType] = v.mapGenerators;
        }
    }

    public void SetMapGenerator(MapGeneratorType generatorType)
    {
        _curMapGenerator = _generatorDict[generatorType];
        _curGeneratorType = generatorType;
    }
}
