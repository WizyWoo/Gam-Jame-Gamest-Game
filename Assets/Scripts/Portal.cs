using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;


public class Portal : MonoBehaviour
{
   public Transform[] portalWorlds;
   public int numberOfPortalsHit, numberOfArtifacts;
    public ParticleSystem portalEffect;
    public GameObject[] artifact;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] artifacts = GameObject.FindGameObjectsWithTag("Artifact");
    }

    // Update is called once per frame
    void Update()
    {
         
            if(Vector3.Distance(player.transform.position, transform.position) < 2)
            {
                if(numberOfPortalsHit == 0)
                {
                    player.transform.position = portalWorlds[1].position;
                    numberOfPortalsHit++;
                }

                else if(numberOfPortalsHit == 1 && numberOfArtifacts == 1)
                {
                    player.transform.position = portalWorlds[0].position;
                    
                }
                else if(numberOfPortalsHit == 1)
                {
                    player.transform.position = portalWorlds[2].position;
                    numberOfPortalsHit++;
                    
                }
                else if(numberOfPortalsHit == 2 && numberOfArtifacts == 2)
                {
                    player.transform.position = portalWorlds[0].position;
                   
                }
                else if(numberOfPortalsHit == 2)
                {
                    player.transform.position = portalWorlds[3].position;
                    numberOfPortalsHit++;
                }
                else if(numberOfPortalsHit == 3 && numberOfArtifacts == 3)
                {
                    player.transform.position = portalWorlds[0].position;
                }
                else if(numberOfPortalsHit == 3)
                {
                    player.transform.position = portalWorlds[3].position;
                    numberOfPortalsHit++;
                }      
                else if(numberOfPortalsHit == 4 && numberOfArtifacts == 4)
                {
                    player.transform.position = portalWorlds[0].position;
                } 
            }
        
    }

    private void OnCollisionEnter(Collision other)
    {
           
    }
}
