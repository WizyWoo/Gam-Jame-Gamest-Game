using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultRewards : MonoBehaviour
{
    public GameObject easyreward, mediumreward, hardreward;
    

    // Start is called before the first frame update
    void Start()
    {
        if(DifficultyController.Instance.Difficulty == 0)
        {
            PlayerPrefs.SetInt("EasyWin", 1);
            easyreward.SetActive(true);
            mediumreward.SetActive(false);
            hardreward.SetActive(false);
        }
        else if(DifficultyController.Instance.Difficulty == 1)
        {
            PlayerPrefs.SetInt("NormalWin", 1);
            easyreward.SetActive(false);
            mediumreward.SetActive(true);
            hardreward.SetActive(false);
        }
        else if(DifficultyController.Instance.Difficulty == 2)
        {
            PlayerPrefs.SetInt("HardWin", 1);
            easyreward.SetActive(false);
            mediumreward.SetActive(false);
            hardreward.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
