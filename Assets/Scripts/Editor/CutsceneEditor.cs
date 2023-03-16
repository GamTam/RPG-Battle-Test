using Cinemachine.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Cutscene))]
public class CutsceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Cutscene cutscene = target as Cutscene;
        
        if (GUILayout.Button("Open Editor")) CutsceneEditorWindow.Open(cutscene);
        
        /*toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);

        switch (toolbarInt)
        {
            case 0:
                string dialogue = EditorGUILayout.TextArea("");
                GUILayout.Button("Add Speech Bubble Action");
                break;

            case 1:
                GUILayout.Button("Add Movement Action");
                break;

            case 2:
                GUILayout.Button("Add Animation Action");
                break;
            case 3:
                GUILayout.Button("Add Wait Action");
                break;
        }*/
        
        if (GUILayout.Button("Speech Bubble")) cutscene.AddAction(new DialogueAction());
        if (GUILayout.Button("Move Object")) cutscene.AddAction(new MoveObjAction());
        if (GUILayout.Button("Set Animation")) cutscene.AddAction(new PlayAnimAction());
        if (GUILayout.Button("Wait")) cutscene.AddAction(new WaitAction());

        base.OnInspectorGUI();
    }
}

public class CutsceneEditorWindow : ExtendedEditorWindow
{
    int toolbarInt = 0;
    string[] toolbarStrings = {"Speech Bubble", "Move Object", "Set Animation", "Wait"};
    
    public static void Open(Cutscene cutscene)
    {
        CutsceneEditorWindow window = GetWindow<CutsceneEditorWindow>("Cutscene Editor");
        window._serializedObject = new SerializedObject(cutscene);
    }
    
    public void OnGUI()
    {
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);

        switch (toolbarInt)
        {
            case 0:
                string textArea = "";
                textArea = GUILayout.TextField(textArea);

                /* DrawField("Name", false, serialDialogue);
                DrawField("PlayWithNext", false, serialDialogue);
                DrawField("Speaker", false, serialDialogue);
                DrawField("SkipCloseAnimation", false, serialDialogue);
                DrawField("Dialogue", false, serialDialogue); */
                
                GUILayout.Button("Insert Above");
                GUILayout.Button("Insert Below");
                GUILayout.Button("Append to Bottom");
                break;

            case 1:
                GUILayout.Button("Add Movement Action");
                break;

            case 2:
                GUILayout.Button("Add Animation Action");
                break;
            case 3:
                GUILayout.Button("Add Wait Action");
                break;
        }

        GUILayout.Space(15);
        GUILayout.Label("Current Actions");

        _currentProperty = _serializedObject.FindProperty("CutsceneActions");
        
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        DrawField("_triggerMoreThanOnce", false);
        DrawSidebar(_currentProperty);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if (selectedProperty != null)
        {
            DrawProperties(selectedProperty, true);
            GUILayout.Space(15);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Move Up"))
            {
            }

            if (GUILayout.Button("Move Down"))
            {
            }
            
            if (GUILayout.Button("Remove Action"))
            {
                var oldLength = _currentProperty.arraySize;
                Debug.Log(selectedPropertyIndex);
                _currentProperty.DeleteArrayElementAtIndex(selectedPropertyIndex);
                if (_currentProperty.arraySize == oldLength)
                    _currentProperty.DeleteArrayElementAtIndex(selectedPropertyIndex);
                selectedProperty = null;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.LabelField("Select action to view details");
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();

        /* if (GUILayout.Button("Speech Bubble")) cutscene.AddAction(new DialogueAction());
        if (GUILayout.Button("Move Object")) cutscene.AddAction(new MoveObjAction());
        if (GUILayout.Button("Set Animation")) cutscene.AddAction(new PlayAnimAction());
        if (GUILayout.Button("Wait")) cutscene.AddAction(new WaitAction()); */

        // base.OnInspectorGUI();
    }

    void DrawSelectedPropertiesPanel()
    {
        _currentProperty = selectedProperty;

        EditorGUILayout.BeginHorizontal("box");
        
        DrawField("PlayWithNext", true);
        
        EditorGUILayout.EndHorizontal();
    }
}
