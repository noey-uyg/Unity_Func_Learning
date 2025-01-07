using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveRightCommand : ICommand
{
    private Player _player;
    public PlayerMoveRightCommand(Player player)
    {
        this._player = player;
    }

    public void Execute()
    {
        _player.MoveRight();
    }

    public void Undo()
    {
        _player.MoveLeft();
    }
}