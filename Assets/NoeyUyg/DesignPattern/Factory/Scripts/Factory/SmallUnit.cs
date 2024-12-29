using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallUnit : UnitFactory
{
    [SerializeField] private SmallBlue _smallBlue;
    [SerializeField] private SmallGreen _smallGreen;
    [SerializeField] private SmallRed _smallRed;

    public override Unit Create(FactoryUnitSubType unitType)
    {
        Unit unit = null;
        switch (unitType)
        {
            case FactoryUnitSubType.SmallBlue:
                unit = UnitPool.Instance.GetSmallBlue();
                unit.Init();
                break;
            case FactoryUnitSubType.SmallRed:
                unit = UnitPool.Instance.GetSmallRed();
                unit.Init();
                break;
            case FactoryUnitSubType.SmallGreen:
                unit = UnitPool.Instance.GetSmallGreen();
                unit.Init();
                break;
        }
        unit.transform.SetParent(transform);
        return unit;
    }
}
