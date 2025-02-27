using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType
{
    GameMain,
    GamePlay,
    GameScore,
}

public class AppleGamePanelManager : Singleton<AppleGamePanelManager>
{
    [System.Serializable]
    public class PanelMapping
    {
        public PanelType panelType;
        public AppleGamePanel panelObject;
    }

    [SerializeField] private List<PanelMapping> _panelMappings = new List<PanelMapping>();
    private Dictionary<PanelType, AppleGamePanel> _panelDictionary;
    private AppleGamePanel _currentPanel;

    private void Start()
    {
        _panelDictionary = new Dictionary<PanelType, AppleGamePanel>();

        foreach (var mapping in _panelMappings)
        {
            _panelDictionary[mapping.panelType] = mapping.panelObject;
            if (mapping.panelType != PanelType.GameMain)
            {
                mapping.panelObject.OnHide();
            }
            else
            {
                _currentPanel = mapping.panelObject;
                _currentPanel.OnShow();
            }
        }
    }

    public AppleGamePanel GetPanel(PanelType type)
    {
        if (_panelDictionary.TryGetValue(type, out var panel))
        {
            return panel;
        }
        return null;
    }

    public void OnShowPanel(PanelType type)
    {
        if (_panelDictionary.TryGetValue(type, out var panel))
        {
            panel.OnShow();
        }
    }

    public void OnShowPanelAndHideCurPanel(PanelType type)
    {
        if (_panelDictionary.TryGetValue(type, out var panel))
        {
            if (_currentPanel != null)
            {
                OnHideCurrentPanel();
            }
            _currentPanel = panel;
            panel.OnShow();
        }
    }

    public void OnHideCurrentPanel()
    {
        _currentPanel.OnHide();
    }
}
