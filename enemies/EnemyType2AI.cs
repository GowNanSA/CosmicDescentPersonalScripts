// Type 2 AI, the enemy chases player BUT stops and waits if the player is looking at the enemy. 
using UnityEditor.XR;
using System.Collections; 
using UnityEngine;
using UnityEngine.AI; // for built in stuff 

/// REFERENCE USED https://youtu.be/UjkSFoLxesw?feature=shared
/// // https://youtu.be/_e57zSZSOS8?feature=shared

// enemy type 2 - weeping angel thing


// HOW to use for game implementation: 
// Add navmesh agent to enemy and assign it as the navmesh agent 


public class EnemyType2AI : MonoBehaviour
{
    public Transform playerTarget;
    public NavMeshAgent agent;

    public LayerMask ground1, player1; // assign layer to maze floor and the player in

    // to allow the enemy to patrol the player 
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // to detect player is looking
    //bool isSeen = false;
    private RaycastHit hit;

    // state machine reference
    public float sightRange = 10f;


    // weeping angel stuff 
    public Transform playerCamera;
    bool playerSeen; // bools to check if player is in range or in sight
    bool playerLooking = false; // check if player is looking 

    Vector3 spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // search for the player 
        playerTarget = GameObject.Find("Player").transform;
        spawnPoint = playerTarget.position;
        //enemySpawnPoint = transform.position;
        playerCamera = Camera.main.transform;
        // assign navmesh agent 
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is in sight or range 
        playerSeen = Physics.CheckSphere(transform.position, sightRange, player1); // player is seen by enemy or not 
        playerLooking = playerIsLooking();
        if (!playerSeen) Patrol(); // player not found
        else if (playerSeen && !playerLooking) // player not looking
        {
            // chase 
            Chase();
        }
        else { // player sees enemy 
            StopOnSight(); 
        }
    }


    // player is looking at enemy or not raycast 
    bool playerIsLooking() {
        Vector3 direction = transform.position - playerCamera.position; 
        float angle = Vector3.Angle(playerCamera.forward, direction);

        if (angle < 50f) { 
            Ray ray = new Ray(playerCamera.position, direction);
            if (Physics.Raycast(ray, out hit, sightRange))
            {
                if (hit.transform == transform) {
                    return true; // player look at enemy 
                }
            }
        }

        return false; // not seen default 
    }



    // enemy roams around patrol 
    void Patrol()
    {
        //agent.updateRotation = true;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    // walking 
    void SearchWalkPoint()
    {
        float randomRangeZ = Random.Range(-walkPointRange, walkPointRange);
        float randomRangeX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomRangeX, transform.position.y, transform.position.z + randomRangeZ);

        // this is a raycast to check if this is on the ground from the tutorial I watched
        if (Physics.Raycast(walkPoint + Vector3.up, -Vector3.up, 3f, ground1))
            walkPointSet = true;
    }

    // chase and attack 
    void Chase()
    {
        agent.updateRotation = true;
        agent.SetDestination(playerTarget.position);
        // we can include the attack and other AI elements in here later on 
        transform.LookAt(playerTarget);
    }


    // mob touches player and sends back to start point 
    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            //teleport player
            Transform playerTransform = collision.gameObject.transform;
            collision.gameObject.transform.position = spawnPoint;

           

        }
    }

    // stop
    void StopOnSight() {
        agent.ResetPath();
        agent.updateRotation = false;

    }

}
// will likely kill the player, lead to game over, or restart level when touched 
