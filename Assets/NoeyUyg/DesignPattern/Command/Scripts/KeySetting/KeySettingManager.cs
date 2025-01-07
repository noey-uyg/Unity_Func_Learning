using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySettingManager : Singleton<KeySettingManager>
{
    [SerializeField] private List<KeyChangeButton> _keyList = new List<KeyChangeButton>();
    private Dictionary<Command_KeyAction, KeyCode> key = new Dictionary<Command_KeyAction, KeyCode>();
    private KeyChangeButton _currentkey;
    private bool _changeKey;

    public bool ChangeKey { get { return _changeKey; } }
    protected override void OnAwake()
    {
        Init();
    }

    // 초기 설정
    private void Init()
    {
        key.Add(Command_KeyAction.MoveUP, KeyCode.W);
        key.Add(Command_KeyAction.MoveDown, KeyCode.S);
        key.Add(Command_KeyAction.MoveLeft, KeyCode.A);
        key.Add(Command_KeyAction.MoveRight, KeyCode.D);

        key.Add(Command_KeyAction.Undo, KeyCode.Z);
    }

    public KeyCode GetKeyCode(Command_KeyAction keyAction)
    {
        return key[keyAction];
    }

    private void OnGUI()
    {
        if(_currentkey != null)
        {
            Event e = Event.current;
            KeyCode code = KeyCode.None;

            if (e.type == EventType.KeyDown)
            {
                code = e.keyCode;
            }
            else if (e.type == EventType.MouseDown)
            {
                switch (e.button)
                {
                    case 0:
                        code = KeyCode.Mouse0;
                        break;
                    case 1:
                        code = KeyCode.Mouse1;
                        break;
                }
            }

            if(code != KeyCode.None)
            {
                if (key.ContainsValue(code))
                {
                    var newDict = new Dictionary<Command_KeyAction, KeyCode>(key);

                    foreach (var v in newDict)
                    {
                        if (v.Value.Equals(code))
                        {
                            key[v.Key] = KeyCode.None;
                        }
                    }
                }

                key[_currentkey.GetKeyAction()] = code;
                for (int i = 0; i < _keyList.Count; i++)
                {
                    _keyList[i].Refresh();
                }
                _currentkey = null;
            }
        }
    }

    public void ChangeKeyButtonClick(KeyChangeButton currentkey)
    {
        _currentkey = currentkey;
        _changeKey = true;
    }

    public void Save()
    {
        _changeKey = false;
    }
}
