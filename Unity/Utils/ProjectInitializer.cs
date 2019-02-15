using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectInitializer : MonoBehaviour
{
    [MenuItem("MyProjectUtils/Create Folder &F")]
    static void CreateFolder(MenuCommand command)
    {
        ProjectWindowUtil.CreateFolder();
    }

    [MenuItem("MyProjectUtils/InitializeProjectFolders")]
    static void InitializeProjectFolders(MenuCommand command)
    {
        CreateFolderWithName("Shaders");
        CreateFolderWithName("Scenes");
        CreateFolderWithName("Plugins");
        CreateFolderWithName("3rd-Party");

        string pathToScripts = CreateFolderWithName("Scripts");
        CreateFolderWithName("Editor", pathToScripts);
        CreateFolderWithName("ScriptableObjects", pathToScripts);
        CreateFolderWithName("Utils", pathToScripts);

        string pathToData = CreateFolderWithName("Data");
        CreateFolderWithName("Textures", pathToData);
        CreateFolderWithName("Materials", pathToData);
        CreateFolderWithName("Models", pathToData);
        CreateFolderWithName("Prefabs", pathToData);
        CreateFolderWithName("Resources", pathToData);
    }

    private static string CreateFolderWithName(string folderName, string parentName = "Assets")
    {
        if (!AssetDatabase.IsValidFolder(parentName + "/" + folderName))
        {
            string guid = AssetDatabase.CreateFolder(parentName, folderName);
            return AssetDatabase.GUIDToAssetPath(guid);
        }
        else
        {
            return parentName + "/" + folderName;
        }
    }
}
