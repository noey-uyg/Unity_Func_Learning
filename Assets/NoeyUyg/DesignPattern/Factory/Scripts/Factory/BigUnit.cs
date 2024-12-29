using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigUnit : UnitFactory
{
    [SerializeField] private BigBlue _smallBlue;
    [SerializeField] private BigGreen _smallGreen;
    [SerializeField] private BigRed _smallRed;

    public override Unit Create(FactoryUnitSubType unitType)
    {
        Unit unit = null;
        switch (unitType)
        {
            case FactoryUnitSubType.BigBlue:
                unit = UnitPool.Instance.GetBigBlue();
                unit.Init();
                break;
            case FactoryUnitSubType.BigRed:
                unit = UnitPool.Instance.GetBigRed();
                unit.Init();
                break;
            case FactoryUnitSubType.BigGreen:
                unit = UnitPool.Instance.GetBigGreen();
                unit.Init();
                break;
        }
        unit.transform.SetParent(transform);
        return unit;
    }
}