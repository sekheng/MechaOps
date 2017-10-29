﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple testing to get the player to 
/// </summary>
public class GetPlayerInputUnit : MonoBehaviour {
    [Header("Debugging purposes!")]
    [Tooltip("The player clicked on the unit")]
    public GameObject m_ClickedPlayerUnitGO;

    // Update is called once per frame
    void Update () {
        // Touch Input can also use GetMouseButton(0)!
        if (Input.GetMouseButton(0))
        {
            Ray clickedRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit clickedObj;
            if (Physics.Raycast(clickedRay, out clickedObj))
            {
                m_ClickedPlayerUnitGO = clickedObj.collider.gameObject;
                ObserverSystemScript.Instance.StoreVariableInEvent("ClickedUnit", m_ClickedPlayerUnitGO);
                ObserverSystemScript.Instance.TriggerEvent("ClickedUnit");
            }
        }
	}
}
