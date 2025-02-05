using UnityEngine;
using UnityEngine.Pool;

public class TilePool : Singleton<TilePool>
{
    [SerializeField] private Tile _tilePrefab;
    private ObjectPool<Tile> _tilePool;

    private const int maxSize = 10000;
    private const int initSize = 100;

    protected override void OnAwake()
    {
        _tilePool = new ObjectPool<Tile>(CreateTile, ActivateTile, DisableTile, DestroyTile, false, initSize, maxSize);
    }

    private Tile CreateTile()
    {
        return Instantiate(_tilePrefab);
    }

    private void ActivateTile(Tile raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableTile(Tile raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroyTile(Tile raw)
    {
        Destroy(raw);
    }

    public Tile GetTile()
    {
        Tile raw = null;
        if (_tilePool.CountActive >= maxSize)
        {
            raw = CreateTile();
        }
        else
        {
            raw = _tilePool.Get();
        }

        return raw;
    }

    public void ReleaseTile(Tile raw)
    {
        _tilePool.Release(raw);
    }
}