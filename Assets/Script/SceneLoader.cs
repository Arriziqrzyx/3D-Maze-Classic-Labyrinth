using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string sceneToLoad;

    public static string SceneToLoad { get => sceneToLoad; }

    // Load
    public static void Load(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public static void ProgressLoad(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene("LoadingProgress");
    }
    
    public static void ReloadLevel()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        ProgressLoad(currentScene);
    }

   
    public static void LoadNextLevel()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        int nextLevel = int.Parse(currentSceneName.Split("Level")[1]) + 1;
        string nextSceneName = "Level" + nextLevel;

        if (SceneUtility.GetBuildIndexByScenePath(nextSceneName) == -1)
        {
            Debug.LogError(nextSceneName + " does not exsits");
            return;
        }

        ProgressLoad(nextSceneName);

    }
}
