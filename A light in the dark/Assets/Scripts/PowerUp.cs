using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != "Player") {
            return;
        }

        gameObject.SetActive(false);
    }
}


public enum PowerUpType {
    Spotlight,
    Invisibility,
    ExtraLight,
    Key
}
