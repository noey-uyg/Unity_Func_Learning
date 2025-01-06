using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBlue : Unit
{
    public override void Init()
    {
        base.Init();
        _hp = 20;
        _range = 1;
        _speed = 1;
        _damage = 3;
        _unitMainType = FactoryUnitMainType.Small;
        _unitSubType = FactoryUnitSubType.SmallBlue;
    }

    public override void Die()
    {
        UnitPool.Instance.ReleaseSmallBlue(this);
    }
}
