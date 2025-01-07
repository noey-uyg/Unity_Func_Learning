using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player _player;

    private ICommand _playerMoveUpCommand;
    private ICommand _playerMoveDownCommand;
    private ICommand _playerMoveLeftCommand;
    private ICommand _playerMoveRightCommand;

    private float _gameTime = 0f;
    private float _replayTime = 0f;
    private SortedList<float, ICommand> _replayCommand = new SortedList<float, ICommand>();

    private bool _isReplay;

    private void Start()
    {
        _playerMoveUpCommand = new PlayerMoveUpCommand(_player);
        _playerMoveDownCommand = new PlayerMoveDownCommand(_player);
        _playerMoveLeftCommand = new PlayerMoveLeftCommand(_player);
        _playerMoveRightCommand = new PlayerMoveRightCommand(_player);
    }

    private void Update()
    {
        if (_isReplay)
            return;

        if (!KeySettingManager.Instance.ChangeKey)
        {
            if (Input.GetKeyDown(KeySettingManager.Instance.GetKeyCode(Command_KeyAction.MoveUP)))
            {
                ExecuteCommand(_playerMoveUpCommand);
            }
            else if (Input.GetKeyDown(KeySettingManager.Instance.GetKeyCode(Command_KeyAction.MoveDown)))
            {
                ExecuteCommand(_playerMoveDownCommand);
            }
            else if (Input.GetKeyDown(KeySettingManager.Instance.GetKeyCode(Command_KeyAction.MoveLeft)))
            {
                ExecuteCommand(_playerMoveLeftCommand);
            }
            else if (Input.GetKeyDown(KeySettingManager.Instance.GetKeyCode(Command_KeyAction.MoveRight)))
            {
                ExecuteCommand(_playerMoveRightCommand);
            }
            else if (Input.GetKeyDown(KeySettingManager.Instance.GetKeyCode(Command_KeyAction.Undo)))
            {
                if (_replayCommand.Count > 0)
                {
                    int lastIndex = _replayCommand.Count - 1;
                    _replayCommand.Values[lastIndex].Undo();
                    _replayCommand.RemoveAt(lastIndex);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isReplay)
        {
            _gameTime = 0f;
            _replayTime += Time.fixedDeltaTime;

            if(_replayCommand.Count > 0)
            {
                if(Mathf.Approximately(_replayTime, _replayCommand.Keys[0]))
                {
                    _replayCommand.Values[0].Execute();
                    _replayCommand.RemoveAt(0);
                }
            }
            else
            {
                _player.SetStartPosition();
                _isReplay = false;
            }
        }
        else
        {
            _gameTime += Time.fixedDeltaTime; // 리플레이를 위한 시간 저장
        }
    }

    private void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _replayCommand.Add(_gameTime, command);
    }

    public void ReplayStart()
    {
        _replayTime = 0f;
        _player.ResetPosition();
        _isReplay = true;
    }
}
