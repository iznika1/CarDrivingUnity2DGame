using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    //C:/Users/xyz/AppData/LocalLow/DefaultCompany/CarDriving
    private string filePath;
    public List<Score> Scores { get; private set; }
    public static ScoreManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this.filePath = Path.Combine(Application.persistentDataPath, "scores.txt");
        if (File.Exists(filePath))
            this.LoadScores();

        if(this.Scores == null)
        {
            this.Scores = new List<Score>();
        }


    }

    public void LoadScores()
    {
        using (FileStream fs = new FileStream(this.filePath, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            this.Scores = (List<Score>)bf.Deserialize(fs);
        }
    }

    public void UpdateScores(Score score)
    {
        if(score == null)       
            throw new ArgumentNullException("score");

        if(!IsItemExists(score))
            this.Scores.Add(score);
     
        this.Scores.Sort((x, y) => DateTime.Compare(x.Time, y.Time));
        this.SaveScores();
    }

    private void SaveScores()
    {
        using (FileStream fs = new FileStream(this.filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, this.Scores);
        }
    }

    public Score LoadBestTime()
    {
        return Scores[0];
    }

    private bool IsItemExists(Score score)
    {
        foreach (var item in Scores)
        {
            if (item.Time.Equals(score.Time))
                return true;

        }

        return false;
    }



    [Serializable]
    public class Score
    {
        public int CoinCount { get; set; }
        public DateTime Time { get; set; }

        public Score() {   }
        public Score(int coinCount, DateTime time)
        {
            this.CoinCount = coinCount;
            this.Time = time;
        }

        public override string ToString()
        {
            return String.Format("{0}\t{1}:{2}", CoinCount, Time.Minute, Time.Second);
        }
    }
}
