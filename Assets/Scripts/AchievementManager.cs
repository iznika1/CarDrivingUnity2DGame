using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{

    public static AchievementManager instance;

    public List<Achievement> achievements;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (achievements == null || achievements.Count == 0)
            achievements = new List<Achievement>();

        achievements.Add(new Achievement(Achievement.AchievementType.RocketBoost, 20, null));
    }

    public Achievement LoadAchievement(Achievement.AchievementType achievementName)
    {
        string name = achievementName.ToString();

        int achievementValue =  PlayerPrefs.GetInt(name, -1);

        return new Achievement(achievementName, achievementValue, null);
    }

    public int LoadAchievementValue(Achievement.AchievementType achievementName)
    {
        string name = achievementName.ToString();

        int achievementValue = PlayerPrefs.GetInt(name, -1);

        if (achievementValue == -1)
            return achievementValue;

        Achievement achievement = achievements.Find(a => a.Name.Equals(achievementName));

        if (achievement.Value != achievementValue)
            SavePlayerAchievement(achievementName);

        return achievementValue;
    }

    public void SavePlayerAchievement(Achievement.AchievementType achievementName)
    {
        string name = achievementName.ToString();

        Achievement achievement = achievements.Find(a => a.Name.Equals(achievementName));

        PlayerPrefs.SetInt(name, achievement.Value);

        PlayerPrefs.Save();
    }

    public void RemoveAchievements()
    {
        PlayerPrefs.DeleteAll();
    }

    [Serializable]
    public struct Achievement
    {
        public enum AchievementType
        {
            RocketBoost
        }

        public AchievementType Name;
        public GameObject Prefab;
        public int Value;

        public Achievement(AchievementType name, int value, GameObject prefab)
        {
            this.Name = name;
            this.Value = value;
            this.Prefab = prefab;
        }

        public override string ToString()
        {
            return String.Format("{0}\t{1}", Name, Value);
        }
    }



}
