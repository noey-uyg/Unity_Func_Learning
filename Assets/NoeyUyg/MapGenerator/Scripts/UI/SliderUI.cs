using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private TextMeshProUGUI _tileText;

    public int Value { get { return (int)_slider.value; } }

    public void SetInitValue(int min, int max)
    {
        _tileText.text = this.gameObject.name;
        _slider.minValue = min;
        _slider.maxValue = max;

        _slider.value = (min + max) / 2;
        UpdateValueText((int)_slider.value);

        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);
        _slider.value = intValue;
        UpdateValueText(intValue);
    }

    public void UpdateValueText(int value)
    {
        _valueText.text = value.ToString();
    }
}
