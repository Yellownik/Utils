using UnityEditor;

public class CustomHotkeys
{
    [MenuItem("Utils/Folder #1")]
    static void CreateFolder()
    {
        EditorApplication.ExecuteMenuItem("Assets/Create/Folder");
    }

    [MenuItem("Utils/Script #2")]
    static void CreateScript()
    {
        EditorApplication.ExecuteMenuItem("Assets/Create/C# Script");
    }

    [MenuItem("Utils/Interface #3")]
    static void CreateInterface()
    {
        EditorApplication.ExecuteMenuItem("Assets/Create/C# Interface");
    }
}
