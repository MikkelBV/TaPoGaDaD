using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{   // https://www.uihere.com/free-cliparts/vector-graphics-clip-art-pixel-8-bit-color-image-8bit-heart-7119588 - heath png
    // Start is called before the first frame update

    private int health = 3;
    public Text healthText;



    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health : " + health;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            health--;
            print("Health: " + health);
        }


    }
}
