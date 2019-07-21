using UnityEngine;
using UnityEditor;
using System;

namespace HangMan.ResourceStorage
{
    /// <summary>
    /// Custom Editor generates GUI and fields with labels of unique TEnum names.
    /// Overrided script should contatin fields:
    ///  Keys - List of Array of TEnum, where TEnum is any enum type;
    ///  Values - List of Array of TObject, where TObject is any Unity object type;
    /// </summary>
    public abstract class EnumObjectPairEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var keys = serializedObject.FindProperty("Keys");
            var values = serializedObject.FindProperty("Values");
            var names = keys.enumNames;

            FixArraySize(keys, names.Length);
            FixArraySize(values, names.Length);
            DrawFields(names, keys, values);

            serializedObject.ApplyModifiedProperties();
        }

        private void FixArraySize(SerializedProperty prop, int newCount)
        {
            int keysCount = prop.arraySize;
            if (newCount < keysCount)
            {
                for (int i = 0; i < keysCount - newCount; i++)
                {
                    prop.DeleteArrayElementAtIndex(prop.arraySize - 1);
                }
            }
            else
            {
                for (int i = 0; i < newCount - keysCount; i++)
                {
                    prop.InsertArrayElementAtIndex(prop.arraySize);
                }
            }
        }

        private void DrawFields(string[] names, SerializedProperty keys, SerializedProperty values)
        {
            for (int i = 0; i < names.Length; i++)
            {
                keys.GetArrayElementAtIndex(i).enumValueIndex = i;
                EditorGUILayout.ObjectField(values.GetArrayElementAtIndex(i), new GUIContent(names[i]));
            }
        }
    }
}
