using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopUIManager : Singleton<TopUIManager>
{
    [SerializeField] private TMP_Dropdown _difficultyLevel;
    [SerializeField] private TextMeshProUGUI _changeMineText;
    [SerializeField] private TextMeshProUGUI _changeTimeText;
    [SerializeField] private TextMeshProUGUI _changeBestTimeText;

    private int _curTime = 0;
    private int _bestTime = 0;

    private void Start()
    {
        _difficultyLevel.onValueChanged.AddListener(OnLevelValueChanged);
        Init();
    }

    private void OnLevelValueChanged(int value)
    {
        MineSweeperGameManager.Instance.SetGameStart(false);
        MineSweeperGameManager.Instance.UpdateLevelValue(value);
    }

    public void Init()
    {
        StopAllCoroutines();
        _changeTimeText.text = "0";
        if (MineSweeperGameManager.Instance.IsClear)
        {
            _bestTime = _curTime > _bestTime ? _curTime : _bestTime;
        }
        _changeBestTimeText.text = _bestTime.ToString();
        ChangeMineText();
    }

    public void ChangeMineText()
    {
        _changeMineText.text = MineSweeperGameManager.Instance.RemainMineCount.ToString();
    }

    public void StartTimeTextCoroutine()
    {
        StartCoroutine(StartTime());
    }

    IEnumerator StartTime()
    {
        _curTime = 0;
        WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

        while(!MineSweeperGameManager.Instance.IsClear && MineSweeperGameManager.Instance.IsGameStart && _curTime < 999)
        {
            yield return waitForSeconds;
            _curTime++;
            _changeTimeText.text = _curTime.ToString();
        }
        
    }
}
