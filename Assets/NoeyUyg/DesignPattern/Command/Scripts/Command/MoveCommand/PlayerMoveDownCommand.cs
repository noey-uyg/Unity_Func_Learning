using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveDownCommand : ICommand
{
    private Player _player;
    public PlayerMoveDownCommand(Player player)
    {
        this._player = player;
    }

    public void Execute()
    {
        _player.MoveDown();
    }

    public void Undo()
    {
        _player.MoveUp();
    }
}
