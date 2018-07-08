using UnityEditor;
using UnityEngine;
using Core;

[CustomEditor(typeof(GameEventInt))]
public class EventEditorInt : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEventInt e = target as GameEventInt;

        if (GUILayout.Button("Invoke"))
            e.Invoke(e.Value);
    }
}

