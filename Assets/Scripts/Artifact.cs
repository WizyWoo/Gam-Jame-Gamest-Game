using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{   private AudioSource audioSource;
    private Portal[] portal;
    // Start is called before the first frame update
    void Start()
    {
        portal = FindObjectsOfType<Portal>();
        audioSource = GetComponent<AudioSource>();
        
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
            }

            Destroy(gameObject);
        }
        
    }
}
