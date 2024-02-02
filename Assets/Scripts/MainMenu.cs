using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject EasyWin, NormalWin, HardWin;
    public Toggle VsyncToggleObj;

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

        int vSync = PlayerPrefs.GetInt("Vsync", 1);

        if(vSync == 0)
        {

            VsyncToggleObj.isOn = false;

        }

    }

    public void VsyncToggle()
    {

        if(VsyncToggleObj.isOn)
        {

            QualitySettings.vSyncCount = 1;
            PlayerPrefs.SetInt("Vsync", 1);

        }
        else
        {

            QualitySettings.vSyncCount = 0;
            PlayerPrefs.SetInt("Vsync", 0);

        }

        PlayerPrefs.Save();

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
