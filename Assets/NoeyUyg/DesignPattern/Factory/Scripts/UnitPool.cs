using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnitPool : Singleton<UnitPool>
{
    [SerializeField] private SmallBlue _smallBluePrefabs;
    [SerializeField] private SmallGreen _smallGreenPrefabs;
    [SerializeField] private SmallRed _smallRedPrefabs;
    [SerializeField] private BigBlue _bigBluePrefabs;
    [SerializeField] private BigGreen _bigGreenPrefabs;
    [SerializeField] private BigRed _bigRedPrefabs;

    private ObjectPool<SmallBlue> _smallBluePool;
    private ObjectPool<SmallGreen> _smallGreenPool;
    private ObjectPool<SmallRed> _smallRedPool;
    private ObjectPool<BigBlue> _bigBluePool;
    private ObjectPool<BigGreen> _bigGreenPool;
    private ObjectPool<BigRed> _bigRedPool;

    private const int maxSize = 10000;
    private const int initSize = 100;

    protected override void OnAwake()
    {
        _smallBluePool = new ObjectPool<SmallBlue>(CreateSmallBlue, ActivateSmallBlue, DisableSmallBlue, DestroySmallBlue, false, initSize, maxSize);
        _smallGreenPool = new ObjectPool<SmallGreen>(CreateSmallGreen, ActivateSmallGreen, DisableSmallGreen, DestroySmallGreen, false, initSize, maxSize);
        _smallRedPool = new ObjectPool<SmallRed>(CreateSmallRed, ActivateSmallRed, DisableSmallRed, DestroySmallRed, false, initSize, maxSize);
        _bigBluePool = new ObjectPool<BigBlue>(CreateBigBlue, ActivateBigBlue, DisableBigBlue, DestroyBigBlue, false, initSize, maxSize);
        _bigGreenPool = new ObjectPool<BigGreen>(CreateBigGreen, ActivateBigGreen, DisableBigGreen, DestroyBigGreen, false, initSize, maxSize);
        _bigRedPool = new ObjectPool<BigRed>(CreateBigRed, ActivateBigRed, DisableBigRed, DestroyBigRed, false, initSize, maxSize);
    }

    #region SmallBlue
    private SmallBlue CreateSmallBlue()
    {
        return Instantiate(_smallBluePrefabs);
    }

    private void ActivateSmallBlue(SmallBlue raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableSmallBlue(SmallBlue raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroySmallBlue(SmallBlue raw)
    {
        Destroy(raw);
    }

    public SmallBlue GetSmallBlue()
    {
        SmallBlue raw = null;

        if (_smallBluePool.CountActive >= maxSize)
        {
            raw = CreateSmallBlue();
        }
        else
        {
            raw = _smallBluePool.Get();
        }

        return raw;
    }

    public void ReleaseSmallBlue(SmallBlue raw)
    {
        _smallBluePool.Release(raw);
    }
    #endregion

    #region SmallGreen
    private SmallGreen CreateSmallGreen()
    {
        return Instantiate(_smallGreenPrefabs);
    }

    private void ActivateSmallGreen(SmallGreen raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableSmallGreen(SmallGreen raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroySmallGreen(SmallGreen raw)
    {
        Destroy(raw);
    }

    public SmallGreen GetSmallGreen()
    {
        SmallGreen raw = null;

        if (_smallGreenPool.CountActive >= maxSize)
        {
            raw = CreateSmallGreen();
        }
        else
        {
            raw = _smallGreenPool.Get();
        }

        return raw;
    }

    public void ReleaseSmallGreen(SmallGreen raw)
    {
        _smallGreenPool.Release(raw);
    }
    #endregion

    #region SmallRed
    private SmallRed CreateSmallRed()
    {
        return Instantiate(_smallRedPrefabs);
    }

    private void ActivateSmallRed(SmallRed raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableSmallRed(SmallRed raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroySmallRed(SmallRed raw)
    {
        Destroy(raw);
    }

    public SmallRed GetSmallRed()
    {
        SmallRed raw = null;

        if (_smallRedPool.CountActive >= maxSize)
        {
            raw = CreateSmallRed();
        }
        else
        {
            raw = _smallRedPool.Get();
        }

        return raw;
    }

    public void ReleaseSmallRed(SmallRed raw)
    {
        _smallRedPool.Release(raw);
    }
    #endregion

    #region BigBlue
    private BigBlue CreateBigBlue()
    {
        return Instantiate(_bigBluePrefabs);
    }

    private void ActivateBigBlue(BigBlue raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableBigBlue(BigBlue raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroyBigBlue(BigBlue raw)
    {
        Destroy(raw);
    }

    public BigBlue GetBigBlue()
    {
        BigBlue raw = null;

        if (_bigBluePool.CountActive >= maxSize)
        {
            raw = CreateBigBlue();
        }
        else
        {
            raw = _bigBluePool.Get();
        }

        return raw;
    }

    public void ReleaseBigBlue(BigBlue raw)
    {
        _bigBluePool.Release(raw);
    }
    #endregion

    #region BigGreen
    private BigGreen CreateBigGreen()
    {
        return Instantiate(_bigGreenPrefabs);
    }

    private void ActivateBigGreen(BigGreen raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableBigGreen(BigGreen raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroyBigGreen(BigGreen raw)
    {
        Destroy(raw);
    }

    public BigGreen GetBigGreen()
    {
        BigGreen raw = null;

        if (_bigGreenPool.CountActive >= maxSize)
        {
            raw = CreateBigGreen();
        }
        else
        {
            raw = _bigGreenPool.Get();
        }

        return raw;
    }

    public void ReleaseBigGreen(BigGreen raw)
    {
        _bigGreenPool.Release(raw);
    }
    #endregion

    #region BigRed
    private BigRed CreateBigRed()
    {
        return Instantiate(_bigRedPrefabs);
    }

    private void ActivateBigRed(BigRed raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableBigRed(BigRed raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroyBigRed(BigRed raw)
    {
        Destroy(raw);
    }

    public BigRed GetBigRed()
    {
        BigRed raw = null;

        if (_bigRedPool.CountActive >= maxSize)
        {
            raw = CreateBigRed();
        }
        else
        {
            raw = _bigRedPool.Get();
        }

        return raw;
    }

    public void ReleaseBigRed(BigRed raw)
    {
        _bigRedPool.Release(raw);
    }
    #endregion
}
