using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRed : Unit
{
    public override void Init()
    {
        base.Init();
        _hp = 10;
        _range = 5;
        _speed = 1;
        _damage = 3;
        _unitMainType = FactoryUnitMainType.Small;
        _unitSubType = FactoryUnitSubType.SmallRed;
    }

    public override void Die()
    {
        UnitPool.Instance.ReleaseSmallRed(this);
    }
}
