using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endScreen : MonoBehaviour
{

    public GameObject Screen;
    private bool isDead = false;


     void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // isDead = true;
            Screen.gameObject.SetActive(true);
        }

        if (isDead == false)
        {
         Screen.gameObject.SetActive(false);
        } else
         {
             Screen.gameObject.SetActive(true);
         }

        /*
        if(health > 0)
        {
            Screen.gameObject.SetActive(false)
        } else
        {
            Screen.gameObject.SetActive(true);
        }
        */
      
    }

}
