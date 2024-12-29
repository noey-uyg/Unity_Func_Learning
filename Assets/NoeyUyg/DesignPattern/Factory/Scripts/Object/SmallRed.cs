using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRed : Unit
{
    public override void Init()
    {
        _hp = 10;
        _range = 5;
        _speed = 1;
        _damage = 3;
        _unitType = FactoryUnitType.SmallRed;
    }
}
