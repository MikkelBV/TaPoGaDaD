using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endScreen : MonoBehaviour
{

    public GameObject Screen;
    private bool isDead = false;


     void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDead = true;
        }

        if (isDead == false)
        {
         Screen.gameObject.SetActive(false);
        } else
         {
             Screen.gameObject.SetActive(true);
         }

      
    }


}
