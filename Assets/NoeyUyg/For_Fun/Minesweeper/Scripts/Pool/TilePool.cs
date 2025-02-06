using UnityEngine;
using UnityEngine.Pool;

public class TilePool : Singleton<TilePool>
{
    [SerializeField] private MinesweeperTile _tilePrefab;
    private ObjectPool<MinesweeperTile> _tilePool;

    private const int maxSize = 10000;
    private const int initSize = 100;

    protected override void OnAwake()
    {
        _tilePool = new ObjectPool<MinesweeperTile>(CreateTile, ActivateTile, DisableTile, DestroyTile, false, initSize, maxSize);
    }

    private MinesweeperTile CreateTile()
    {
        return Instantiate(_tilePrefab);
    }

    private void ActivateTile(MinesweeperTile raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableTile(MinesweeperTile raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroyTile(MinesweeperTile raw)
    {
        Destroy(raw);
    }

    public MinesweeperTile GetTile()
    {
        MinesweeperTile raw = null;
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

    public void ReleaseTile(MinesweeperTile raw)
    {
        _tilePool.Release(raw);
    }
}