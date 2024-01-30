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

    private PlayerHandsController playerhand;

    public float hoverHeight = 1f; // The height at which the agent hovers above the ground
    public float hoverSpeed = 1f; // The speed at which the agent moves up and down
    private float originalY; // The original y position of the agent
    public float minHeight = 0; // The minimum height of the hover

    private bool played;

    private AudioSource audioSource;





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
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        currentStates = states.SearchPlayer;
        playerhand = GameObject.FindWithTag("Player").GetComponent<PlayerHandsController>();
        originalY = minHeight;
    }

    // Update is called once per frame
    void Update()
    {
        Hover();

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

        
        if(!played)
        {
            audioSource.Play();
            played = true; 
        }
        

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

        played = false;
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


    // Method to make the ghost hoqer above the ground using the hoverheight and hover speed, and the original y position

    public void Hover()
    {
        transform.position = new Vector3(transform.position.x, originalY + ((float)Math.Sin(Time.time * hoverSpeed) * hoverHeight), transform.position.z);
        

    }

    // New method called ghost speed, depending on "PlayerHandsController.candle" transform, if it is null, then the speed of the agent should we twice its original speed, and when candle is not null, it should be normal
    public void GhostSpeed()
    {
        if (playerhand.Candle == null)
        {
            agent.speed = 10;
        }
        else
        {
            agent.speed = 5;
        }
    }

    // New method called ghost sight, depending on "PlayerHandsController.candle" transform, if it is null, then the sight of the agent should we twice its original sight, and when candle is not null, it should be normal
    public void GhostSight()
    {
        if (playerhand.Candle == null)
        {
            eyesight = 20;
        }
        else
        {
            eyesight = 10;
        }
    }

    // New method called ghost hearing, depending on "PlayerHandsController.candle" transform, if it is null, then the hearing of the agent should we twice its original hearing, and when candle is not null, it should be normal
    public void GhostHearing()
    {
        if (playerhand.Candle == null)
        {
            agent.angularSpeed = 100;
        }
        else
        {
            agent.angularSpeed = 50;
        }
    }

    // New method called ghost memory, depending on "PlayerHandsController.candle" transform, if it is null, then the memory of the agent should we twice its original memory, and when candle is not null, it should be normal
    public void GhostMemory()
    {
        if (playerhand.Candle == null)
        {
            recentlyVisitedPositions = recentlyVisitedPositions.Where(x => Time.time - x.TimeVisited < 10).ToList();
        }
        else
        {
            recentlyVisitedPositions = recentlyVisitedPositions.Where(x => Time.time - x.TimeVisited < 5).ToList();
        }
    }

    // New method called ghost smell, depending on "PlayerHandsController.candle" transform, if it is null, then the smell of the agent should we twice its original smell, and when candle is not null, it should be normal
    public void GhostSmell()
    {
        if (playerhand.Candle == null)
        {
            agent.acceleration = 20;
        }
        else
        {
            agent.acceleration = 10;
        }
    }

    // New method called ghost hearing, depending on "PlayerHandsController.candle" transform, if it is null, then the hearing of the agent should we twice its




}

