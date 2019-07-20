using UnityEngine;
using UnityEditor;

public class AddNameSpace : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");
        if (index < 0)
            return;

        string file = path.Substring(index);
        if (file != ".cs")
            return;

        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);

        string lastPart = path.Substring(path.IndexOf("Assets"));
        string _namespace = lastPart.Substring(0, lastPart.LastIndexOf("/"));
        _namespace = _namespace.Replace("/", ".");
        _namespace = _namespace.Replace(" ", "");
        _namespace = _namespace.Replace("Assets.", "");
        _namespace = _namespace.Replace("Scripts.", "");

        file = file.Replace("#NAMESPACE#", _namespace);
        System.IO.File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }
}