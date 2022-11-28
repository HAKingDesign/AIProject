using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    AUTHOR: Hunter King
    FILENAME: Pausemenu.cs
    SPECIFICATION: game pause Menus fuctionalities
    FOR: CS 3368 Introduction to Artificial Intelligence Section 002
*/

public class Pausememu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GamePaused = false; // game paused flag

    public GameObject PauseMenuUI; // reference to pause menu UI game object

    public GameObject GameOverUI; // reference to game over UI game object

    public GameObject playerObject; // reference to player game object

    private FirstPersonController playerScript; // refernce to FirstPersonController component in player object
    
    void Start(){
        playerScript = playerObject.GetComponent<FirstPersonController>(); //obtain First Person Controller script connected to player object
        GameOverUI.SetActive(false); //set visiblity of Game over UI to false
        PauseMenuUI.SetActive(false); //set visiblity of Pause menu UI to false
        Time.timeScale = 1f; //set time scale of scene to 1
        GamePaused = false; //set game paused flag to false 
        playerScript.cameraCanMove=true; //set camera can move to true
        playerScript.lockCursor=true; //set lock cursor to true 
        Cursor.visible = false; //set cursor visiblity to false
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // if escape button is pressed
        {
            if (!GamePaused) // if game is not paused
            {
                Pause(PauseMenuUI); // pause the game
            }
        }
    }

    /*
    NAME: PauseResume
    PURPOSE: function for in game resume button so it can pass the the Pause Menu UI
    */

    public void PauseResume(){ 
        Resume(PauseMenuUI); 
    }

    /*
    NAME: Resume
    PURPOSE: set all values to resume gameplay
    */

    public void Resume(GameObject UI)
    {
        UI.SetActive(false); // set the passed UI to active 
        Time.timeScale = 1f; //set time scale of scene to 1
        GamePaused = false; //set game paused flag to false 
        playerScript.cameraCanMove=true; //set camera can move to true
        playerScript.lockCursor=true; //set lock cursor to true 
        Cursor.visible = false; //set cursor visiblity to false
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor
    }

    /*
    NAME: Resume
    PURPOSE: set all values to pause gameplay
    */

    public void Pause(GameObject UI)
    {   
        UI.SetActive(true); // set the passed UI to active
        Time.timeScale = 0f; // set time scale to 0
        GamePaused = true; // set pause flag to true
        playerScript.cameraCanMove=false; // set player can move camera to false
        playerScript.lockCursor=false; //set cursor lock flag to false
        Cursor.visible = true; // set cursor visibility to true
        Cursor.lockState = CursorLockMode.Confined; // confine the cursor to game window
    }

     /*
    NAME: QuitGame
    PURPOSE: exit the game
    */

    public void QuitGame(){
        Debug.Log("Quit");
        Application.Quit(); // quit out of application
    }

    /*
    NAME: toMainMenu
    PURPOSE: change scenes to main menu scene
    */

    public void toMainMenu(){
        SceneManager.LoadScene(0); // set scene to main menu scene
    } 
}
