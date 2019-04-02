using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static GameObject PLAYER, LIGHT;
    public GameObject lightObject;
    public GameObject weapon;
    public Monster monster;
    public GameObject Screen;
    public Text healthText;
    public float lightVelocity = 10f;
    public float pSpeed = 1f;
    public float lightSpawnOffset = 1f;

    [HideInInspector] public bool isPredator;
    [HideInInspector] public bool isLightCollected;

    private Transform waypoint;
    private int health;

    private Rigidbody rigb;
    private float modifierZ;
    private float modifierX;


    void Start() {
        rigb = GetComponent<Rigidbody>();

        lightObject.SetActive(false);
        isLightCollected = true;
        health = 3;
        healthText.text = "Health : " + health;

        PLAYER = gameObject;
        LIGHT = lightObject;

        Screen.SetActive(false);
    }

    void Update() {
        //WASD Movement
        if (Input.GetKey(KeyCode.W))
            rigb.AddForce(0, 0, pSpeed, ForceMode.VelocityChange);
        if (Input.GetKey(KeyCode.A))
            rigb.AddForce(-pSpeed, 0, 0, ForceMode.VelocityChange);
        if (Input.GetKey(KeyCode.S))
            rigb.AddForce(0, 0, -pSpeed, ForceMode.VelocityChange);
        if (Input.GetKey(KeyCode.D))
            rigb.AddForce(pSpeed, 0, 0, ForceMode.VelocityChange);

        if (Input.GetMouseButtonDown(1) && isLightCollected) {

            ActivateLight();
        }

        if (isPredator){
            lightObject.GetComponent<Light>().color = new Color(Random.value, Random.value, Random.value);
        }

        if (isLightCollected == true){
            monster.agent.speed = 0.7f;
        }
        else {
            monster.agent.speed = 1.5f;
        }


    }

    float getMouseDist(Vector3 position){
        return Mathf.Abs(transform.position.x - position.x);
    }

    void ActivateLight() {
        isLightCollected = false;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = worldMousePos - transform.position;

        direction.y = 0.0f;
        direction.Normalize();
        lightObject.GetComponent<Rigidbody>().velocity = lightVelocity * direction;

        lightObject.transform.position = new Vector3(transform.position.x + direction.x * lightSpawnOffset, 0, transform.position.z + direction.z * lightSpawnOffset);
        lightObject.SetActive(true);

        //set light attributes on activation
        if(!isPredator){
            lightObject.GetComponent<Light>().color = Color.white;
            lightObject.GetComponent<Light>().range = 150f;
            lightObject.GetComponent<Light>().intensity = 1.75f;
        }
        else{
            lightObject.GetComponent<Light>().color = Color.green;
            lightObject.GetComponent<Light>().range = 100;
            lightObject.GetComponent<Light>().intensity = 3f;
        }
    }

    void PowerUpSpotlight() {
        Debug.Log("Jeppi");
    }

    void PowerUpInvisibility() {
        Debug.Log("Ismail");
    }

    void PowerUpExtraLight() {
        Debug.Log("Somin");
    }

    void OnPowerUp(PowerUpType type) {
        switch(type) {
            case PowerUpType.Spotlight:
                PowerUpSpotlight();
                break;
            case PowerUpType.Invisibility:
                PowerUpInvisibility();
                break;
            case PowerUpType.ExtraLight:
                PowerUpExtraLight();
                break;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Weapon"){
            isPredator = true;
            weapon.SetActive(false);
        } else if (other.gameObject.tag == "Monster") {
            TakeDamage(3);
        } else if (other.gameObject.tag == "Light1") {
            lightObject.SetActive(false);
            isLightCollected = true;
        } else if(other.gameObject.tag == "Trap"){
            TakeDamage(1);
        } else if (other.gameObject.tag == "PowerUp") {
            OnPowerUp(other.gameObject.GetComponent<PowerUp>().type);
        }
    }

    void TakeDamage(int damage) {
        health -= damage;
        healthText.text = "Health : " + health;

        if (health <= 0) {
            enabled = false;
            Screen.SetActive(true);
        }
    }
}
