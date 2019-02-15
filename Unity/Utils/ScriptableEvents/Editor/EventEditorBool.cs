using UnityEditor;
using UnityEngine;
using Core;

[CustomEditor(typeof(GameEventBool))]
public class EventEditorBool: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEventBool e = target as GameEventBool;

        if (GUILayout.Button("Invoke"))
            e.Invoke(e.Value);
    }
}

