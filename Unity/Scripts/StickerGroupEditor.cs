using SelfieGame.Sticker.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(StickerGroup))]
[CanEditMultipleObjects]
public class StickerGroupEditor : Editor
{
    int capacity = 0;
    float imageSize = 70;

    float verticalSpacing = 10;
    float labelWidth = 90;
    
    private void OnEnable()
	{
		capacity = ((StickerGroup)target).Stickers.Count;
	}

    public override void OnInspectorGUI()
    {     
        StickerGroup group = (StickerGroup)target;

        DrawCapacity();
        FixGroupSize(group);
        DrawImages(group);

        EditorUtility.SetDirty(group);
    }

    private void DrawCapacity()
    {
        EditorGUILayout.BeginHorizontal();
            GUILayout.Label("StickerCount:", GUILayout.Width (labelWidth));

            int temp = EditorGUILayout.DelayedIntField(capacity);
            capacity = (temp < 0) ? 0 : temp;

            GUILayout.Space(20);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(verticalSpacing);
    }

    private void FixGroupSize(StickerGroup group)
    {
        int oldCapacity = group.Stickers.Count;
        if (capacity < oldCapacity)
        {
            group.Stickers.RemoveRange(capacity, oldCapacity - capacity);
        }
        else
        {
            group.Stickers.AddRange(new Sprite[capacity - oldCapacity]);
        }
    }

    private void DrawImages(StickerGroup group)
    {
        for (int i = 0; i < capacity; i++)
        {
            string name = "Element " + (i + 1);
            float aspect = 1;

            if (group.Stickers[i] != null)
            {
                name = (i + 1) + ": " + group.Stickers[i].name;
                aspect = (float)group.Stickers[i].rect.width / group.Stickers[i].rect.height;
            }

            EditorGUILayout.BeginHorizontal();
				GUILayout.Label(name, GUILayout.Width (labelWidth));

                group.Stickers[i] = (Sprite)EditorGUILayout.ObjectField (group.Stickers[i], typeof(Sprite), false,
                new GUILayoutOption[] {GUILayout.Height(imageSize), GUILayout.Width(imageSize * aspect)});

				GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(verticalSpacing);
        }
    }
}
