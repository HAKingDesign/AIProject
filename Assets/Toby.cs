/*
    AUTHOR: Taylor Dobson
    FILENAME: Toby.cs
    SPECIFICATION: AI program for the agent
    FOR: CS 3368 Introduction to Artificial Intelligence Section 002
*/

//Standard modules
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   //This AI module implements the navmesh features in Unity.


/* 
    NAME: Toby
    PURPOSE: The Toby Class contains the instructions for the AI agent on how to interact with it's environment and the player.
    INVARIANTS: Cannot go outside the bounds of the NavMesh
*/
public class Toby : MonoBehaviour   //MonoBehaviour is the base class from which every Unity script derives.
{
    NavMeshAgent agent; //This component is attached to a mobile character in the game to allow it to navigate the Scene using the NavMesh.
    public GameObject player; //Declares the player as a game object to be interacted with.
    public GameObject[] wps; //Declares an array of game objects used for the waypoint system.
    public GameObject paperObject; //Declares the papers as a game object to be interacted with.
    private ObjectCollection paperTracker;   //Declares an ObjectCollection variable to be used within the Toby class.
    int currentWP = 0;  //This variable represents which waypoint the AI is targeting.
    Vector3 wanderTarget = Vector3.zero; //Baseline target position that is updated with a new value for the target position to seek each time the Wander() function is called.



    /* 
        NAME: Start
        PARAMETERS: None
        PURPOSE: The function initializes the variable agent as a NavMeshAgent.
        PRECONDITION: Called on the frame when a script is enabled just before the Update method is called the first time.
        POSTCONDITION: N/A
    */
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>(); //Returns the component of type<> if the GameObject has one attached.
        paperTracker = paperObject.GetComponent<ObjectCollection>();    //Instantiates the ObjectCollection type variable as a game object.
    }



    /* 
        NAME: Seek
        PARAMETERS: Vector3 variable
        PURPOSE: The function directs the agent to a location.
        PRECONDITION: The parameter should be a Vector3 variable and called when the agent is pursuing a location.
        POSTCONDITION: Give agent the new location to go to.
    */
    void Seek(Vector3 location)
    {
        agent.SetDestination(location); //Sets or updates the destination thus triggering the calculation for a new path.
    }



    /* 
        NAME: Patrol
        PARAMETERS: None
        PURPOSE: This function makes the AI target a waypoint to approach.
        PRECONDITION: This function can be called within the Update function.
        POSTCONDITION: This function calls the Seek function to head towards the next waypoint in the wps array indicated by the currentWP index.
    */
    void Patrol()
    {
        Seek(wps[currentWP].transform.position);
    }



    /* 
        NAME: Wander
        PARAMETERS: None
        PURPOSE: The function directs the agent to follow a target location of a randomly generated point on a circle's circumference generated in front of the agent for a limited amount of time.
        PRECONDITION: This function is called within the Update function as a Coroutine to allow pausing of the function.
        POSTCONDITION: When this function completes it gives the agent an updated location to pursue on the generated wander circle.
    */
    IEnumerator Wander()    //Function type 'IEnumerator' required as a parameter to utilize the StartCoroutine function.
    {
        agent.speed = 2;
        float timePassed = 0;   //This variable indicates time passed in seconds.
        currentWP++;    //Increments the waypoint index.
        if (currentWP >= wps.Length)   //Resets the currentWP index to restart at the first waypoint when the index limit is reached.
            currentWP = 0;
        while (timePassed < 5)  //Represents the number of seconds this function will run.
        {
            float wanderDistance = 10;  //Distance circle is located offset from agent.
            float wanderRadius = 10;    //Radius of circle.
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

            yield return null;  //Yield execution of this coroutine and return to the main loop until next frame.
        }
    }



    /* 
        NAME: CanSeePlayer
        PARAMETERS: None
        PURPOSE: This function allows the agent see the player from where it is based on other game objects in the world and the environment.
        PRECONDITION: This function can be called within the Update function.
        POSTCONDITION: This function returns true if the agent can see the player within a certain distance, within a certain angle, and is an unobstructed view, otherwise returns false.
    */
    bool CanSeePlayer()
    {
        RaycastHit raycastInfo; //Variable of a structure used to get information back from a raycast.
        Vector3 rayToPlayer = player.transform.position - this.transform.position; //Calculate a ray to the player from the agent.
        float lookAngle = Vector3.Angle(this.transform.forward, rayToPlayer); //Determines if the agent is facing towards the player.

        if (Vector3.Distance(this.transform.position, player.transform.position) < 5 &&              //Checks whether the player is within 5 units of the agent.
                             lookAngle < 60 &&                                                       //Checks to see if the agent is looking at the player within a 60 degree angle.
                             Physics.Raycast(this.transform.position, rayToPlayer, out raycastInfo)) //Performs a raycast to determine if there's anything between the agent and the player.
        {
            if (raycastInfo.transform.gameObject.tag == "Player") //If the ray hits the player when no other colliders are in the way, the agent can see the player.
                return true;
        }
        return false;
    }



    /* 
        NAME: Update
        PARAMETERS: None
        PURPOSE: This function is consistently called for the agent to base it's decisions on.
        PRECONDITION: Called once per frame.
        POSTCONDITION: Updates the agent's decisions.
    */
    void Update()
    {
        if (CanSeePlayer())     //Pursue the player if the player is visible based on the conditions of the CanSeePlayer function.
            Seek(player.transform.position);
        if (!CanSeePlayer())    //If the player is not visible based on the conditions of the CanSeePlayer function, continue to call the Patrol function.
            Patrol();
        if (Vector3.Distance(agent.transform.position, wps[currentWP].transform.position) < 3 && !CanSeePlayer())   //Once reaching a waypoint within 3 units and if the player is not visible based on the conditions of the CanSeePlayer function, call the Wander function as a coroutine.
            StartCoroutine(Wander());   //A coroutine is a method that can pause execution and return control to Unity but then continue where it left off on the following frame.
        agent.speed = (float)((paperTracker.Paper * 0.5) + 3);  //Increases the AI's speed based on the number of papers collected.
    }
}
