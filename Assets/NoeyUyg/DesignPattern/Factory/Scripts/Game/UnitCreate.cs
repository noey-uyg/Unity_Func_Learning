using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCreate : Singleton<UnitCreate>
{
    [SerializeField] private Transform _smallUnitSpawnPoint;
    [SerializeField] private Transform _bigUnitSpawnPoint;
    [SerializeField] private UnitFactory _smallUnitFactory;
    [SerializeField] private UnitFactory _bigUnitFactory;

    [SerializeField] private TMP_InputField _inputField;

    private List<Unit> _smallUnitList = new List<Unit>();
    private List<Unit> _bigUnitList = new List<Unit>();

    private int _inputCreateNumber = 0;
    private float _createDelay = 2.5f;
    private float _trackingDelay = 0.25f;

    private void Start()
    {
        _inputField.onValidateInput += ValidInput;
        _inputField.onEndEdit.AddListener(ValidRange);
    }

    // 유닛 생성
    public void CreateUnit(FactoryUnitMainType mainType, FactoryUnitSubType subType, Vector2 spawnPosition)
    {
        Unit unit = null;

        switch (mainType)
        {
            case FactoryUnitMainType.Small:
                unit = _smallUnitFactory.Create(subType);
                unit.transform.position = spawnPosition;
                _smallUnitList.Add(unit);
            break;
            case FactoryUnitMainType.Big:
                unit = _bigUnitFactory.Create(subType);
                unit.transform.position = spawnPosition;
                _bigUnitList.Add(unit);
            break;
        }

        if(unit != null)
        {
            StartCoroutine(TrackEnemies(unit));
        }
    }

    // 유닛 지속적으로 추적
    private IEnumerator TrackEnemies(Unit unit)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_trackingDelay);
        while (unit.gameObject.activeInHierarchy)
        {
            unit.FindNearbyEnemies(unit.UnitMainType == FactoryUnitMainType.Big ? _smallUnitList : _bigUnitList);
            yield return waitForSeconds;
        }
    }

    public void SetCrateNumber()
    {
        if (!string.IsNullOrEmpty(_inputField.text)) // InputField값이 비어있지 않을 때 값 저장
            _inputCreateNumber = int.Parse(_inputField.text);
        else
            _inputCreateNumber = 0;

        if(_inputCreateNumber > 0)
        {
            StartCoroutine(IECreateUnit(FactoryUnitMainType.Big, _inputCreateNumber));
            StartCoroutine(IECreateUnit(FactoryUnitMainType.Small, _inputCreateNumber * 2));
        }
    }

    IEnumerator IECreateUnit(FactoryUnitMainType mainType, int createNumber)
    {
        float minDelay = 0.1f;
        float delay = Mathf.Max(minDelay, _createDelay / createNumber);
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        for(int i=0;i<createNumber; i++)
        {
            Vector2 spawnPoint = mainType == FactoryUnitMainType.Big ? _bigUnitSpawnPoint.position : _smallUnitSpawnPoint.position;

            int subTypeNum = Random.Range(0, (int)FactoryUnitSubType.BigRed);
            subTypeNum = mainType == FactoryUnitMainType.Big ? subTypeNum + 3 : subTypeNum;

            CreateUnit(mainType, (FactoryUnitSubType)subTypeNum, spawnPoint);

            yield return waitForSeconds;
        }
    }

    // InputField 값 숫자만 받기
    private char ValidInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar))
        {
            return addedChar;
        }

        return '\0';
    }

    // InputField 값 1~100으로 제한
    private void ValidRange(string input)
    {
        if(int.TryParse(input, out int inputText))
        {
            if(inputText < 1 || inputText > 100)
            {
                _inputField.text = string.Empty;
                return;
            }
        }
    }
}
