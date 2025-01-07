using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveLeftCommand : ICommand
{
    private Player _player;
    public PlayerMoveLeftCommand(Player player)
    {
        this._player = player;
    }

    public void Execute()
    {
        _player.MoveLeft();
    }

    public void Undo()
    {
        _player.MoveRight();
    }
}
