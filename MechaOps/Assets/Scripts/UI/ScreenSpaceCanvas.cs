﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[DisallowMultipleComponent, RequireComponent(typeof(Canvas))]
public class ScreenSpaceCanvas : MonoBehaviour
{
    [SerializeField] private UnitSelection m_UnitSelection;
    [SerializeField] private ScrollRect m_UnitActionSelectionScrollRect;

    public UnitSelection GetUnitSelection() { return m_UnitSelection; }
    public ScrollRect GetUnitActionSelectionScrollRect() { return m_UnitActionSelectionScrollRect; }
}