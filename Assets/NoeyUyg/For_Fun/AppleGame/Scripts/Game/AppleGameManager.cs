using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGameManager : Singleton<AppleGameManager>
{
    private int _curScore;

    private bool _isGameStart;

    public int CurScore { get { return _curScore; } }
    public bool IsGameStart { get { return _isGameStart; } }

    public void GameStart()
    {
        _isGameStart = true;
        _curScore = 0;
        AppleGameGenerator.Instance.MapGenerate();
    }

    public void GameFinish()
    {
        _isGameStart = false;
        AppleGameGenerator.Instance.Init();
    }

    public void AddScore(int num)
    {
        _curScore += num;
        var panel = (AppleGamePlayPanel)AppleGamePanelManager.Instance.GetPanel(PanelType.GamePlay);
        if (panel.gameObject.activeInHierarchy)
        {
            panel.SetCurScore(_curScore);
        }
    }
}
