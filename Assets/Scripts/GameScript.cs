using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public static GameScript gameManger;

    public GameObject gameOverPanel;
    public GameObject finishLevelPanel;
    public GameObject pauseLevelPanel;
    public GameObject player;
    public GameObject enemyBig;
    public Text timerLabel;
    public Text bestTimeLabel;
    public Text totalCoinsLabel;
    public bool timeEnabled;
    public int coinCount = 0;
    public Text coinTextLabel;
    public int coinForAchievement;
    public GameObject achievementMedalImage;
    public GameObject achievementImage;
    public GameObject achievementText;

    private float time;
    private AchievementManager achievementManager;
    private PlayerController playerController;
    private EnemyController enemyController;
    private ScoreManager scoreManager;


    private void Awake()
    {
        gameManger = this;
        playerController = player.GetComponent<PlayerController>();
        achievementManager = gameManger.GetComponent<AchievementManager>();
        scoreManager = gameManger.GetComponent<ScoreManager>();
    }

    void Start()
    {
        StartGame();
    }

    private void FixedUpdate()
    {
        if (timeEnabled)
        {
            time += Time.deltaTime;

            var minutes = time / 60;
            var seconds = time % 60;

            timerLabel.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        }
    }

    public void GoToMenu()
    {
        MenuScript.currentMenuScript.GoToMenuScene();
    }

    public void TryAgain()
    {
        EnableEnenmy(true);
        gameOverPanel.SetActive(false);
        playerController.EnableMovment(true);
        int totalCoin = PlayerPrefs.GetInt("TotalCoins", 0);

        if (totalCoin >= coinCount)
            totalCoin -= coinCount;
        else
            totalCoin = 0;

        PlayerPrefs.SetInt("TotalCoins", totalCoin);
        MenuScript.currentMenuScript.RestartCurrentScene();
    }


    public void GameOver()
    {
        player.SetActive(false);
        gameOverPanel.SetActive(true);
        playerController.EnableMovment(false);
        timeEnabled = false;
        EnableEnenmy(false);
    }

    public void FinishLevel()
    {
        timeEnabled = false;
        playerController.EnableMovment(false);
        finishLevelPanel.SetActive(true);
        if (CanGetAchievement())
        {
            achievementImage.SetActive(true);
            achievementText.SetActive(true);
            achievementMedalImage.SetActive(true);

            achievementManager.SavePlayerAchievement(AchievementManager.Achievement.AchievementType.RocketBoost);
        }
        else
        {
            achievementImage.SetActive(false);
            achievementText.SetActive(false);
            achievementMedalImage.SetActive(false);
        }

        int totalCoin = PlayerPrefs.GetInt("TotalCoins", 0);
        totalCoin += coinCount;
        PlayerPrefs.SetInt("TotalCoins", totalCoin);

        SaveScore();

        if(PlayerPrefs.GetInt("LastPassedLevel") <= MenuScript.currentMenuScript.CurrentLevel())
            PlayerPrefs.SetInt("LastPassedLevel", MenuScript.currentMenuScript.CurrentLevel());
    }

    public void FinishGame()
    {
        timeEnabled = false;
        Destroy(enemyBig);
        playerController.EnableMovment(false);
        finishLevelPanel.SetActive(true);
        achievementImage.SetActive(true);
        achievementMedalImage.SetActive(true);

        SaveScore();

        ScoreManager.Score bestScore = ScoreManager.instance.LoadBestTime();

        bestTimeLabel.text = String.Format("{0:00} : {1:00}", bestScore.Time.Minute, bestScore.Time.Second);
        totalCoinsLabel.text = PlayerPrefs.GetInt("TotalCoins").ToString();

        if (PlayerPrefs.GetInt("LastPassedLevel") <= MenuScript.currentMenuScript.CurrentLevel())
            PlayerPrefs.SetInt("LastPassedLevel", MenuScript.currentMenuScript.CurrentLevel());
    }

    public void PausedLevel()
    {
        pauseLevelPanel.SetActive(true);
        EnableEnenmy(false);
        timeEnabled = false;
        playerController.EnableMovment(false);
    }

    public void ContinueLevel()
    {
        timeEnabled = true;
        playerController.EnableMovment(true);
        EnableEnenmy(true);
        pauseLevelPanel.SetActive(false);
    }



        private void SaveScore()
    {
        string[] strArray = timerLabel.text.Split(':');
        string stringDate = strArray[0].Trim() + ":" + strArray[1].Trim();

        DateTime dateTime = DateTime.ParseExact(stringDate, "mm:ss", CultureInfo.InvariantCulture);

        ScoreManager.Score score = new ScoreManager.Score();
        score.CoinCount = coinCount;
        score.Time = dateTime;

        scoreManager.UpdateScores(score);
    }

    public bool CanGetAchievement()
    {
        if (coinCount >= coinForAchievement && achievementManager.LoadAchievementValue(AchievementManager.Achievement.AchievementType.RocketBoost) == -1)
            return true;

        return false;
    }

    public void GoToLevel(int scene)
    {
        switch (scene)
        {
            case 1:
                MenuScript.currentMenuScript.LoadScene(Scenes.LEVEL_ONE_SCENE);
                break;
            case 2:
                MenuScript.currentMenuScript.LoadScene(Scenes.LEVEL_TWO_SCENE);
                break;
            case 3:
                MenuScript.currentMenuScript.LoadScene(Scenes.LEVEL_THREE_SCENE);
                break;
        }
    }

    public void StartGame()
    {
        gameOverPanel.SetActive(false);
        finishLevelPanel.SetActive(false);
        player.SetActive(true);
        playerController.EnableMovment(true);
        pauseLevelPanel.SetActive(false);
        ResetTimer();
        EnableEnenmy(true);
    }

    public bool isPlayerActive()
    {
        return player.activeSelf;
    }

    public void ResetTimer()
    {
        time = 0f;
        timerLabel.text = "00:00";
        timeEnabled = true;
    }

    public void EnableEnenmy(bool enable)
    {
        PlayerPrefs.SetInt("EnemyEnabled", enable? 1:0);
    }
}
