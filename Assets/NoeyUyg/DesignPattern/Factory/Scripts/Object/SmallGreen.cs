using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGreen : Unit
{
    public override void Init()
    {
        base.Init();
        _hp = 15;
        _range = 3;
        _speed = 2;
        _damage = 2;
        _unitMainType = FactoryUnitMainType.Small;
        _unitSubType = FactoryUnitSubType.SmallGreen;
    }

    public override void Die()
    {
        UnitPool.Instance.ReleaseSmallGreen(this);
    }
}