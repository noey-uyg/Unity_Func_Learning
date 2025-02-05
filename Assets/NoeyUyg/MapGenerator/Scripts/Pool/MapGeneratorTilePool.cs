using UnityEngine;
using UnityEngine.Pool;

public class MapGeneratorTilePool : Singleton<MapGeneratorTilePool>
{
    [SerializeField] private MapGeneratorTile _tilePrefab;
    private ObjectPool<MapGeneratorTile> _tilePool;

    private const int maxSize = 10000;
    private const int initSize = 100;

    protected override void OnAwake()
    {
        _tilePool = new ObjectPool<MapGeneratorTile>(CreateMapGeneratorTile, ActivateMapGeneratorTile, DisableMapGeneratorTile, DestroyMapGeneratorTile, false, initSize, maxSize);
    }

    private MapGeneratorTile CreateMapGeneratorTile()
    {
        return Instantiate(_tilePrefab);
    }

    private void ActivateMapGeneratorTile(MapGeneratorTile raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableMapGeneratorTile(MapGeneratorTile raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroyMapGeneratorTile(MapGeneratorTile raw)
    {
        Destroy(raw);
    }

    public MapGeneratorTile GetMapGeneratorTile()
    {
        MapGeneratorTile raw = null;
        if (_tilePool.CountActive >= maxSize)
        {
            raw = CreateMapGeneratorTile();
        }
        else
        {
            raw = _tilePool.Get();
        }

        return raw;
    }

    public void ReleaseMapGeneratorTile(MapGeneratorTile raw)
    {
        _tilePool.Release(raw);
    }
}
