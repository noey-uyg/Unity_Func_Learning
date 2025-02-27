using UnityEngine;
using UnityEngine.Pool;
public class AppleGameSquarePool : Singleton<AppleGameSquarePool>
{
    [SerializeField] private AppleGameSquare _applePrefab;
    private ObjectPool<AppleGameSquare> _applePool;

    private const int maxSize = 500;
    private const int initSize = 100;

    protected override void OnAwake()
    {
        _applePool = new ObjectPool<AppleGameSquare>(CreateApple, ActivateApple, DisableApple, DestroyApple, false, initSize, maxSize);
    }

    private AppleGameSquare CreateApple()
    {
        return Instantiate(_applePrefab);
    }

    private void ActivateApple(AppleGameSquare raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableApple(AppleGameSquare raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroyApple(AppleGameSquare raw)
    {
        Destroy(raw);
    }

    public AppleGameSquare GetApple()
    {
        AppleGameSquare raw = null;
        if (_applePool.CountActive >= maxSize)
        {
            raw = CreateApple();
        }
        else
        {
            raw = _applePool.Get();
        }

        return raw;
    }

    public void ReleaseApple(AppleGameSquare raw)
    {
        _applePool.Release(raw);
    }
}
