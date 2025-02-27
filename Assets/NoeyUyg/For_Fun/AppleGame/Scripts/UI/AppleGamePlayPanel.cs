using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AppleGamePlayPanel : AppleGamePanel
{
    [SerializeField] private AppleGameSliderUI _timer;
    [SerializeField] private TextMeshProUGUI _curScore;

    private WaitForSeconds _defaultWaitForSeconds = new WaitForSeconds(1f);
    private Coroutine _coroutine;
    public override void ShowAfterAction()
    {
        SetCurScore(0);
        _timer.SetInitValue(0, 120);
        _coroutine = StartCoroutine(IETimer());
    }

    IEnumerator IETimer()
    {
        while (_timer.Value > 0)
        {
            yield return _defaultWaitForSeconds;

            _timer.OnValueChanged();
        }

        AppleGameManager.Instance.GameFinish();
        AppleGamePanelManager.Instance.OnShowPanelAndHideCurPanel(PanelType.GameScore);
    }

    public void SetCurScore(int num)
    {
        _curScore.text = num.ToString();
    }

    public void OnRetry()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        AppleGameManager.Instance.GameFinish();
        AppleGameManager.Instance.GameStart();
        AppleGamePanelManager.Instance.OnShowPanelAndHideCurPanel(PanelType.GamePlay);
    }
}
