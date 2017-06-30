using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HighscoresScript : MonoBehaviour {

    public Text highScoresLabel;
    public int highScoreCount = 10;

	void Start () {
        LoadScores();
	}

    private void LoadScores()
    {
        StringBuilder scores = new StringBuilder();
        int count = 1;
        foreach (var score in ScoreManager.instance.Scores)
        {
            if (count > highScoreCount)
                break;

            string timeToString = string.Format("{0:00} : {1:00}", score.Time.Minute, score.Time.Second);

            if(count != highScoreCount)
                scores.AppendFormat("{0}.\t\t{1}", count, timeToString);
            else
                scores.AppendFormat("{0}.\t{1}", count, timeToString);

            scores.AppendLine();

            count++;
        }



        highScoresLabel.text = scores.ToString();

    }
}
