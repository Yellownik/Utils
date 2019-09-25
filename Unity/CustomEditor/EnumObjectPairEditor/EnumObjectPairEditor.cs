using UnityEngine;
using UnityEditor;

namespace Utils.CustomEditor
{
    /// <summary>
    /// Custom Editor generates GUI and fields with labels of unique TEnum names.
    /// These names will be updated with changes in the original enum. 
    /// Overrided script should contatin fields:
    ///  [SerializeField] List<TEnum> Keys, where TEnum is any enum type;
    ///  [SerializeField] List<TObject> Values, where TObject is any Unity object type;
    /// </summary>
    public abstract class EnumObjectPairEditor : Editor
    {
        public virtual string KeysName { get; protected set; } = "Keys";
        public virtual string ValuesName { get; protected set; } = "Values";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var keys = serializedObject.FindProperty(KeysName);
            var values = serializedObject.FindProperty(ValuesName);
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
                GenerateField(names[i], values.GetArrayElementAtIndex(i));
            }
        }

        protected virtual void GenerateField(string name, SerializedProperty elem)
        {
            EditorGUILayout.PropertyField(elem, new GUIContent(name));
        }
    }
}
