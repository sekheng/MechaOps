﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// What it does is that the Unit UI Action will store the action from the unit itself!
/// </summary>
public class UnitActionUILogic : MonoBehaviour {
    [Tooltip("To get the unit's action reference")]
    public IUnitAction m_unitActionRef;

    /// <summary>
    /// It will only set the UI to be active according to m_unitActionRef.unitActionName if the tag of the UI is the same!
    /// </summary>
    public void ActivateGameObjWithTag()
    {
        // Too lazy to use a system, use ObserverSystem to pass message for now! TODO: Think of a better solution than this.
        ObserverSystemScript.Instance.StoreVariableInEvent(m_unitActionRef.m_UnitActionName, m_unitActionRef);
        GameUI_Manager.Instance.SetTheGameObjTagActive(m_unitActionRef.m_UnitActionName);
    }
}
