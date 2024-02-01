using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{   private AudioSource audioSource;
    private Portal[] portal;
    private TextHelp textHelp;
    // Start is called before the first frame update
    void Start()
    {
        portal = FindObjectsOfType<Portal>();
        audioSource = GetComponent<AudioSource>();
        textHelp = FindObjectOfType<TextHelp>();
        
    }

    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            audioSource.Play();
            foreach(Portal p in portal)
            {
                p.numberOfArtifacts++;
                if(p.numberOfArtifacts == 1)
                {
                   textHelp.DisplayText("You Found the first artifact", 6);
                }
                if(p.numberOfArtifacts == 2)
                {
                    textHelp.DisplayText("You Found the second artifact", 6);
                }
                if(p.numberOfArtifacts == 3)
                {
                    textHelp.DisplayText("You Found the third artifact", 6);
                }
            }

            Destroy(gameObject);
        }
        
    }
}
