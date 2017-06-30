using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockedManager : MonoBehaviour {

    public List<Button> levelsButtons;

	void Start () {
        showUnlockedLevels();
	}

    private void showUnlockedLevels()
    {
        int lastPassedLevel = PlayerPrefs.GetInt("LastPassedLevel", 0);

        if(lastPassedLevel != 0)
        {
            switch (lastPassedLevel)
            {
                case 1:
                    levelsButtons[1].interactable = true;
                    break;
                case 2:
                    levelsButtons[1].interactable = true;
                    levelsButtons[2].interactable = true;
                    break;
            }
        }
        else
        {
            levelsButtons[0].interactable = true;
        }
    }
}
