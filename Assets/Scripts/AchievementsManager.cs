using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager : MonoBehaviour {

    public List<GameObject> achievents;

    void Start()
    {
        LoadPlayerCollectedAchievements();
    }

    private void LoadPlayerCollectedAchievements()
    {
        AchievementManager.Achievement achievement = AchievementManager.instance.LoadAchievement(AchievementManager.Achievement.AchievementType.RocketBoost);

        foreach (GameObject gameObjectAchievement in achievents)
        {
            if (gameObjectAchievement.name == achievement.Name.ToString() && achievement.Value != -1)
                gameObjectAchievement.SetActive(true);
            else
                gameObjectAchievement.SetActive(false);

        }

    }

    public void DeleteAllAchievements()
    {
        PlayerPrefs.DeleteKey(AchievementManager.Achievement.AchievementType.RocketBoost.ToString());

        LoadPlayerCollectedAchievements();
    }
		
}
