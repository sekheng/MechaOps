﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// To show the tile info on the UI logic
/// </summary>
public class ShowTileInfoUI : MonoBehaviour {
    [System.Serializable]
    public struct TileTypeInfo
    {
        public TileType m_TileType;
        public string m_TileDescription;
    }
    [System.Serializable]
    public struct HazardTypeInfo
    {
        public HazardType m_HazardType;
        public string m_HazardDescription;
    }

    [Header("Variables for ShowTileInfoUI")]
    [SerializeField, Tooltip("Tile information gameobject")]
    protected GameObject m_TileInfoGO;
    [SerializeField, Tooltip("Minimize button")]
    protected Button m_MinimizeButton;
    [SerializeField, Tooltip("Expand button")]
    protected Button m_ExpandButton;
    [SerializeField, Tooltip("Text UI for the display of tile name")]
    protected TextMeshProUGUI m_TileNameTxt;
    [SerializeField, Tooltip("Text UI for tile movement cost")]
    protected TextMeshProUGUI m_MoveCostTxt;
    [SerializeField, Tooltip("Text UI for tile concealment")]
    protected TextMeshProUGUI m_TileConcealTxt;
    [SerializeField, Tooltip("Text UI for tile description")]
    protected TextMeshProUGUI m_TileDescriptionTxt;
    [SerializeField, Tooltip("Audio to play the minimizing sound effect")]
    protected AudioClip m_MinimizeSFX;
    [SerializeField, Tooltip("Audio to play the expand sound effect")]
    protected AudioClip m_ExpandSFX;
    [SerializeField, Tooltip("Audio Source to play SFX")]
    protected AudioSource m_SFXSource;
    [SerializeField, Tooltip("Array of the Tile type information")]
    protected TileTypeInfo[] m_ArrayOfTileTypeInfo;
    [SerializeField, Tooltip("Array of hazard type information")]
    protected HazardTypeInfo[] m_ArrayOfHazardTypeInfo;

    [Header("Debugging for ShowTileInfoUI")]
    [SerializeField, Tooltip("Tile that the player has clicked!")]
    protected Tile m_ClickedTile;
    [SerializeField, Tooltip("Contains all of the game event names asset. Will try to link from GameSystemDirectory if nothing is here")]
    protected GameEventNames m_EventAsset;
    [SerializeField, Tooltip("Tweening to disable this script")]
    protected TweenDisableScript m_tweenDisableScript;
    [SerializeField, Tooltip("Flag to check whether is it ready to show the tile info")]
    protected bool m_ReadyShowTileInfo = true;

    private void Awake()
    {
        if (!m_EventAsset)
        {
            m_EventAsset = GameSystemsDirectory.GetSceneInstance().GetGameEventNames();
        }
        InitEvents();
        if (!m_tweenDisableScript)
        {
            m_tweenDisableScript = m_TileInfoGO.GetComponent<TweenDisableScript>();
        }
        if (!m_SFXSource)
        {
            m_SFXSource = GameSystemsDirectory.GetSceneInstance().GetSFXSource();
        }
#if UNITY_ASSERTIONS
        Assert.IsNotNull(m_EventAsset, "Event asset is still null at ShowTileInfoUI.Awake()");
        Assert.IsNotNull(m_tweenDisableScript, "The tween disable script is null at ShowTileInfoUI.Awake()");
#endif
    }

    private void Start()
    {
        // Set it back to inactive afterwards
        m_TileInfoGO.SetActive(false);
    }

    protected void InitEvents()
    {
        GameEventSystem.GetInstance().SubscribeToEvent<GameObject>(m_EventAsset.GetEventName(GameEventNames.GameUINames.ClickedTile), GetClickedTileGO);
        GameEventSystem.GetInstance().SubscribeToEvent<FactionType>(m_EventAsset.GetEventName(GameEventNames.GameplayNames.GameOver), DestroyItself);
        GameEventSystem.GetInstance().SubscribeToEvent<FactionType>(m_EventAsset.GetEventName(GameEventNames.GameplayNames.TurnStart), AnimateToInactive);
        GameEventSystem.GetInstance().SubscribeToEvent<FactionType>(m_EventAsset.GetEventName(GameEventNames.GameplayNames.TurnEnd), AnimateToInactive);
        GameEventSystem.GetInstance().SubscribeToEvent<UnitStats>(m_EventAsset.GetEventName(GameEventNames.GameplayNames.UnitFinishedAction), SetReadyToShowTrue);
        GameEventSystem.GetInstance().SubscribeToEvent(m_EventAsset.GetEventName(GameEventNames.GameplayNames.UnitStartAction), SetReadyToShowFalse);
    }

