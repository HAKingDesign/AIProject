/*

    AUTHOR: Dhruv Maniar
    FILENAME: Health.cs
    SPECIFICATION: To manage and display health for the player and operate respawn function
    FOR: CS3368 Introducation to Artifical Intelligence Section 001

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random; // Specifing the libray from where Random fuction is to be used in this script

/*

    NAME: Health
    PURPOSE: To manage and display health for the player on a health bar
    and operate respawn function with respawn points as Vector3 in an array
    INVARIANTS: When a player is close enough to SpAIder it should take damage and display less health
                and the SpAIder should respawn at a new location

*/
public class Health : MonoBehaviour
{
    public int maxHealth = 100; // Maximum Health
    public int currentHealth; // Current Health
    public Slider HealthBar; // The Health bar
    public GameObject Menus; // To display menu screen when health is 0
    public GameObject SpAIder_prefab; // Spaider prefab as game object to respawn
    private Vector3[] Spawnpoints = new Vector3[7]; //Creating a array of Vector3 points of size 7
    Pausememu PauseScript; // Pause menu Script
    
    void Start()// Start is called before the first frame update
    {
        // Adding Waypoints as Vector3 points into an Spawnpoints array so that the respawned SpAIder has access to all Spawnpoints when the script resets (without the need to manualy add it in inspector view).
        Spawnpoints[0] = new Vector3(4.69f, 0.27f, 0.014f);
        Spawnpoints[1] = new Vector3(-7.91f, 0.82f, 10.0f);
        Spawnpoints[2] = new Vector3(-4.4f, 0.91f, 26.63f);
        Spawnpoints[3] = new Vector3(-0.61f, 0.855f, 50f);
        Spawnpoints[4] = new Vector3(16.11f, 0.96f, 43.8f);
        Spawnpoints[5] = new Vector3(11.13f, 1.168f, 18.0f);
        Spawnpoints[5] = new Vector3(-5.54f, 5.92f, 15.04f);
        PauseScript = Menus.GetComponent<Pausememu>(); //obtain Script connected to pause menu
        currentHealth = maxHealth; //Setting the health to maximum (100) when game starts.
    }
    /*

        NAME: TakeDamage
        PARAMETER: amount
        PURPOSE: Simple function to subtract health and set it has current health
        PRECONDTION: Game must have started
        POSTCONDTION: Changed health anytime SpAIder deals Damage

    */
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }
    /*

        NAME: SetHealth
        PARAMETER: health
        PURPOSE: Simple function to set health bar value to currenthealth
        PRECONDTION: Game must have started
        POSTCONDTION: Changed health bar UI anytime SpAIder deals Damage

    */
    public void SetHealth(int health)
    {
        HealthBar.value = health;
    }
    /*

        NAME: SpawnSpAIder
        PARAMETER: none
        PURPOSE: To Instantiate SpAIder_prefab as a gameobject at an random spawn location 
        PRECONDTION: Game must have started
        POSTCONDTION: The SpAIder appears at a new location.

    */
    void SpawnSpAIder()
    {
        GameObject spawn = Instantiate(SpAIder_prefab) as GameObject;
        int r = Random.Range(0,7);
        spawn.transform.position = Spawnpoints[r];
    }

    /*

        NAME: OnTriggerEnter
        PARAMTERS: Collider ohter
        PURPOSE: Check if SpAIder object causes a trigger,
                    print Spaider dealt damage
                    Use fucntion TakeDamage to deal damage randomly selected between 5 to 25
                    Spawn the SpAIder at a new location
                    Destroy current SpAIder
                    Destroy the game object SpAIder to respawn on new spawn point
        PRECONDTION: Player within a radius of 2 units
        POSTCONDTION: Take damage and remove the SpAIder game object.

    */
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SpAIder")
        {   // if Toby tagged objects hit player
            Debug.Log("SpAIder Dealt Damage");
            int d = Random.Range(5,25);
            TakeDamage(d);
            SetHealth(currentHealth);
            SpawnSpAIder();
            Destroy(GameObject.FindGameObjectWithTag("SpAIder"));
            
        }
    }
    /* 
        NAME: Update
        PARAMETERS: None
        PURPOSE: This function is consistently called for the player to check its health.
        PRECONDITION: Called once per frame.
        POSTCONDITION: If health is 0
                            Stop game and show pause menu screen.
    */
    void Update()
    {
        if(currentHealth<=0)
        {
            PauseScript.Pause(PauseScript.GameOverUI);
        }  
    }   
}