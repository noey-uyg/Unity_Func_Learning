using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRed : Unit
{
    public override void Init()
    {
        base.Init();
        _hp = 20;
        _range = 5;
        _speed = 1;
        _damage = 6;
        _unitMainType = FactoryUnitMainType.Big;
        _unitSubType = FactoryUnitSubType.BigRed;
    }

    public override void Die()
    {
        UnitPool.Instance.ReleaseBigRed(this);
    }
}
