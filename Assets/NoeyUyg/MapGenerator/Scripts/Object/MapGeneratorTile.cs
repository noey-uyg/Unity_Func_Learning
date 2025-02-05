using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorTile : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void SetSize(float x, float y)
    {
        _transform.localScale = new Vector3(x, y);
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
