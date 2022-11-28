using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Pausememu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool GamePaused = false;

    public GameObject PauseMenuUI;

    public GameObject GameOverUI;

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
                Resume(PauseMenuUI);
            } else 
            {
                Pause(PauseMenuUI);
            }
        }
    }
    public void PauseResume(){
        Resume(PauseMenuUI);
    }

    public void Resume(GameObject UI)
    {
        UI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        playerScript.cameraCanMove=true;
        playerScript.lockCursor=true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Pause(GameObject UI)
    {   
        UI.SetActive(true);
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
    public void toMainMenu(){
        SceneManager.LoadScene(1);
    } 
}
