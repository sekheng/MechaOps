﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "UnitsToSpawn", menuName = "Spawn System/Units To Spawn")]
public class UnitsToSpawn : ScriptableObject
{
    [SerializeField] private UnitType[] m_UnitList = null;

    public UnitType[] GetUnitList()
    {
        return m_UnitList;
    }

    public void SetUnitList(UnitType[] _unitList)
    {
        m_UnitList = _unitList;
    }
}