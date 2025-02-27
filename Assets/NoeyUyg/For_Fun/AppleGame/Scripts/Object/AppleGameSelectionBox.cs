using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGameSelectionBox : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Transform _transform;
    [SerializeField] private Color _possibleColor;
    [SerializeField] private Color _impossibleColor;

    public Transform GetTransform { get { return _transform; } }

    public void SetColor(int num)
    {
        _sr.color = num == 10 ? _possibleColor : _impossibleColor;
    }
}
