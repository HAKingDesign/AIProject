/*
    AUTHOR: Dhruv Maniar
    FILENAME: SpAIder.cs
    SPECIFICATION: AI program for Toby's pet spider named SpAIder
    FOR: CS 3368 Introduction to Artificial Intelligence Section 002
*/

//Standard modules
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //This AI module implements the navmesh features in Unity.


/* 
    NAME: SpAIder
    PURPOSE: The SpAIder Class contains the instructions for the AI agent on how to interact with it's environment and the player.
    INVARIANTS: Cannot go outside the bounds of the NavMesh
*/
public class SpAIder : MonoBehaviour   //MonoBehaviour is the base class from which every Unity script derives.
{
    NavMeshAgent agent; //This component is attached to a mobile character in the game to allow it to navigate the Scene using the NavMesh.
    public GameObject player; //Declares the player as a game object to be interacted with.
    int currentSP = 0;  //This variable represents which spawn point the AI is targeting.
    public Vector3 wanderTarget = Vector3.zero; //Baseline target position that is updated with a new value for the target position to seek each time the Wander() function is called.
    public Vector3 targetPosition = Vector3.zero;  //Variable to save the targetPosition of player or spawn point.
    private Vector3[] Spawnpoints = new Vector3[7];
    Animator anim;


    /* 
        NAME: Start
        PARAMETERS: None
        PURPOSE: The function initializes the variable agent as a NavMeshAgent.
        PRECONDITION: Called on the frame when a script is enabled just before the Update method is called the first time.
        POSTCONDITION: N/A
    */
    void Start()
    {
        // Adding Waypoints as Vector3 points into an Spawnpoints array so that the respawned SpAIder has access to all Spawnpoints when the script resets (without the need to manualy add it in inspector view).
        Spawnpoints[0] = new Vector3(4.69f, 0.27f, 0.014f);
        Spawnpoints[1] = new Vector3(-7.91f, 0.82f, 10.0f);
        Spawnpoints[2] = new Vector3(-4.4f, 0.91f, 26.63f);
        Spawnpoints[3] = new Vector3(-0.61f, 0.855f, 50f);
        Spawnpoints[4] = new Vector3(16.11f, 0.96f, 43.8f);
        Spawnpoints[5] = new Vector3(11.13f, 1.168f, 18.0f);
        Spawnpoints[5] = new Vector3(-5.54f, 5.92f, 15.04f);
        anim = GetComponent<Animator>();// Gets the animator component to access bool data types from the animator control to control the animation states as a Finite State Machine.
        player = GameObject.FindGameObjectWithTag("Player"); // Declairing Gameobject player as the FirstPersonController manualy so that the respawned SpAIder has access when the script resets.
        agent = this.GetComponent<NavMeshAgent>(); //Returns the component of type<> if the GameObject has one attached.
        targetPosition = Spawnpoints[currentSP];//Instantiates the targetPostion with the first spawn point position.
    }



    /* 
        NAME: Seek
        PARAMETERS: Vector3 variable
        PURPOSE: The function directs the SpAIder to a location.
        PRECONDITION: The parameter should be a Vector3 variable and called when the SpAIder is pursuing a location.
        POSTCONDITION: Give SpAIder the new location to go to.
    */
    void Seek(Vector3 location)
    {
        agent.SetDestination(location); //Sets or updates the destination thus triggering the calculation for a new path.
    }



