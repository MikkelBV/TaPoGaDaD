﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static GameObject PLAYER, LIGHT;
    public GameObject lightObject;
    public GameObject weapon;
    public Monster monster;
    public GameObject Screen;
    public Text healthText;
    public GameObject splashCanvas;
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
    private int keysCollected;

    public GameObject door;
    private Light doorLight;
    private ParticleSystem doorParticle;
    private BoxCollider doorCollider;
    private NavMeshAgent lightAgent;

    private float splashTimer;
    private bool canMove;

    public GameObject DeathAudio;
    private bool doorOpen;

    public GameObject DoorBlock;
    void Start()
    {
        rigb = GetComponent<Rigidbody>();
        doorLight = door.GetComponent<Light>();
        doorParticle = door.GetComponent<ParticleSystem>();
        doorCollider = door.GetComponent<BoxCollider>();

        lightObject.SetActive(false);
        isLightCollected = true;
        health = 3;
        healthText.text = "Health : " + health;
        isInvisible = false;
        PLAYER = gameObject;
        LIGHT = lightObject;
        keysCollected = 0;
        doorOpen = false;

        Screen.SetActive(false);
        lightAgent = lightObject.GetComponent<NavMeshAgent>();
        lightAgent.enabled = false;

        canMove = true;
        splashTimer = 3f;
        //Cursor.visible = false;
    }

    void Update()
    {

        splashTimer -= Time.deltaTime;
        if (splashTimer < 0)
        {
            splashCanvas.SetActive(false);
        }
        //Mouse orientation
        direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        //WASD Movement
        float speed = pSpeed * Time.deltaTime;
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.forward * speed, Space.World);
            if (Input.GetKey(KeyCode.S))
                transform.Translate(Vector3.forward * -speed, Space.World);
            if (Input.GetKey(KeyCode.A))
                transform.Translate(Vector3.right * -speed, Space.World);
            if (Input.GetKey(KeyCode.D))
                transform.Translate(Vector3.right * speed, Space.World);
        }
        if (Input.GetKey(KeyCode.Escape))
            Application.LoadLevel(Application.loadedLevel);

        if (Input.GetMouseButtonDown(0) && isLightCollected)
        {
            ActivateLight();
            timerLight = 5.0f;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CollectLight();
        }

        if (isPredator)
        {
            lightObject.GetComponent<Light>().color = new Color(Random.value, Random.value, Random.value);
        }

        if (invisibilityReady == true && Input.GetKey(KeyCode.R))
        {
            isInvisible = true;
            timerInvis = 10.0f;
            GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f, 1f);
            //GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1f, 1f, 1f, 1f));
            Debug.Log("Ismail");
        }

        if (isInvisible)
        {
            timerInvis -= Time.deltaTime;
            if (timerInvis < 0)
            {
                isInvisible = false;
                invisibilityReady = false;
                GetComponent<Renderer>().material.color = new Color(0, 0.8f, 0, 1f);
                //GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0f, 1f, 0f, 1f));
            }
        }

        if (lightAgent.enabled)
        {
            lightAgent.SetDestination(transform.position);
        }
    }

    float getMouseDist(Vector3 position)
    {
        return Mathf.Abs(transform.position.x - position.x);
    }

    void CollectLight()
    {
        lightAgent.enabled = true;
    }
    void ActivateLight()
    {
        isLightCollected = false;

        worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = worldMousePos - transform.position;
        direction.y = 0f;
        direction.Normalize();

        lightObject.GetComponent<Rigidbody>().velocity = lightVelocity * direction;
        lightObject.transform.position = new Vector3(transform.position.x + direction.x * lightSpawnOffset, 0, transform.position.z + direction.z * lightSpawnOffset);
        lightObject.SetActive(true);

        //set light attributes on activation
        if (!isPredator)
        {
            lightObject.GetComponent<Light>().color = Color.white;
            lightObject.GetComponent<Light>().range = 150f;
            lightObject.GetComponent<Light>().intensity = 1.75f;
        }
        else
        {
            lightObject.GetComponent<Light>().color = Color.green;
            lightObject.GetComponent<Light>().range = 100;
            lightObject.GetComponent<Light>().intensity = 3f;
        }
    }

    void PowerUpSpotlight()
    {
        spotLight = GetComponentInChildren<Light>();
        spotLight.enabled = !spotLight.enabled;
    }

    void PowerUpExtraLight()
    {
        Debug.Log("Somin");
    }

    void PowerUpKey()
    {
        keysCollected += 1;
        Debug.Log("Picked up a key. Total: " + keysCollected);

        if (keysCollected == 3)
        {
            doorLight.color = Color.green;
            var doorParticleSettings = doorParticle.main;
            doorParticleSettings.startColor = new Color(0, 1, 0, 1);
            doorOpen = true;
            DoorBlock.SetActive(false);
            //doorCollider.enabled = false;
        }
    }
    void OnPowerUp(PowerUpType type)
    {
        switch (type)
        {
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

    public void ResetLight()
    {
        lightObject.GetComponent<NavMeshAgent>().enabled = false;
        lightObject.SetActive(false);
        isLightCollected = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            isPredator = true;
            weapon.SetActive(false);
        }
        else if (other.gameObject.tag == "Monster")
        {
            TakeDamage(3);
        }
        else if (other.gameObject.tag == "Light1")
        {
            ResetLight();
        }
        else if (other.gameObject.tag == "Trap")
        {
            TakeDamage(1);
        }
        else if (other.gameObject.tag == "PowerUp")
        {
            OnPowerUp(other.gameObject.GetComponent<PowerUp>().type);
        }

        else if (other.gameObject.tag == "Door"){
            if (doorOpen){
                SceneManager.LoadScene("winscreen", LoadSceneMode.Single);
            }
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = "Health : " + health;

        if (health <= 0)
        {
            //enabled = false;
            DeathAudio.GetComponent<AudioSource>().Play();
            canMove = false;
            Screen.SetActive(true);
        }
    }
}
