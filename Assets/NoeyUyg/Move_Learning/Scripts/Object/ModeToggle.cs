using UnityEngine;
using UnityEngine.UI;

public class ModeToggle : MonoBehaviour
{
    [SerializeField] private MoveMode _mode;
    [SerializeField] private Toggle _thisToggle;

    public bool IsOn { get { return _thisToggle.isOn; } }

    private void Awake()
    {
        _thisToggle.onValueChanged.AddListener(IsSelect);
    }

    public void IsSelect(bool isOn)
    {
        if (isOn)
        {
            Controller.Instance.SetMode(_mode);
        }
    }
}
