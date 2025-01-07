using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Transform _transform;
    private float _speed = 1f;
    private Vector2 _startPosition;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _startPosition = _transform.position;
    }

    public void MoveUp()
    {
        MovePlayer(Vector2.up);
    }

    public void MoveDown()
    {
        MovePlayer(Vector2.down);
    }

    public void MoveLeft()
    {
        MovePlayer(Vector2.left);
    }

    public void MoveRight()
    {
        MovePlayer(Vector2.right);
    }

    public void SetStartPosition()
    {
        _startPosition = _transform.position;
    }

    public void ResetPosition()
    {
        _transform.position = _startPosition;
    }

    private void MovePlayer(Vector2 dir)
    {
        _transform.Translate(dir * _speed);
    }
}
