using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject EasyWin, NormalWin, HardWin;

    private void Start()
    {

        if(PlayerPrefs.HasKey("EasyWin"))
        {

            EasyWin.SetActive(true);

        }
        if(PlayerPrefs.HasKey("NormalWin"))
        {

            NormalWin.SetActive(true);

        }
        if(PlayerPrefs.HasKey("HardWin"))
        {

            HardWin.SetActive(true);

        }

    }

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
