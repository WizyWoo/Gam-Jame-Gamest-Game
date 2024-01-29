using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class GhostNavigation : MonoBehaviour
{

    private NavMeshAgent agent;
    private GameObject player;
    public float searchDistance = 10f; // The distance the ray is cast
    public float searchAngle = 75f; // The angle of the search


    public float eyesight;

    public states currentStates;

    private struct VisitedPosition
    {
        public Vector3 Position { get; set; }
        public float TimeVisited { get; set; }

    }

    private List<VisitedPosition> recentlyVisitedPositions = new List<VisitedPosition>();

    public enum states
    {
        SearchPlayer,
        ChasingPlayer

    }



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        currentStates = states.SearchPlayer;
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentStates)
        {
            case states.SearchPlayer:
                SearchPlayer();
                break;
            case states.ChasingPlayer:
                ChasePlayer();
                break;
        }
    }



    public void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);

        //If the pllayer is outside of the agents searchdistance and searchangle, the agent should search for the player
        RaycastHit hitInfo;
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (Physics.Raycast(transform.position, direction, out hitInfo, searchDistance))
        {
            if (hitInfo.collider.gameObject.tag != "Player" || angle > searchAngle)
            {
                currentStates = states.SearchPlayer;
            }
        }
        Debug.DrawRay(transform.position, direction, Color.yellow);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(searchAngle, transform.up) * transform.forward * searchDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-searchAngle, transform.up) * transform.forward * searchDistance, Color.red);

        //Also make the outer half circle of the cone debugged
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(searchAngle / 2, transform.up) * transform.forward * searchDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-searchAngle / 2, transform.up) * transform.forward * searchDistance, Color.red);

        //Debug line on the ground, to see where the agent is going
        Debug.DrawLine(transform.position, agent.destination, Color.blue);

    



    }

    public void SearchPlayer()
    {


        //move to a random spot on the ground
        if (agent.remainingDistance < 0.5f)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * searchDistance;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, searchDistance, 1);
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);


        }

        //The agents "eyes" are a cone of 75 degrees, and 10 units long, they should search for the player, when the player is found, the agent should chase the player, debug draw the "eyes" of the agent
        RaycastHit hitInfo;
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (Physics.Raycast(transform.position, direction, out hitInfo, searchDistance))
        {
            if (hitInfo.collider.gameObject.tag == "Player" && angle < searchAngle)
            {
                currentStates = states.ChasingPlayer;
            }
        }
        Debug.DrawRay(transform.position, direction, Color.yellow);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(searchAngle, transform.up) * transform.forward * searchDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-searchAngle, transform.up) * transform.forward * searchDistance, Color.red);

        //Also make the outer half circle of the cone debugged
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(searchAngle / 2, transform.up) * transform.forward * searchDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-searchAngle / 2, transform.up) * transform.forward * searchDistance, Color.red);

        Debug.DrawLine(transform.position, agent.destination, Color.blue);


    }
}

