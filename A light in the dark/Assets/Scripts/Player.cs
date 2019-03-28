using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {
    public static GameObject PLAYER, LIGHT;
    public GameObject lightObject;
    public GameObject weapon;
    public Monster monster;
    public float lightVelocity = 10f;
    public float pSpeed = 1f;
    public float lightSpawnOffset = 1f;

    [HideInInspector] public bool isPredator;
    [HideInInspector] public bool isLightCollected;

    private Transform waypoint;
    private NavMeshAgent agent;
    private int health;

    private Rigidbody rigb;
    private float modifierZ;
    private float modifierX;


    void Start() {
        rigb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;

        lightObject.SetActive(false);
        isLightCollected = true;
        health = 3;

        PLAYER = gameObject;
        LIGHT = lightObject;
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


    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Trap") {
            Debug.Log("1 damage");
            TakeDamage(1);
            return;
        }

        if (other.gameObject.tag == "Weapon"){
            isPredator = true;
            weapon.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Monster") {
            Debug.Log("3 damage");
            TakeDamage(3);
        } else if (other.gameObject.tag == "Light1"){
            lightObject.SetActive(false);
            isLightCollected = true;
        }
    }

    void TakeDamage(int damage) {
        health -= damage;

        if (health <= 0) {
            enabled = false;
            Debug.Log("bitch u ded");
            Debug.Break();
        }
    }
}
