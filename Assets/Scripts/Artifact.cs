using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    private AudioSource audioSource;
    private Portal[] portal;
    private TextHelp textHelp;
    private PortalManager portalManager;
    // Start is called before the first frame update
    void Start()
    {
        portalManager = PortalManager.instance;

        portal = FindObjectsOfType<Portal>();
        audioSource = GetComponent<AudioSource>();
        textHelp = FindObjectOfType<TextHelp>();

    }

    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            audioSource.Play();


            portalManager.numberOfArtifacts++;
            if (portalManager.numberOfArtifacts == 1)
            {
                textHelp.DisplayText("You Found the first artifact", 6);
            }
            if (portalManager.numberOfArtifacts == 2)
            {
                textHelp.DisplayText("You Found the second artifact", 6);
            }
            if (portalManager.numberOfArtifacts == 3)
            {
                textHelp.DisplayText("You Found the third artifact", 6);
            }


            Destroy(gameObject);
        }

    }
}
