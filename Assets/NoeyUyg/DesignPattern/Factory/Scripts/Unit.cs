using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Unit : MonoBehaviour
{
    protected int _hp;
    protected int _range;
    protected int _speed;
    protected int _damage;
    protected FactoryUnitType _unitType;

    public virtual void Init() { }    
}
