using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    private SceneName _sceneName;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void Init()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void AddOnClickListener(UnityAction<SceneName> action)
    {
        _button.onClick.AddListener(() => action(_sceneName));
    }
    
    public void SetType(SceneName sceneName)
    {
        _sceneName = sceneName;
        _buttonText.text = _sceneName.ToString();
    }
}
