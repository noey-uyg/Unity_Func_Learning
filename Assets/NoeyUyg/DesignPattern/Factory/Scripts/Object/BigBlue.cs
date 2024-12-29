using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlue : Unit
{
    public override void Init()
    {
        _hp = 40;
        _range = 1;
        _speed = 1;
        _damage = 2;
        _unitType = FactoryUnitType.BigBlue;
    }
}

