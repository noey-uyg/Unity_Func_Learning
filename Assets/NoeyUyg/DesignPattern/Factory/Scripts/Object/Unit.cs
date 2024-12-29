using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Unit : MonoBehaviour
{
    private Transform _thisTransform;
    private Rigidbody2D _thisRigidbody;
    private Coroutine _attackCoroutine;

    protected int _hp;
    protected int _range;
    protected int _speed;
    protected int _damage;
    protected FactoryUnitSubType _unitSubType;
    protected FactoryUnitMainType _unitMainType;

    public Transform ThisTransform { get { return _thisTransform; } }
    public FactoryUnitMainType UnitMainType { get { return _unitMainType; } }

    // �ʱ�ȭ
    public virtual void Init()
    {
        _thisTransform = GetComponent<Transform>();
        _thisRigidbody = GetComponent<Rigidbody2D>();
    }

    // ����� �� ���� ã��
    public void FindNearbyEnemies(List<Unit> unitList)
    {
        if (unitList == null || unitList.Count == 0)
            return;

        Unit closestUnit = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < unitList.Count; i++){
            Unit curUnit = unitList[i];

            if (curUnit.UnitMainType != _unitMainType)
            {
                float distance = Vector3.Distance(_thisTransform.position, curUnit.ThisTransform.position);

                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestUnit = curUnit;
                }
            }
        }

        if(closestUnit != null)
        {
            StartCoroutine(MoveToTarget(closestUnit));
        }
    }

    IEnumerator MoveToTarget(Unit targetUnit)
    {
        while (targetUnit != null && targetUnit.gameObject.activeInHierarchy)
        {
            Vector2 targetPosition = targetUnit.ThisTransform.position;
            Vector2 direction = (targetPosition - (Vector2)_thisTransform.position).normalized;
            Vector2 targetPositionWithRange = targetPosition - direction * _range; // range ��ŭ �ڷ� ������

            float distanceToTarget = Vector2.Distance(_thisTransform.position, targetUnit.ThisTransform.position);

            // �����Ÿ� ���� ������ �̵��� ����
            if (distanceToTarget <= _range)
            {
                // ���� �ڷ�ƾ ����
                if (_attackCoroutine == null)
                {
                    _attackCoroutine = StartCoroutine(AttackCoroutine(targetUnit));
                }
                yield break; // �̵��� ����
            }

            if (distanceToTarget > _range)
            {
                Vector2 newPosition = Vector2.MoveTowards(_thisTransform.position, targetPositionWithRange, _speed * Time.fixedDeltaTime);
                _thisRigidbody.MovePosition(newPosition);
            }

            yield return null;
        }
    }

    IEnumerator AttackCoroutine(Unit target)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_speed*0.1f);
        while(target!= null && target.gameObject.activeInHierarchy)
        {
            Attack(target);
            yield return waitForSeconds;
        }
    }

    // ����
    public void Attack(Unit target)
    {
        target.TakeDamage(_damage);
    }

    // ������ ����
    public void TakeDamage(int damage)
    {
        _hp -= damage;

        if(_hp <= 0)
        {
            Die();
        }
    }

    // ���� �� ���� ó��
    public virtual void Die() { }
}
