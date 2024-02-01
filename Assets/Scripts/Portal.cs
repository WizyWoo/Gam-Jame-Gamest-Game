using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Lobbies.Models;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public Transform[] portalWorldsPos;
    private PortalManager portalManager;
    public ParticleSystem portalEffect;
    public GameObject[] artifact;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        portalManager = PortalManager.instance;
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] artifacts = GameObject.FindGameObjectsWithTag("Artifact");
        portalWorldsPos[0] = GameObject.FindGameObjectWithTag("StarterPoss").transform;
        portalWorldsPos[1] = GameObject.Find("ArtifactRoomSpawnPos1").transform;
        portalWorldsPos[2] = GameObject.Find("ArtifactRoomSpawnPos2").transform;

    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(player.transform.position, transform.position) < 1)
        {
            if (portalManager.numberOfPortalsHit == 0 && portalManager.numberOfArtifacts == 0)
            {
                player.transform.position = portalWorldsPos[1].position;

            }

            else if (portalManager.numberOfPortalsHit == 0 && portalManager.numberOfArtifacts == 1)
            {
                player.transform.position = portalWorldsPos[0].position;
                portalManager.numberOfPortalsHit++;

            }
            else if (portalManager.numberOfPortalsHit == 1 && portalManager.numberOfArtifacts == 1)
            {
                player.transform.position = portalWorldsPos[2].position;

            }
            else if (portalManager.numberOfPortalsHit == 1 && portalManager.numberOfArtifacts == 2)
            {
                player.transform.position = portalWorldsPos[0].position;
                portalManager.numberOfPortalsHit++;

            }
            else if (portalManager.numberOfPortalsHit == 2 && portalManager.numberOfArtifacts == 2)
            {
                player.transform.position = portalWorldsPos[3].position;
            }
            else if (portalManager.numberOfPortalsHit == 2 && portalManager.numberOfArtifacts == 3)
            {
                player.transform.position = portalWorldsPos[0].position;
                portalManager.numberOfPortalsHit++;
            }
            else if (portalManager.numberOfPortalsHit == 3 && portalManager.numberOfArtifacts == 3)
            {
                player.transform.position = portalWorldsPos[3].position;
            }
            else if (portalManager.numberOfPortalsHit == 3 && portalManager.numberOfArtifacts == 4)
            {
                player.transform.position = portalWorldsPos[0].position;
            }
            Destroy(gameObject);
        }

    }


}
