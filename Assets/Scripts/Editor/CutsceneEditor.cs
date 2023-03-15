using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cutscene))]
public class CutsceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var cutscene = target as Cutscene;
        
        if (GUILayout.Button("Speech Bubble")) cutscene.AddAction(new DialogueAction());
        if (GUILayout.Button("Move Object")) cutscene.AddAction(new MoveObjAction());
        if (GUILayout.Button("Set Animation")) cutscene.AddAction(new PlayAnimAction());
        if (GUILayout.Button("Wait")) cutscene.AddAction(new WaitAction());

        base.OnInspectorGUI();
    }
}
