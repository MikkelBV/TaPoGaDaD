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
    [HideInInspector] public bool invisibilityReady = false;

    private Transform waypoint;
    private int health;

    private Rigidbody rigb;
    private float modifierZ;
    private float modifierX;

    private float timerInvis;
    [HideInInspector] public float timerLight;
    private bool canShoot;
    private int keysCollected;

    public GameObject door;
    private Light doorLight;
    private ParticleSystem doorParticle;
    private BoxCollider doorCollider;


    void Start() {
        rigb = GetComponent<Rigidbody>();
        doorLight = door.GetComponent<Light>();
        doorParticle = door.GetComponent<ParticleSystem>();
        doorCollider = door.GetComponent<BoxCollider>();

        // GetComponentInChildren<Light>().enabled = false;
        lightObject.SetActive(false);
        isLightCollected = true;
        health = 3;
        healthText.text = "Health : " + health;
        isInvisible = false;
        canShoot = true;
        PLAYER = gameObject;
        LIGHT = lightObject;
        keysCollected = 0;

        Screen.SetActive(false);
    }

    void Update() {
        //Mouse orientation
        direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(direction.x, direction. y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        //WASD Movement
        float speed = pSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward * speed;
        if (Input.GetKey(KeyCode.S))
            transform.position += -1 * transform.forward * speed;
        if (Input.GetKey(KeyCode.A))
            transform.position += -1 * transform.right * speed;
        if (Input.GetKey(KeyCode.D))
            transform.position += transform.right * speed;

        if (Input.GetMouseButtonDown(1) && isLightCollected && canShoot) {
                ActivateLight();
                timerLight = 5.0f;
        }

        if (!canShoot){
            timerLight -= Time.deltaTime;
            Debug.Log(timerLight);
            if (timerLight < 0) canShoot = true;
        }

        if (isPredator){
            lightObject.GetComponent<Light>().color = new Color(Random.value, Random.value, Random.value);
        }

        if (invisibilityReady == true && Input.GetKey(KeyCode.R)) {
            isInvisible = true;
            timerInvis = 10.0f;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1f, 1f, 1f, 1f));
            Debug.Log("Ismail");
        }
        
        if (isInvisible){
            timerInvis -= Time.deltaTime;
            //Debug.Log(timerInvis);
            if (timerInvis < 0){
                isInvisible = false;
                invisibilityReady = false;
                GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0f, 1f, 0f, 1f));
            }
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

    void PowerUpExtraLight() {
        Debug.Log("Somin");
    }

    void PowerUpKey(){
        keysCollected += 1;
        Debug.Log("Picked up a key. Total: " + keysCollected);

        if (keysCollected == 3){
            doorLight.color = Color.green;
            var doorParticleSettings = doorParticle.main;
            doorParticleSettings.startColor = new Color(0,1,0,1);
            doorCollider.enabled = false;
        }
    }
    void OnPowerUp(PowerUpType type) {
        switch(type) {
            case PowerUpType.Spotlight:
                PowerUpSpotlight();
                break;
            case PowerUpType.Invisibility:
                invisibilityReady = true;
                break;
            case PowerUpType.ExtraLight:
                PowerUpExtraLight();
                break;
            case PowerUpType.Key:
                PowerUpKey();
                break;
        }
    }

    public void ResetLight() {
        canShoot = false;
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
