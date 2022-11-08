using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollection : MonoBehaviour
{
    public int Paper = 0;
    public int paperToWin = 3;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Paper")
        {
            Paper += 1;
            Debug.Log("A paper was picked up. Total papers = " + Paper);
            Destroy(other.gameObject);

        }
    }

    void OnGUI()
    {
        if (Paper < paperToWin)
        {
            GUI.Box(new Rect((Screen.width / 2) - 100, 10, 200, 35), "" + Paper + " Papers");
        }
        else
        {
            GUI.Box(new Rect((Screen.width / 2) - 100, 10, 200, 35), "All papers collected!");
        }
    }


}