    /* 
        NAME: Wander
        PARAMETERS: None
        PURPOSE: The function directs the SpAIder to follow a target location of a randomly generated point on a circle's circumference generated in front of the SpAIder for a limited amount of time.
        PRECONDITION: This function is called within the Update function as a Coroutine to allow pausing of the function.
        POSTCONDITION: When this function completes it gives the SpAIder an updated location to pursue on the generated wander circle.
    */
    IEnumerator Wander()    //Function type 'IEnumerator' required as a parameter to utilize the StartCoroutine function.
    {
        float timePassed = 0;   //This variable indicates time passed in seconds.
        currentSP++;    //Increments the waypoint index.
        if (currentSP >= 7)   //Resets the currentSP index to restart at the first waypoint when the index limit is reached.
            currentSP = 0;
        while (timePassed < 10)  //Represents the number of seconds this function will run.
        {
            agent.speed = 2.5f; //Sets the AI's speed to be slower when wandering.
            float wanderDistance = 1;  //Distance circle is located offset from agent.
            float wanderRadius = 2;    //Radius of circle.
            float wanderJitter = 1;    //Variable that effects the amount of target point repositioning on the circle.
            timePassed += Time.deltaTime;   //Adds real time seconds to the variable.

            wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter); //Updates the wanderTarget with a new vector utilizing Random.Range(var, var) for the x and z axis that picks a random value between -1 and 1 to allow movement forward and backward, multiplied by the wanderJitter for a wider variance.

            wanderTarget.Normalize();   //Sets the wanderTarget vector magnitude to 1 .
            wanderTarget *= wanderRadius;   //Sets the wanderTarget vector position to the correct length to match the circle's radius.

            Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance); //Repositions the circle's origin to be located in front of the agent by length of wanderDistance.
            Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal); //Transitions the 'var' from local space into world space.

            Seek(targetWorld); //Instructs the agent to pursue the location created by targetWorld.

            if (CanSeePlayer()){    //If the SpAIder can see the player while Wander is running, will cut out of the Wander function to allow pursuit.
                break;
            }
            yield return null;  //Yield execution of this coroutine and return to the main loop until next frame.
        }
    }



    /* 
        NAME: CanSeePlayer
        PARAMETERS: None
        PURPOSE: This function allows the SpAIder to see the player from where it is based on other game objects in the world and the environment.
        PRECONDITION: This function can be called within the Update function.
        POSTCONDITION: This function returns true if the SpAIder can see the player within a certain distance, within a certain angle, and is an unobstructed view, otherwise returns false.
    */
    bool CanSeePlayer()
    {
        RaycastHit raycastInfo; //Variable of a structure used to get information back from a raycast.
        Vector3 rayToPlayer = player.transform.position - this.transform.position; //Calculate a ray to the player from the agent.
        float lookAngle = Vector3.Angle(this.transform.forward, rayToPlayer); //Determines if the agent is facing towards the player.

        if (Vector3.Distance(this.transform.position, player.transform.position) < 9 &&              //Checks whether the player is within 5 units of the agent.
                             lookAngle < 90 &&                                                       //Checks to see if the agent is looking at the player within a 60 degree angle.
                             Physics.Raycast(this.transform.position, rayToPlayer, out raycastInfo)) //Performs a raycast to determine if there's anything between the agent and the player.
        {
            if (raycastInfo.transform.gameObject.tag == "Player") //If the ray hits the player when no other colliders are in the way, the agent can see the player.
                anim.SetBool("Attack", true);
                return true;
        }
        anim.SetBool("Attack", false);
        return false;
    }



    /* 
        NAME: Update
        PARAMETERS: None
        PURPOSE: This function is consistently called for the SpAIder to base it's decisions on.
        PRECONDITION: Called once per frame.
        POSTCONDITION: Updates the SpAIder's decisions.
    */
    void Update()
    {
        if (CanSeePlayer()){     //Set the player position as targetPosition if the player is visible based on the conditions of the CanSeePlayer function.
            targetPosition = player.transform.position;
        }
        if (Vector3.Distance(agent.transform.position, targetPosition) >= 3  || CanSeePlayer()) //If SpAIder has not reached the current targetPositon or can see the player, seeks to targetPositon.
        { 
            Seek(targetPosition);
        } else if (Vector3.Distance(agent.transform.position, targetPosition) < 3 && !CanSeePlayer()){   //Once reaching a waypoint within 3 units and if the player is not visible based on the conditions of the CanSeePlayer function, call the Wander function as a coroutine.
            StartCoroutine(Wander()); //A coroutine is a method that can pause execution and return control to Unity but then continue where it left off on the following frame.
            anim.SetBool("Attack", false);//Set bool for Attack animation false to turn of attack animation
            anim.SetBool("Damage", false);//Set bool for Damage animation false to turn of damage animation
            targetPosition = Spawnpoints[currentSP];//Set target position to next waypoint position.
        }
        if(Input.GetKeyDown(KeyCode.E)) // Deal damage to SpAIder by pressing E
            {
                anim.SetBool("Attack", false);   //Set bool for Attack animation false to turn of attack animation
                anim.SetBool("Damage", true);    //Set bool for Damage animation true to turn on damage animation                        
            }
          
    }
    /*

        NAME: OnTriggerEnter
        PARAMTERS: Collider ohter
        PURPOSE: Check if SpAIder object causes a trigger,
                    Set bool for Attack animation true to turn on attack animation
                    Destroy the game object SpAIder to respawn on new spawn point
        PRECONDTION: Player within a radius of 2 units
        POSTCONDTION: Play attack animation and remove the SpAIder game object.

    */
    
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "SpAIder")
        {   
            anim.SetBool("Attack", true);
            Destroy(gameObject);

        }
    }
    
}
