using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetAndModeUI : Singleton<TargetAndModeUI>
{
    [SerializeField] private ToggleGroup _targetToggleGroup;
    [SerializeField] private ToggleGroup _modeToggleGroup;

    [SerializeField] private TextMeshProUGUI _targetText;
    [SerializeField] private TextMeshProUGUI _modeText;

    private int _targetToggleClickCount = 0;
    private int _modeToggleClickCount = 0;

    // Ÿ�� ��� ���̱�/ �����
    public void ShowTargetToggleControl()
    {
        _targetToggleGroup.gameObject.SetActive(_targetToggleClickCount == 0);
        _targetToggleClickCount = 1 - _targetToggleClickCount;
    }

    // ��� ��� ���̱�/ �����
    public void ShowModeToggleControl()
    {
        _modeToggleGroup.gameObject.SetActive(_modeToggleClickCount == 0);
        _modeToggleClickCount = 1 - _modeToggleClickCount;
    }

    // Ÿ�� �ؽ�Ʈ
    public void SetTargetText(string text)
    {
        _targetText.text = string.Format("Target : {0}", text);
    }

    // ��� �ؽ�Ʈ
    public void SetModeText(string text)
    {
        _modeText.text = string.Format("Mode : {0}", text);
    }
}
