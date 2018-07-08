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
    float startX = 20;
    float startY = 50;

    float imageSize = 70;

    float verticalSpacing = 10;
    float horizSpacing = 40;
    
    public override void OnInspectorGUI()
    {     
        float x = startX;
        float y = startY;

        StickerGroup group = (StickerGroup)target;

        Rect position = new Rect(x, y, 70, 15);
        EditorGUI.PrefixLabel(position, new GUIContent("Preview:"));

        position = new Rect(x + 70 + horizSpacing, y, imageSize, imageSize);
        group.preview = (Sprite)EditorGUI.ObjectField(position, group.preview, typeof(Sprite), false);
        y += imageSize + verticalSpacing;

        position = new Rect(x, y, 70, 15);
        EditorGUI.PrefixLabel(position, new GUIContent("StickerCount:"));

        position = new Rect(x + 70 + horizSpacing, y, 70, 15);
        int temp = EditorGUI.IntField(position, group.StickerCount);
        group.StickerCount = (temp < 0) ? 0 : temp;
        y += 15 + verticalSpacing;

        FixGroupSize(group);
        DrawImages(x, y, group);

        EditorUtility.SetDirty(group);
    }

    private void FixGroupSize(StickerGroup group)
    {
        int count = group.Stickers.Count;
        if (group.StickerCount < count)
        {
            group.Stickers.RemoveRange(group.StickerCount, count - group.StickerCount);
        }
        else
        {
            group.Stickers.AddRange(new Sprite[group.StickerCount - count]);
        }
    }

    private void DrawImages(float x, float y, StickerGroup group)
    {
        Rect position;
        for (int i = 0; i < group.StickerCount; i++)
        {
            string name = "Element " + (i + 1);
            float aspect = 1;

            if (group.Stickers[i] != null)
            {
                name = (i + 1) + ": " + group.Stickers[i].name;
                aspect = (float)group.Stickers[i].rect.width / group.Stickers[i].rect.height;
            }

            position = new Rect(x + 70 + horizSpacing, y, imageSize * aspect, imageSize);
            group.Stickers[i] = (Sprite)EditorGUI.ObjectField(position, group.Stickers[i], typeof(Sprite), false);

            position = new Rect(x, y + imageSize / 2, 100, 15);
            EditorGUI.PrefixLabel(position, new GUIContent(name));

            y += imageSize + verticalSpacing;
        }
    }
}
