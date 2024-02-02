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
            SoundManager.instance.PlaySound(3);
            Instantiate(GameObject.Find("Spooky Ghost(Clone)"));


            portalManager.numberOfArtifacts++;
            if (portalManager.numberOfArtifacts == 1)
            {
                textHelp.DisplayText("1 / 4 Portals Destroyed", 4);
            }
            if (portalManager.numberOfArtifacts == 2)
            {
                textHelp.DisplayText("2 / 4 Portals Destroyed", 4);
            }
            if (portalManager.numberOfArtifacts == 3)
            {
                textHelp.DisplayText("3 / 4 Portals Destroyed", 4);
            }
            if (portalManager.numberOfArtifacts == 4)
            {
                textHelp.DisplayText("All Portals Destroyed... Thank you, for your saccrifice", 4);
            }


            Destroy(gameObject);
        }

    }
}
