using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AppleGameScorePanel : AppleGamePanel
{
    [SerializeField] private TextMeshProUGUI _finalScore;

    public override void ShowAfterAction()
    {
        AppleGameManager.Instance.GameFinish();
        _finalScore.text = AppleGameManager.Instance.CurScore.ToString();
    }

    public void OnRetry()
    {
        AppleGameManager.Instance.GameStart();
        AppleGamePanelManager.Instance.OnShowPanelAndHideCurPanel(PanelType.GamePlay);
    }
}
