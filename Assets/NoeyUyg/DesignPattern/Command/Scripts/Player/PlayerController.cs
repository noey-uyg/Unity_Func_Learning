using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Toggle _undoAddToggle;

    private ICommand _playerMoveUpCommand;
    private ICommand _playerMoveDownCommand;
    private ICommand _playerMoveLeftCommand;
    private ICommand _playerMoveRightCommand;

    private float _gameTime = 0f;
    private float _replayTime = 0f;
    private SortedList<float, KeyValuePair<Command_Action,ICommand>> _replayCommand = new SortedList<float, KeyValuePair<Command_Action, ICommand>>();
    private Stack<ICommand> _undoCommand = new Stack<ICommand>();

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
                ExecuteCommand(Command_Action.Excute, _playerMoveUpCommand);
            }
            else if (Input.GetKeyDown(KeySettingManager.Instance.GetKeyCode(Command_KeyAction.MoveDown)))
            {
                ExecuteCommand(Command_Action.Excute, _playerMoveDownCommand);
            }
            else if (Input.GetKeyDown(KeySettingManager.Instance.GetKeyCode(Command_KeyAction.MoveLeft)))
            {
                ExecuteCommand(Command_Action.Excute, _playerMoveLeftCommand);
            }
            else if (Input.GetKeyDown(KeySettingManager.Instance.GetKeyCode(Command_KeyAction.MoveRight)))
            {
                ExecuteCommand(Command_Action.Excute, _playerMoveRightCommand);
            }
            else if (Input.GetKeyDown(KeySettingManager.Instance.GetKeyCode(Command_KeyAction.Undo)))
            {
                if(_undoCommand.Count > 0)
                {
                    var undoCommand = _undoCommand.Pop();
                    ExecuteCommand(Command_Action.Undo, undoCommand);
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
                    switch (_replayCommand.Values[0].Key)
                    {
                        case Command_Action.Excute:
                            {
                                _replayCommand.Values[0].Value.Execute();
                            }
                            break;
                        case Command_Action.Undo:
                            {
                                _replayCommand.Values[0].Value.Undo();
                            }
                            break;
                    }

                    _replayCommand.RemoveAt(0);
                }
            }
            else
            {
                _player.SetStartPosition();
                Debug.Log("Replay Finish");
                _isReplay = false;
            }
        }
        else
        {
            _gameTime += Time.fixedDeltaTime; // 리플레이를 위한 시간 저장
        }
    }

    private void ExecuteCommand(Command_Action action, ICommand command)
    {
        switch (action)
        {
            case Command_Action.Undo:
                {
                    command.Undo();
                }
                break;
            case Command_Action.Excute:
                {
                    command.Execute();
                    _undoCommand.Push(command);
                }
                break;
        }

        if (action == Command_Action.Undo && !_undoAddToggle.isOn)
            return;

        _replayCommand.Add(_gameTime, new KeyValuePair<Command_Action,ICommand>(action,command));
    }

    public void ReplayStart()
    {
        _replayTime = 0f;
        _player.ResetPosition();
        _undoCommand.Clear();
        _isReplay = true;
    }
}
