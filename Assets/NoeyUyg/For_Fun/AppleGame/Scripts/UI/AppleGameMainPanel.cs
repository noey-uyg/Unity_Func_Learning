using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGameMainPanel : AppleGamePanel
{
    public void OnGameStart()
    {
        AppleGameManager.Instance.GameStart();
        AppleGamePanelManager.Instance.OnShowPanelAndHideCurPanel(PanelType.GamePlay);
    }
}
