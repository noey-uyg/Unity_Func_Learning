using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapGeneratorSelectUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _allUI;
    [SerializeField] private GameObject _setCommonUI;
    [SerializeField] private GameObject _setCellularUI;

    [Header("Select")]
    [SerializeField] private TMP_Dropdown _mapDropdown;

    [Header("SetCommon")]
    [SerializeField] private SliderUI _mapWidth;
    [SerializeField] private SliderUI _mapHeight;
    [SerializeField] private SliderUI _RefreshCount;

    [Header("SetCelluar")]
    [SerializeField] private SliderUI _cellRatio;
    [SerializeField] private SliderUI _wallCount;
    [SerializeField] private TMP_InputField _seed;

    private void Start()
    {
        InitDropdown();
        InitSliderUI();
        _mapDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        MapGeneratorManager.Instance.SetMapGenerator(MapGeneratorType.CellularAutomata);

    }

    private void InitDropdown()
    {
        _mapDropdown.ClearOptions();
        List<string> options = new List<string>();

        for(int i = 0; i < (int)MapGeneratorType.LastInstance; i++)
        {
            options.Add(((MapGeneratorType)i).ToString());
        }

        _mapDropdown.AddOptions(options);
    }

    private void InitSliderUI()
    {
        _mapWidth.SetInitValue(MapGeneratorManager.Instance.MinWidth, MapGeneratorManager.Instance.MaxWidth);
        _mapHeight.SetInitValue(MapGeneratorManager.Instance.MinHeight, MapGeneratorManager.Instance.MaxHeight);
        _RefreshCount.SetInitValue(MapGeneratorManager.Instance.MinRefreshCount, MapGeneratorManager.Instance.MaxRefreshCount);

        _cellRatio.SetInitValue(MapGeneratorManager.Instance.CellDenSeRatioMin, MapGeneratorManager.Instance.CellDenSeRatioMax);
        _wallCount.SetInitValue(MapGeneratorManager.Instance.WallCountMin, MapGeneratorManager.Instance.WallCountMax);
    }

    private void OnDropdownValueChanged(int index)
    {
        var type = (MapGeneratorType)index;
        MapGeneratorManager.Instance.SetMapGenerator(type);
        ChangeMapGenerator(type);
    }

    public void OnGeneratorButtonClick()
    {
        SettingGenerator();
        MapGeneratorManager.Instance.CurMapGenerator.GenerateMap();
    }

    private void ChangeMapGenerator(MapGeneratorType type)
    {
        SettingUIDeActive();
        switch (type)
        {
            case MapGeneratorType.CellularAutomata:
                {
                    _setCellularUI.SetActive(true);
                }
                break;
            case MapGeneratorType.BSP:
                {

                }
                break;
        }
    }

    private void SettingUIDeActive()
    {
        _setCellularUI.SetActive(false);
    }

    private void SettingGenerator()
    {
        MapGeneratorManager.Instance.CurMapGenerator.CommonSetting(_mapWidth.Value, _mapHeight.Value, _RefreshCount.Value);
        MapGeneratorManager.Instance.CurMapGenerator.SetMapGeneratorStartAndFinishAction(OnStartAction, OnFinishAction);

        switch (MapGeneratorManager.Instance.CurGeneratorType)
        {
            case MapGeneratorType.CellularAutomata:
                {
                    var generator = (CellularAutomata)MapGeneratorManager.Instance.CurMapGenerator;
                    generator.Setting(_cellRatio.Value, _wallCount.Value, _seed.text);
                }
                break;
            case MapGeneratorType.BSP:
                {

                }
                break;
        }
    }

    public void OnStartAction()
    {
        _allUI.SetActive(false);
    }

    public void OnFinishAction()
    {
        _allUI.SetActive(true);
    }
}
