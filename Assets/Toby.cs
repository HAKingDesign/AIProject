/*
    AUTHOR: Taylor Dobson
    FILENAME: Toby.cs
    SPECIFICATION: AI program for the agent
    FOR: CS 3368 Introduction to Artificial Intelligence Section 002
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* 
    NAME: Toby
    PURPOSE: The Toby Class contains the instructions for the AI agent on how to interact with it's environment and the player.
    INVARIANTS: ...
*/
public class Toby : MonoBehaviour
{
    NavMeshAgent agent; //This component is attached to a mobile character in the game to allow it to navigate the Scene using the NavMesh.
    public GameObject player; //Declares the player as a game object to be interacted with.
    Vector3 wanderTarget = Vector3.zero; //Baseline target position that is updated with a new value for the target position to seek each time the Wander() function is called.

    /* 
        NAME: Start
        PARAMETERS: None
        PURPOSE: The function is called before the first frame update.
        PRECONDITION: Called before the first frame update.
        POSTCONDITION: 
    */
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>(); //Returns the component of type if the GameObject has one attached.
    }


    /* 
        NAME: Seek
        PARAMETERS: Vector3 variable
        PURPOSE: The function sends the agent to a location on the nav mesh.
        PRECONDITION: What should be true regarding the parameters and when the function can be called.
        POSTCONDITION: What should be true after the function returns, such as variables changed or values returned.
    */
    void Seek(Vector3 location)
    {
        agent.SetDestination(location); //Sets or updates the destination thus triggering the calculation for a new path.
    }


    /* 
        NAME: Wander
        PARAMETERS: None
        PURPOSE: The function pushes the agent to follow a player location of a randomly generated point on a circle in front of the agent.
        PRECONDITION: What should be true regarding the parameters and when the function can be called
        POSTCONDITION: What should be true after the function returns, such as variables changed or values returned
    */
    void Wander()
    {
        float wanderDistance = 8;  //Distance circle is located offset from agent.
        float wanderRadius = 8;    //Radius of circle.
        float wanderJitter = 1.0f;    //Variable that effects the amount of player point repositioning on the circle.

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                    0,
                                    Random.Range(-1.0f, 1.0f) * wanderJitter); //Creates a new player that moves the existing wanderTarget.
        //Random.Range(var, var) picks a random value between -1 and 1 to allow movement forward and backward, multiplied by the wanderJitter for a wider variance.
        //Vector3(x, y, z)
        wanderTarget.Normalize();   //Sets the wanderTarget vector magnitude to 1 .
        wanderTarget *= wanderRadius;   //Sets the wanderTarget vector position to the correct length to match the circle's radius.

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance); //Repositions the circle's origin to be located in front of the agent by length of wanderDistance.
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal); //Transitions the 'var' from local space into world space.

        Seek(targetWorld); //Instructs the agent to pursue the location created by targetWorld.
    }


    /* 
        NAME: CanSeePlayer
        PARAMETERS: None
        PURPOSE: This function allows the agent see the player from where it is based on other game objects in the world and the environment.
        PRECONDITION: What should be true regarding the parameters and when the function can be called
        POSTCONDITION: What should be true after the function returns, such as variables changed or values returned
    */
    bool CanSeePlayer()
    {
        RaycastHit raycastInfo; //Variable of a structure used to get information back from a raycast.
        Vector3 rayToPlayer = player.transform.position - this.transform.position; //Calculate a ray to the player from the agent.
        float lookAngle = Vector3.Angle(this.transform.forward, rayToPlayer); //Determines if the agent is facing towards the player.

        if (Vector3.Distance(this.transform.position, player.transform.position) < 10 &&              //Checks whether the player is within 5 units of the agent.
                             lookAngle < 80 &&                                                       //Checks to see if the agent is looking at the player within a 60 degree angle.
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
        POSTCONDITION: 
    */
    void Update()
    {
        //If the player can't be seen, continue wandering.
        if (!CanSeePlayer())
            Wander();
        else
            Seek(player.transform.position);
    }
}
