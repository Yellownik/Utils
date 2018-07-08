using UnityEditor;
using UnityEngine;
using Core;

[CustomEditor(typeof(GameEventFloat))]
public class EventEditorFloat: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEventFloat e = target as GameEventFloat;

        if (GUILayout.Button("Invoke"))
            e.Invoke(e.Value);
    }
}

