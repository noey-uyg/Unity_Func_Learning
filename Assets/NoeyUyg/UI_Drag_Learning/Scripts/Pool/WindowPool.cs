using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WindowPool : Singleton<WindowPool>
{
    [SerializeField] private Window _windowPrefab;
    private ObjectPool<Window> _windowPool;

    private const int maxSize = 10000;
    private const int initSize = 10;

    protected override void OnAwake()
    {
        _windowPool = new ObjectPool<Window>(CreateWindow, ActivateWindow, DisableWindow, DestroyWindow, false, initSize, maxSize);
    }

    private Window CreateWindow()
    {
        return Instantiate(_windowPrefab);
    }

    private void ActivateWindow(Window window)
    {
        window.gameObject.SetActive(true);
    }

    private void DisableWindow(Window window)
    {
        window.gameObject.SetActive(false);
    }

    private void DestroyWindow(Window window)
    {
        Destroy(window);
    }

    public Window GetWindow()
    {
        Window window = null;

        if (_windowPool.CountActive >= maxSize)
        {
            window = CreateWindow();
        }
        else
        {
            window = _windowPool.Get();
        }

        return window;
    }

    public void ReleaseWindow(Window window)
    {
        _windowPool.Release(window);
    }
}
