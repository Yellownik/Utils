using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Dropdown Dropdown;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        List<string> scenes = new List<string>();
        var sceneNumber = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneNumber; i++)
        {
            scenes.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }

        Dropdown.ClearOptions();
        Dropdown.AddOptions(scenes);

        Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int id)
    {
        SceneManager.LoadScene(Dropdown.options[id].text);
    }
}
