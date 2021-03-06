﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class LevelSelectionCanvas : MonoBehaviour
{
    [SerializeField] private MainMenuManager m_MainMenuManager = null;
    [SerializeField] private LevelSelectionLibrary m_LevelSelectionLibrary = null;
    [SerializeField] private LevelSelectionButton m_LevelSelectionButton_Prefab = null;
    [SerializeField] private RectTransform m_ScrollViewContent = null;
    [SerializeField] private float m_ButtonHeightPadding = 10.0f;
    [SerializeField] private Button m_ConfirmButton = null;

    private List<LevelSelectionButton> m_Buttons = new List<LevelSelectionButton>();

    LevelSelectionLibrary GetLevelSelectionLibrary() { return m_LevelSelectionLibrary; }

    private void Awake()
    {
        SpawnLevelSelectionButtons();
    }

    private void OnEnable()
    {
        m_ConfirmButton.gameObject.SetActive(false);
    }

    private void SpawnLevelSelectionButtons()
    {
        LevelSelectionData[] levelSelectionDatas = m_LevelSelectionLibrary.GetLevelSelectionData();
        if (levelSelectionDatas == null) { return; }
        Assert.IsTrue(levelSelectionDatas.Length > 0, MethodBase.GetCurrentMethod().Name + " - levelSelectionDatas.Length must be > 0.");
        for (int i = 0; i < levelSelectionDatas.Length; ++i)
        {
            LevelSelectionButton button = Instantiate(m_LevelSelectionButton_Prefab.gameObject, m_ScrollViewContent).GetComponent<LevelSelectionButton>();
            RectTransform buttonTransform = button.gameObject.GetComponent<RectTransform>();

            button.SetLevelSelectionCanvas(this);
            button.SetMainMenuManager(m_MainMenuManager);
            button.SetLevelSelectionData(levelSelectionDatas[i]);
            Vector3 buttonLocalPosition = buttonTransform.localPosition;
            buttonLocalPosition.y = (float)i * -(buttonTransform.sizeDelta.y + m_ButtonHeightPadding);
            buttonTransform.localPosition = buttonLocalPosition;

            m_ScrollViewContent.sizeDelta = new Vector2(m_ScrollViewContent.sizeDelta.x, ((float)(i) * m_ButtonHeightPadding) + (float)(i + 1) * buttonTransform.sizeDelta.y);

            m_Buttons.Add(button);
        }
    }

    public void OnLevelSelectionButtonClick(LevelSelectionButton _ignoredButton)
    {
        // Allow the user to click the Confirm Button.
        m_ConfirmButton.gameObject.SetActive(true);

        // Toggle off the other levels.
        for (int i = 0; i < m_Buttons.Count; ++i)
        {
            if (m_Buttons[i] == _ignoredButton) { continue; }

            Toggle toggle = m_Buttons[i].gameObject.GetComponent<Toggle>();
            toggle.isOn = false;
        }
    }
}