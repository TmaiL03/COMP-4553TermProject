using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI scoresText;

    void Start()
    {
        int totalPlayers = PlayerPrefs.GetInt("TotalPlayers", 0);
        string scoreDisplay = "Final Scores:\n\n";

        for (int i = 0; i < totalPlayers; i++)
        {
            int score = PlayerPrefs.GetInt("Player" + (i + 1) + "_Score", 0);
            scoreDisplay += "Player " + (i + 1) + ": " + score + " points\n";
        }

        scoresText.text = scoreDisplay;
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
