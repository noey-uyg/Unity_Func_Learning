using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveUpCommand : ICommand
{
    private Player _player;
    public PlayerMoveUpCommand(Player player)
    {
        this._player = player;
    }

    public void Execute()
    {
        _player.MoveUp();
    }

    public void Undo()
    {
        _player.MoveDown();
    }
}