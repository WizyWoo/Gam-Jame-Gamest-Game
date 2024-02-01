using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.UI;
using Unity.AI.Navigation;
public class GhostNavigation : MonoBehaviour
{
    private Material initialMaterial;
    public Material chaseMaterial;
    private MeshRenderer renderer;
    public bool PlayerIsDead;
    private NavMeshAgent agent;
    public Sprite jumpscare;
    private GameObject player;
    public float searchDistance; // The distance the ray is cast
    public float searchAngle = 75f; // The angle of the search

    private PlayerHandsController playerhand;

    public float hoverHeight = 1f; // The height at which the agent hovers above the ground
    public float hoverSpeed = 1f; // The speed at which the agent moves up and down
    private float originalY; // The original y position of the agent
    public float minHeight = 0; // The minimum height of the hover

    private bool played, SFXplayed;

    private AudioSource audioSource;
    public AudioSource jumpscareSFX;

    public float restartTimer;





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
        renderer = GetComponentInChildren<MeshRenderer>();
        NavMeshLink link = GetComponent<NavMeshLink>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        currentStates = states.SearchPlayer;
        playerhand = Camera.main.GetComponent<PlayerHandsController>();
        originalY = minHeight;
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerIsDead)
        {
            GameObject.Find("DeathCam").GetComponent<Image>().color = new Color(0, 0, 0, GameObject.Find("DeathCam").GetComponent<Image>().color.a + Time.deltaTime);

            //when the alpa is 100%, change imagie to game over
            if (GameObject.Find("DeathCam").GetComponent<Image>().color.a >= 1)
            {

                //Make the image shake violently
                GameObject.Find("DeathCam").GetComponent<RectTransform>().anchoredPosition = new Vector3(UnityEngine.Random.Range(-30, 30), UnityEngine.Random.Range(-30, 30), 0);

                GameObject.Find("DeathCam").GetComponent<Image>().sprite = jumpscare;
                GameObject.Find("DeathCam").GetComponent<Image>().color = new Color(1, 1, 1, 1);

                restartTimer += Time.deltaTime;
                Debug.Log(restartTimer);

                if (!SFXplayed)
                {
                    jumpscareSFX.Play();
                    SFXplayed = true;

                }


                if (restartTimer >= 5)
                {
                    //restart scene
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                }


            }

        }



        Hover();
        playerHasLight();

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
       // StartCoroutine(ChangeMaterialOverTime(chaseMaterial, 1f)); // Change material over 1 second





        // agent speed should accelerate when chasing the player
       // agent.acceleration = 20;


        if (!played)
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

        // if the agent is within 0.5 units from the player, a canvas screen should pop up, and the player should be dead, ignore y position
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z)) < 1)
        {
            PlayerIsDead = true;
            //Stop the agent from moving
            agent.SetDestination(transform.position);
            //destroy movement controller on pøayer
            player.GetComponent<PlayerMovementController>().enabled = false;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            player.GetComponent<Rigidbody>().velocity = agent.transform.forward * 10;
        }





    }



    public void KilledPlayer()
    {


    }

    public void SearchPlayer()
    {
        
       // StartCoroutine(ChangeMaterialOverTime(initialMaterial, 1f)); // Change material back over 1 second
        //slowly change to material0 from material1
       // agent.acceleration = 10;
        played = false;
        //move to a random spot on the ground
        if (agent.remainingDistance <= 1)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * searchDistance;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, searchDistance, 1);
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);

            //repeat previusly block of code if agent has traweled for longer than 5 meters



            if (Vector3.Distance(transform.position, player.transform.position) > 40)
            {
                Vector3 randomDirectionaroundP = UnityEngine.Random.insideUnitSphere * eyesight * 3;
                randomDirectionaroundP += player.transform.position;
                NavMeshHit hit2;
                NavMesh.SamplePosition(randomDirectionaroundP, out hit2, eyesight, 1);
                Vector3 finalPosition2 = hit2.position;
                agent.SetDestination(finalPosition2);
            }



        }

        //The agents "eyes" are a cone of 75 degrees, and 10 units long, they should search for the player, when the player is found, the agent should chase the player, debug draw the "eyes" of the agent
        RaycastHit hitInfo;
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (Physics.Raycast(transform.position, direction, out hitInfo, searchDistance))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
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

    // New method called ghost speed, depending on "PlayerHandsController.candle" transform, if it is null, then the speed of the agent should we twice its original speed, and when candle is not null, it should be norma


    IEnumerator ChangeMaterialOverTime(Material targetMaterial, float duration)
    {
        Material currentMaterial = renderer.material;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            Color color = Color.Lerp(currentMaterial.color, targetMaterial.color, t);
            renderer.material.color = color;
            yield return null;
        }

        renderer.material = targetMaterial;
    }


    public void playerHasLight()
    {

        if (playerhand.Candle != null && playerhand.Candle.GetComponent<CandleController>().IsLitFam == true)
        {
            searchDistance = 10;
            agent.speed = 2.5f;
        }
        else
        {
            searchDistance = 20;
            agent.speed = 4f;

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

