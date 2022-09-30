/*
    AUTHOR: Taylor Dobson
    FILENAME: Bot.cs
    SPECIFICATION: AI program for the agent
    FOR: CS 3368 Introduction to Artificial Intelligence Section 002
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/* 
    NAME: Bot
    PURPOSE: The Bot Class contains ...
    INVARIANTS: ...
*/
public class Toby : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    Vector3 wanderTarget = Vector3.zero;    //Baseline target position that is updated with a new value that the target position is going to move to each time the Wander() function is called.
    public float wanderDistance = 8;  //Distance circle is located offset from agent
    public float wanderRadius = 8;    //Radius of circle
    public float wanderJitter = 1;     //Variable that effects the amount of target point repositioning on the circle
    /* 
        NAME: Start
        PARAMETERS: None
        PURPOSE: The function is called before the first frame update.
        PRECONDITION: What should be true regarding the parameters and when the function can be called
        POSTCONDITION: What should be true after the function returns, such as variables changed or values returned
    */
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }


    /* 
        NAME: Seek
        PARAMETERS: 
        PURPOSE: The function sends the agent to a location on the nav mesh
        PRECONDITION: What should be true regarding the parameters and when the function can be called
        POSTCONDITION: What should be true after the function returns, such as variables changed or values returned
    */
    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }


    /* 
        NAME: Wander
        PARAMETERS: None
        PURPOSE: The function pushes the agent to follow a target location of a randomly generated point on a circle in front of the agent.
        PRECONDITION: What should be true regarding the parameters and when the function can be called
        POSTCONDITION: What should be true after the function returns, such as variables changed or values returned
    */
    void Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter); //Creates a new target that moves the existing wanderTarget
        //Random.Range(var, var) picks a random value between -1 and 1 to allow movement forward and backward, multiplied by the wanderJitter for a wider variance.
        //Vector3(x, y, z)
        wanderTarget.Normalize();   //Sets the wanderTarget vector length to 1 
        wanderTarget *= wanderRadius;   //Sets the wanderTarget vector position to the correct length to match the circle's radius

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance); //Repositions the circle's origin to be located in front of the agent by length of wanderDistance
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal); //Transforms the local target into a world position
        //InverseTransformVector(var) transitions the 'var' from local space to world space

        Seek(targetWorld);
    }


    /* 
        NAME: CanSeePlayer
        PARAMETERS: None
        PURPOSE: This function allows the agent see the target from where it is based on other game objects in the world
        PRECONDITION: What should be true regarding the parameters and when the function can be called
        POSTCONDITION: What should be true after the function returns, such as variables changed or values returned
    */
    bool CanSeePlayer()
    {
        RaycastHit raycastInfo;
        Vector3 rayToPlayer = target.transform.position - this.transform.position; //calculate a ray to the target from the agent
        float lookAngle = Vector3.Angle(this.transform.forward, rayToPlayer); //determines if the agent is facing towards the target

        if (Vector3.Distance(this.transform.position, target.transform.position) < 5 && 
            lookAngle < 60 && 
            Physics.Raycast(this.transform.position, rayToPlayer, out raycastInfo)) //perform a raycast to determine if there's anything between the agent and the target
        {
            if (raycastInfo.transform.gameObject.tag == "Player") //ray will hit the target if no other colliders in the way
                return true;
        }
        return false;
    }


    /* 
        NAME: Update
        PARAMETERS: None
        PURPOSE: The function is called once per frame.
        PRECONDITION: What should be true regarding the parameters and when the function can be called
        POSTCONDITION: What should be true after the function returns, such as variables changed or values returned
    */
    void Update()
    {
        //if the target is considered out of range and can't be seen
        //if (!CanSeePlayer())
            Wander();
        //else
            //Seek(target.transform.position);
    }
}
