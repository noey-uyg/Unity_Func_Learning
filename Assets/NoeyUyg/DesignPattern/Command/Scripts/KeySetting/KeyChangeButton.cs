using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyChangeButton : MonoBehaviour
{
    [SerializeField] private Command_KeyAction _action;
    [SerializeField] private TextMeshProUGUI _actionText;
    [SerializeField] private TextMeshProUGUI _keyText;

    private void Start()
    {
        _actionText.text = _action.ToString();
        _keyText.text = KeySettingManager.Instance.GetKeyCode(_action).ToString();
    }

    public void Refresh()
    {
        string keyText = KeySettingManager.Instance.GetKeyCode(_action).ToString();
        if (!_keyText.text.Equals(keyText))
            _keyText.text = keyText;
    }

    public Command_KeyAction GetKeyAction()
    {
        return _action;
    }
}
