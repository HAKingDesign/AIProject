/*

    AUTHOR: Sebastian Baglo
    FILENAME: ObjectCollection.cs
    SPECIFICATION: Should manage and display collection logic for the player
    FOR: CS3368 Introducation to Artifical Intelligence Section 001

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    NAME: ObjectCollection
    PURPOSE: Display and manage the logic regarding the Papers.
    Can be used by other classes to give a dynamic game based on how
    far you have progressed in the game.
    INVARIANTS: When a player is close enough to a paper, is should be collected
    and deleted from the scene. Can only pick up 7 papers.

*/

public class ObjectCollection : MonoBehaviour
{
    public int Paper = 0; //Amount of Papers collected so far
    public int paperToWin = 7; //How many papers you need to collect to win the game
    public GameObject ExitGate;
    public GameObject Menus;

    Pausememu PauseScript;

    void Start(){
        PauseScript = Menus.GetComponent<Pausememu>();
    }

    /*

        NAME: OnTriggerEnter
        PARAMTERS: Collider ohter
        PURPOSE: Increment papers collected and delete that object from the game scene
        PRECONDTION: Player and Paper object are within a radius of 2 units
        POSTCONDTION: Remove the Paper object from the scene and increment
        the Papers collected score.

    */

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Paper")
        {
            Paper += 1;
            Debug.Log("A paper was picked up. Total papers = " + Paper);
            Destroy(other.gameObject);

        }
        if (other.gameObject.tag == "Toby")
        {
            Debug.Log("Toby");
            PauseScript.Pause(PauseScript.GameOverUI);
        }
        if (other.gameObject.tag == "Exit")
        {
            Debug.Log("Exit");
            PauseScript.Pause(PauseScript.GameOverUI);
        }
    }

    /*

        NAME: OnGUI
        PARAMETER: NONE
        PURPOSE: Simple display of Papers collected and what to do when you have them all
        PRECONDTION: Game must have started
        POSTCONDTION: Changed score any time new Paper are picked up or
        all papers are collected

    */

    void OnGUI()
    {
        if (Paper < paperToWin)
        {
            GUI.Box(new Rect((Screen.width / 2) - 100, 10, 200, 35), "" + Paper + " out of 7 Papers collected");
        }
        else
        {
            GUI.Box(new Rect((Screen.width / 2) - 100, 10, 200, 35), "All papers collected, get out of the castle!");
            ExitGate.SetActive(true);
        }
    }


}
