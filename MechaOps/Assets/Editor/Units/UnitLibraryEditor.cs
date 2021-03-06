﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitLibrary))]
public class UnitLibraryEditor : Editor
{
    SerializedProperty m_LibraryProperty;

    void OnEnable()
    {
        m_LibraryProperty = serializedObject.FindProperty("m_Library");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < m_LibraryProperty.arraySize; ++i)
        {
            SerializedProperty property = m_LibraryProperty.GetArrayElementAtIndex(i);
            string propertyName = ((UnitType)i).ToString();
            EditorGUILayout.PropertyField(property, new GUIContent(propertyName), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}