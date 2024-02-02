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

    public void PPFX(int pog)
    {

        if(pog == 1)
        {

            PlayerPrefs.SetInt("Pog", 1);

        }
        else
        {

            PlayerPrefs.SetInt("Pog", 0);

        }

        PlayerPrefs.Save();

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
