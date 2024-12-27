using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGreen : Unit
{
    public override void Init()
    {
        _hp = 15;
        _range = 3;
        _speed = 2;
        _damage = 2;
        _unitType = FactoryUnitType.SmallGreen;
    }
}