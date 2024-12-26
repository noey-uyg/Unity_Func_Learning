using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    private Transform _thisTransform;
    private const float _speed = 0.005f;

    private Coroutine _moveCoroutine;

    private void Awake()
    {
        _thisTransform = GetComponent<Transform>();    
    }

    public void Move(Vector2 targetPos)
    {
        if(_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = null;

        if(Controller.Instance.Mode == MoveMode.Mouse)
        {
            _moveCoroutine = StartCoroutine(MoveToPosition(targetPos));
        }
        else if(Controller.Instance.Mode == MoveMode.Keyboard)
        {
            MoveToKeyboardInput(targetPos);
        }
    }

    public void MoveToKeyboardInput(Vector2 targetPos) // 키보드로 이동 시 전처리
    {
        targetPos = (Vector2)_thisTransform.position + targetPos.normalized;

        _moveCoroutine = StartCoroutine(MoveToPosition(targetPos));
    }

    IEnumerator MoveToPosition(Vector2 targetPos) // 움직임 코루틴
    {
        while (Vector2.Distance(_thisTransform.position, targetPos) > 0.001f)
        {
            _thisTransform.position = Vector2.MoveTowards(_thisTransform.position, targetPos, _speed);
            yield return null;
        }

        _thisTransform.position = targetPos;
    }
}
