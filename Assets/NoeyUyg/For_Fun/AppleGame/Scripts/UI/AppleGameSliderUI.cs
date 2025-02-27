using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppleGameSliderUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public int Value { get { return (int)_slider.value; } }

    public void SetInitValue(int min, int max)
    {
        _slider.minValue = min;
        _slider.maxValue = max;

        _slider.value = max;
    }

    public void OnValueChanged()
    {
        int intValue = Mathf.RoundToInt(_slider.value - 1);
        _slider.value = intValue;
    }
}
