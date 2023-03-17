using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject _serializedObject;
    protected SerializedProperty _currentProperty;

    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;
    protected int selectedPropertyIndex = -70;

    protected void DrawProperties(SerializedProperty property, bool drawChildren)
    {
        string lastPropertyPath = "";

        foreach (SerializedProperty p in property)
        {
            /* if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                lastPropertyPath = p.propertyPath;
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    ReorderableList list = new ReorderableList(_serializedObject, p, false, false, true, true);
                    EditorGUI.indentLevel += 1;
                    DrawProperties(p, drawChildren);
                    EditorGUI.indentLevel -= 1;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropertyPath) && p.propertyPath.Contains(lastPropertyPath)) continue;
                lastPropertyPath = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            } */
            
            if (!string.IsNullOrEmpty(lastPropertyPath) && p.propertyPath.Contains(lastPropertyPath)) continue;
            lastPropertyPath = p.propertyPath;
            EditorGUILayout.PropertyField(p, drawChildren);
        }
    }

    protected void DrawSidebar(SerializedProperty property)
    {
        List<string> toolbarStrings = new List<string>();
        List<string> internalStrings = new List<string>();
        
        foreach (SerializedProperty p in property)
        {
            toolbarStrings.Add(p.displayName);
            internalStrings.Add(p.propertyPath);
            // if (GUILayout.Button(p.displayName))
            // {
            //     selectedPropertyPath = p.propertyPath;
            //     selectedPropertyIndex = i;
            // }
            //
            // i++;
        }
        
        selectedPropertyIndex = GUILayout.SelectionGrid(selectedPropertyIndex, toolbarStrings.ToArray(), 1);

        if (selectedPropertyIndex < 0 || selectedPropertyIndex >= internalStrings.Count) selectedProperty = null;
        else selectedProperty = _serializedObject.FindProperty(internalStrings[selectedPropertyIndex]);
    }

    protected void DrawField(string propertyName, bool relative, SerializedObject obj = null)
    {
        if (obj == null)
        {
            obj = _serializedObject;
        }
        if (relative && _currentProperty != null) EditorGUILayout.PropertyField(_currentProperty.FindPropertyRelative(propertyName), true);
        else if (obj != null) EditorGUILayout.PropertyField(obj.FindProperty(propertyName), true);
    }
}
