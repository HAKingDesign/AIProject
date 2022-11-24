using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Pausememu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GamePaused = false;

    public GameObject PauseMenuUI;

    public GameObject playerObject;

    private FirstPersonController playerScript;
    
    void Start(){
        playerScript = playerObject.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            } else 
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        playerScript.cameraCanMove=true;
        playerScript.lockCursor=true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        playerScript.cameraCanMove=false;
        playerScript.lockCursor=false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void QuitGame(){
        Debug.Log("Quit");
        Application.Quit();
    }
}
