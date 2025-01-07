using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RawImagePool : Singleton<RawImagePool>
{
    [SerializeField] private RawImageScript _rawImagePrefab;
    private ObjectPool<RawImageScript> _rawImagePool;

    private const int maxSize = 10000;
    private const int initSize = 100;

    protected override void OnAwake()
    {
        _rawImagePool = new ObjectPool<RawImageScript>(CreateRawImage, ActivateRawImage, DisableRawImage, DestroyRawImage, false, initSize, maxSize);
    }

    private RawImageScript CreateRawImage()
    {
        return Instantiate(_rawImagePrefab);
    }

    private void ActivateRawImage(RawImageScript raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableRawImage(RawImageScript raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroyRawImage(RawImageScript raw)
    {
        Destroy(raw);
    }

    public RawImageScript GetRawImage()
    {
        RawImageScript raw = null;

        if(_rawImagePool.CountActive >= maxSize)
        {
            raw = CreateRawImage();
        }
        else
        {
            raw = _rawImagePool.Get();
        }

        return raw;
    }

    public void ReleaseRawImage(RawImageScript raw)
    {
        _rawImagePool.Release(raw);
    }
}
