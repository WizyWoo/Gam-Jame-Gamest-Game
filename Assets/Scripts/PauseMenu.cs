using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    CameraController _cameraController;
    [SerializeField]
    private Slider _sensitivitySlider, _gammaSlider;
    [SerializeField]
    private PostProcessProfile _postProcessProfile, _potate, _pog;
    [SerializeField]
    private PostProcessVolume _postProcessVolume;

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

    private void Start()
    {

        if(PlayerPrefs.GetInt("Pog", 1) == 1)
        {

            PPFX(1);

        }
        else
        {

            PPFX(0);

        }

        _sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 3);
        _gammaSlider.value = PlayerPrefs.GetFloat("Gamma", 1.1f);
        _postProcessProfile.GetSetting<ColorGrading>().postExposure.value = _gammaSlider.value;
        PlayerPrefs.Save();

    }


    public void ChangingGamma()
    {

        _postProcessProfile.GetSetting<ColorGrading>().postExposure.value = _gammaSlider.value;

    }

    public void Resume()
    {

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _pauseMenu.SetActive(false);
        _cameraController.InputSensitivity = _sensitivitySlider.value;
        _postProcessProfile.GetSetting<ColorGrading>().postExposure.value = _gammaSlider.value;
        PlayerPrefs.SetFloat("MouseSensitivity", _sensitivitySlider.value);
        PlayerPrefs.SetFloat("Gamma", _gammaSlider.value);
        PlayerPrefs.Save();

    }

    public void PPFX(int pog)
    {

        if(pog == 1)
        {

            _postProcessProfile = _pog;
            PlayerPrefs.SetInt("Pog", 1);

        }
        else
        {

            _postProcessProfile = _potate;
            PlayerPrefs.SetInt("Pog", 0);

        }

        _postProcessVolume.profile = _postProcessProfile;

        PlayerPrefs.Save();

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
