using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreate : MonoBehaviour
{
    [SerializeField] private UnitFactory _smallUnitFactory;
    [SerializeField] private UnitFactory _bigUnitFactory;
    [SerializeField] private Camera _mainCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 targetPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Unit unit = _smallUnitFactory.Create(FactoryUnitType.SmallRed);
            unit.transform.position = targetPos;
        }
    }
}
