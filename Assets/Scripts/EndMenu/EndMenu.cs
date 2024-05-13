using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    public TMP_Text resultText;

    void Start()
    {
        int gameResult = PlayerPrefs.GetInt("GameResult", -1); // Default value -1 indicates no result

        if (gameResult == 1)
        {
            resultText.text = "You Won";
        }
        else if (gameResult == 0)
        {
            resultText.text = "You Lost";
        }
        else
        {
            resultText.text = "Game Result Unknown";
        }
    }

    public static void PlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public static void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public static void QuitButton()
    {
        Application.Quit();
    }
}
