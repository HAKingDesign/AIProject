using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject Light;

    void Start()
    {
        Light.SetActive(true);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) 
            {
                if(Light.activeSelf)
                {
                    Light.SetActive(false);
                }
                else
                {
                    Light.SetActive(true);
                }
                                
            }
            
    }
}
