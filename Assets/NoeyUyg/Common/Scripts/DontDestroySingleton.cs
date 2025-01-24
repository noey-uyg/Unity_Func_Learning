using UnityEngine;

/// <summary>
/// ΩÃ±€≈Ê
/// </summary>
public class DontDestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    protected virtual void OnAwake() { }
}
