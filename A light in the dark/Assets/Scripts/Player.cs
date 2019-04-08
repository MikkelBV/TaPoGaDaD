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
    public Light spotLight; 
    public float lightVelocity = 10f;
    public float pSpeed = 1f;
    public float lightSpawnOffset = 1f;
    public float lookSpeed = 5f; 

    public Vector3 worldMousePos;
    public Vector3 direction; 

    [HideInInspector] public bool isPredator;
    [HideInInspector] public bool isLightCollected;
    [HideInInspector] public bool isInvisible;

    private Transform waypoint;
    private int health;

    private Rigidbody rigb;
    private float modifierZ;
    private float modifierX;

    private float timer;



    void Start() {
        rigb = GetComponent<Rigidbody>();
        
        GetComponentInChildren<Light>().enabled = false;
        lightObject.SetActive(false);
        isLightCollected = true;
        health = 3;
        healthText.text = "Health : " + health;
        isInvisible = false;

        PLAYER = gameObject;
        LIGHT = lightObject;

        Screen.SetActive(false);
    }

    void Update() {
        
        //Mouse orientation
        direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(direction.x, direction. y) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
      
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
        } else {
            monster.agent.speed = 1.5f;
        }

        if (isInvisible){
            timer -= Time.deltaTime;
            Debug.Log(timer);
            if (timer < 0) isInvisible = false;
        }
    }

    float getMouseDist(Vector3 position){
        return Mathf.Abs(transform.position.x - position.x);
    }

    void ActivateLight() {
        isLightCollected = false;
      
        worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = worldMousePos - transform.position; 
        direction.y = 0f; 
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
        spotLight = GetComponentInChildren<Light>();
        spotLight.enabled = !spotLight.enabled;
    }

    void PowerUpInvisibility() {
        isInvisible = true;
        timer = 10.0f;
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

    public void ResetLight() {
        lightObject.SetActive(false);
        isLightCollected = true;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Weapon"){
            isPredator = true;
            weapon.SetActive(false);
        } else if (other.gameObject.tag == "Monster") {
            TakeDamage(3);
        } else if (other.gameObject.tag == "Light1") {
            ResetLight();
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
