using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void QuitGame()
    {

        Application.Quit();

    }

    public void SetDifficulty(int difficulty)
    {

        DifficultyController.Instance.Difficulty = difficulty;
        SceneManager.LoadScene(1);

    }

}
