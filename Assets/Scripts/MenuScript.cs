using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public static MenuScript currentMenuScript;

    private void Awake()
    {
        PlayerPrefs.DeleteKey("TotalCoins");
        currentMenuScript = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void OnNewGameClick()
    {
        LoadScene(Scenes.LEVEL_ONE_SCENE);
    }

    public void OnHighscoreClick()
    {
        LoadScene(Scenes.HIGHSCORE_SCENE);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnAchievementClick()
    {
        LoadScene(Scenes.ACHIEVEMENT_SCENE);
    }

    public void OnLoadLevelClick()
    {
        LoadScene(Scenes.LOAD_LEVEL_SCENE);
    }

    public void LoadScene(string sceneName)
    {
        LoadedScene = sceneName;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void LoadScene(int sceneIndex)
    {
        LoadedSceneIndex = sceneIndex;
        SceneManager.LoadScene(LoadedSceneIndex);
    }

    public void RestartCurrentScene()
    {
        Scene sceneLoaded = SceneManager.GetActiveScene();
        LoadScene(sceneLoaded.buildIndex);
    }

    public void GoToMenuScene()
    {
        LoadScene(Scenes.MENU_SCENE);
    }

    public int CurrentLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        switch (currentLevel)
        {
            case 2:
                currentLevel = 1;
                break;
            case 3:
                currentLevel = 2;
                break;
            case 4:
                currentLevel = 3;
                break;
        }

        return currentLevel;
    }

    private string LoadedScene { get; set; }
    private int LoadedSceneIndex { get; set; }




}
