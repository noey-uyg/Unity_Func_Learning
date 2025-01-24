using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SceneChangeButtonPool : Singleton<SceneChangeButtonPool>
{
    [SerializeField] private SceneChangeButton _buttonPrefab;
    private ObjectPool<SceneChangeButton> _sceneChangePool;

    private const int maxSize = 10000;
    private const int initSize = 100;

    protected override void OnAwake()
    {
        _sceneChangePool = new ObjectPool<SceneChangeButton>(CreateSceneChangeButton, ActivateSceneChangeButton, DisableSceneChangeButton, DestroySceneChangeButton, false, initSize, maxSize);
    }

    private SceneChangeButton CreateSceneChangeButton()
    {
        return Instantiate(_buttonPrefab);
    }

    private void ActivateSceneChangeButton(SceneChangeButton raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableSceneChangeButton(SceneChangeButton raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroySceneChangeButton(SceneChangeButton raw)
    {
        Destroy(raw);
    }

    public SceneChangeButton GetSceneChangeButton()
    {
        SceneChangeButton raw = null;
        if (_sceneChangePool.CountActive >= maxSize)
        {
            raw = CreateSceneChangeButton();
        }
        else
        {
            raw = _sceneChangePool.Get();
        }

        return raw;
    }

    public void ReleaseSceneChangeButton(SceneChangeButton raw)
    {
        _sceneChangePool.Release(raw);
    }
}