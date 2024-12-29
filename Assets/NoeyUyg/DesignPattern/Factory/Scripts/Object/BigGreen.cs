using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGreen : Unit
{
    public override void Init()
    {
        base.Init();
        _hp = 30;
        _range = 3;
        _speed = 2;
        _damage = 4;
        _unitMainType = FactoryUnitMainType.Big;
        _unitSubType = FactoryUnitSubType.BigGreen;
    }

    public override void Die()
    {
        UnitPool.Instance.ReleaseBigGreen(this);
    }
}
