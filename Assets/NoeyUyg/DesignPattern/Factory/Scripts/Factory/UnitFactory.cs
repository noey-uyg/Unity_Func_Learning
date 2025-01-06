using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitFactory : MonoBehaviour
{
    public abstract Unit Create(FactoryUnitSubType unitType);
    public abstract void Release(Unit unit);
}
