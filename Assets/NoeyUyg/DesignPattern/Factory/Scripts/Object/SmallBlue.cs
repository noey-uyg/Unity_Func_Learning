using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBlue : Unit
{
    public override void Init()
    {
        _hp = 20;
        _range = 1;
        _speed = 1;
        _damage = 1;
        _unitType = FactoryUnitType.SmallBlue;
    }
}