    protected void DeinitEvents()
    {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<GameObject>(m_EventAsset.GetEventName(GameEventNames.GameUINames.ClickedTile), GetClickedTileGO);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<FactionType>(m_EventAsset.GetEventName(GameEventNames.GameplayNames.GameOver), DestroyItself);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<FactionType>(m_EventAsset.GetEventName(GameEventNames.GameplayNames.TurnStart), AnimateToInactive);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<FactionType>(m_EventAsset.GetEventName(GameEventNames.GameplayNames.TurnEnd), AnimateToInactive);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<UnitStats>(m_EventAsset.GetEventName(GameEventNames.GameplayNames.UnitFinishedAction), SetReadyToShowTrue);
        GameEventSystem.GetInstance().UnsubscribeFromEvent(m_EventAsset.GetEventName(GameEventNames.GameplayNames.UnitStartAction), SetReadyToShowFalse);
    }

    /// <summary>
    /// We are not using OnDisable because this gameobject will become active and inactive repeatedly
    /// </summary>
    private void OnDestroy()
    {
        DeinitEvents();
    }

    protected void GetClickedTileGO(GameObject _tileGO)
    {
        if (_tileGO.tag == "Tile" || _tileGO.tag == "TileDisplay")
        {
            if (_tileGO.tag == "TileDisplay")
            {
                // get the parent since parent will be the tile!
                m_ClickedTile = _tileGO.transform.parent.GetComponent<Tile>();
            }
            else
            {
                m_ClickedTile = _tileGO.GetComponent<Tile>();
            }
            // we need to know whether is it a known / unknown tile
            if (!m_ClickedTile.Known)
            {
                m_TileNameTxt.text = "Unknown Tile";
                m_TileConcealTxt.text = "Concealment: ?";
                m_MoveCostTxt.text = "Movement Cost: ?";
                m_TileDescriptionTxt.text = "";
            }
            else
            {
                // then set the values for display!
                m_TileNameTxt.text = m_ClickedTile.GetTileType().ToString() + " Tile";
                m_TileConcealTxt.text = "Concealment: " + m_ClickedTile.GetTotalConcealmentPoints();
                m_MoveCostTxt.text = "Movement Cost: " + m_ClickedTile.GetTotalMovementCost();
                foreach (TileTypeInfo tileTypeInfo in m_ArrayOfTileTypeInfo)
                {
                    if (tileTypeInfo.m_TileType == m_ClickedTile.GetTileType())
                    {
                        m_TileDescriptionTxt.text = tileTypeInfo.m_TileDescription + "\n";
                        break;
                    }
                }
                // check whether it is visible to the player otherwise not much information will be shown
                if (m_ClickedTile.VisibleCounter > 0)
                {
                    // we show whether is it hazardous to walk through it or not
                    foreach (HazardTypeInfo hazardStuff in m_ArrayOfHazardTypeInfo)
                    {
                        if (hazardStuff.m_HazardType == m_ClickedTile.GetHazardType())
                        {
                            m_TileDescriptionTxt.text += hazardStuff.m_HazardDescription + "\n";
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// When player pressed the close button on this UI
    /// </summary>
    public void MinimizeThis()
    {
        m_MinimizeButton.gameObject.SetActive(false);
        m_ExpandButton.gameObject.SetActive(true);
        AnimateToInactive();
        m_SFXSource.PlayOneShot(m_MinimizeSFX);
    }

    public void ExpandThis()
    {
        m_MinimizeButton.gameObject.SetActive(true);
        m_ExpandButton.gameObject.SetActive(false);
        m_TileInfoGO.SetActive(true);
        m_SFXSource.PlayOneShot(m_ExpandSFX);
    }

    /// <summary>
    /// Animating the tile info UI to inactive automatically
    /// </summary>
    public void AnimateToInactive()
    {
        if (m_tweenDisableScript.gameObject.activeSelf)
        {
            m_tweenDisableScript.AnimateUI();
            m_ClickedTile = null;
        }
    }

    protected void AnimateToInactive(FactionType _whatFaction)
    {
        AnimateToInactive();
    }

    protected void DestroyItself(FactionType _factionDead)
    {
        Destroy(gameObject);
    }

    protected void SetReadyToShowTrue(UnitStats _unitFinishedActStat)
    {
        m_ReadyShowTileInfo = true;
    }

    protected void SetReadyToShowFalse()
    {
        m_ReadyShowTileInfo = false;
    }
}
