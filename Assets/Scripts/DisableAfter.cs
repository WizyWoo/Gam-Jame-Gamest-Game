using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfter : MonoBehaviour
{

    private void Start()
    {

        Invoke(nameof(DisableMe), 1f);

    }

    public void DisableMe()
    {
            
        gameObject.SetActive(false);

    }

}
