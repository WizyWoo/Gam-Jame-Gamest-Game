using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject _pauseMenu;

    private void Update()
    {
            
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            if(_pauseMenu.activeInHierarchy)
            {

                Resume();

            }
            else
            {

                Pause();

            }

        }

    }

    public void Resume()
    {

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _pauseMenu.SetActive(false);

    }

    public void MainMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

    }

    public void Pause()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);

    }

}
