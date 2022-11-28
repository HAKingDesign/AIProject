/*

    AUTHOR: Sebastian Baglo
    FILENAME: MusicControlScript.cs
    SPECIFICATION: Control and manage music in-game
    FOR: CS3368 Introducation to Artifical Intelligence Section 001

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    NAME: MusicControlScript
    PURPOSE: Manage the in-game music playing when starting a game
    INVARIANTS: When the music is done it should replay

*/

public class MusicControlScript : MonoBehaviour
{
    public static MusicControlScript instance; //Make the sound an instance so we can accsess it

    /*

    NAME: Awake
    PARAMETERS: None
    PURPOSE: Initiate the music playing when game starts.
    PRECONDITION: Must start a game for it too trigger
    POSTCONDITION: Music loops

    */
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
