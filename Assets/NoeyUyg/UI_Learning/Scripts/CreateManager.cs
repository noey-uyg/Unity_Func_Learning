using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
    [SerializeField] private Transform _windowParent;

    public void CreateWindow()
    {
        Window window = WindowPool.Instance.GetWindow();

        window.transform.SetParent(_windowParent);

        window.transform.localScale = Vector3.one;
        window.transform.localPosition = Vector3.zero;
    }
}
