using SelfieGame.Sticker.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(SpriteGroup))]
[CanEditMultipleObjects]
public class SpriteGroupEditor : Editor
{
    int capacity = 0;
    float imageSize = 70;

    float verticalSpacing = 10;
    float labelWidth = 90;

    private void OnEnable()
    {
        capacity = ((SpriteGroup)target).Sprites.Count;
    }

    public override void OnInspectorGUI()
    {
        SpriteGroup group = (SpriteGroup)target;

        DrawCapacity();
        FixGroupSize(group);
        DrawImages(group);

        EditorUtility.SetDirty(group);
    }

    private void DrawCapacity()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Sprite Count:", GUILayout.Width(labelWidth));

        int temp = EditorGUILayout.DelayedIntField(capacity);
        capacity = (temp < 0) ? 0 : temp;

        GUILayout.Space(20);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(verticalSpacing);
    }

    private void FixGroupSize(SpriteGroup group)
    {
        int oldCapacity = group.Sprites.Count;
        if (capacity < oldCapacity)
        {
            group.Sprites.RemoveRange(capacity, oldCapacity - capacity);
        }
        else
        {
            group.Sprites.AddRange(new Sprite[capacity - oldCapacity]);
        }
    }

    private void DrawImages(SpriteGroup group)
    {
        for (int i = 0; i < capacity; i++)
        {
            string name = "Element " + (i + 1);
            float aspect = 1;

            if (group.Sprites[i] != null)
            {
                name = (i + 1) + ": " + group.Sprites[i].name;
                aspect = (float)group.Sprites[i].rect.width / group.Sprites[i].rect.height;
            }

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(name, GUILayout.Width(labelWidth));
                GUILayout.FlexibleSpace();
                group.Sprites[i] = (Sprite)EditorGUILayout.ObjectField(
                    group.Sprites[i],
                    typeof(Sprite),
                    false,
                    new GUILayoutOption[] { GUILayout.Height(imageSize), GUILayout.Width(imageSize * aspect) }
                    );
                GUILayout.Space(20);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(verticalSpacing);
        }
    }
}
