using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerBehavior : MonoBehaviour {
    public GameObject dropLight1;
    public GameObject dropLight2;
    public Monster monster;
    

    private Vector3 mouseClickPos;
    private Transform waypoint;
    private NavMeshAgent agent;
    private bool ignoreNextLightCollision = false;
    private int health;

    private Rigidbody rigb; 
    public float pSpeed = 1f;

    private Vector3 worldMousePos;
    private float modifierZ;
    private float modifierX;

    [HideInInspector]
    public bool lightSpawnable;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;

        dropLight1.SetActive(false);
        dropLight2.SetActive(false);
        lightSpawnable = true;

        health = 3;

        rigb = GetComponent<Rigidbody>();

    }

    void Update() {

        if (Input.GetKey(KeyCode.W)){
            rigb.AddForce(0, 0, pSpeed, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.A)){
            rigb.AddForce(-pSpeed, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.S)){
            rigb.AddForce(0, 0, -pSpeed, ForceMode.VelocityChange);
        }
        if (Input.GetKey(KeyCode.D)){
            rigb.AddForce(pSpeed, 0, 0, ForceMode.VelocityChange);
        }
    
       
        if (Input.GetMouseButtonDown(0)) {


            /*** Click to move ***/
            /*
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                mouseClickPos = hit.point;
                GoToWaypoint();
            }
            */
            
            //TODO
            //Limit so light cannot be placed if already placed
            if (lightSpawnable){
            CheckLight();
            ActivateLight(dropLight1);
            lightSpawnable = false;
            }

        }
        worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(1)) {
            CheckLight();
            
            Vector3 direction = worldMousePos - transform.position;
            direction.y = 0.0f;
            direction.Normalize();
            //float lightVelocity = direction.magnitude;
            const float lightVelocity = 10f;

            dropLight2.GetComponent<Rigidbody>().velocity = lightVelocity * direction;
            direction.Normalize();
            ActivateLight(dropLight2);
        }
    }

    void GoToWaypoint() {
        agent.destination = mouseClickPos;
    }

    void CheckLight() {

        if (!dropLight1.activeSelf) {
            ActivateLight(dropLight1);
        } else if (!dropLight2.activeSelf) {
            ActivateLight(dropLight2);
        }
        

        monster.LightsUpdated(dropLight1, dropLight2);
    }

    void ActivateLight(GameObject light) {
        if (worldMousePos.x > transform.position.x){
            modifierZ = 0f;
            modifierX = 0.5f;
        }
        else if (worldMousePos.x < transform.position.x){
            modifierZ = 0f;
            modifierX = -0.5f;
        }
        /* DOES NOT FUCKING WORK
        else if (Mathf.Abs((transform.position.x-worldMousePos.x)) < 1 && worldMousePos.z > transform.position.z){
            modifierZ = 0.5f;
            modifierX = 0f;
        }
        else if (Mathf.Abs((transform.position.x-worldMousePos.x)) < 1 && worldMousePos.z < transform.position.z){
            modifierZ = -0.5f;
            modifierX = 0f;
        }
        */
        Debug.Log(transform.position.x-worldMousePos.x);

        light.transform.position = new Vector3(transform.position.x+modifierX, 0, transform.position.z+modifierZ);
        light.SetActive(true);
        ignoreNextLightCollision = true;
        
        //set light attributes on placement
        light.GetComponent<Light>().color = Color.white;
        light.GetComponent<Light>().range = 150f;
        light.GetComponent<Light>().intensity = 1.75f;

    }


    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Trap") {
            Debug.Log("1 damage");
            TakeDamage(1);
            return;
        }

        if (ignoreNextLightCollision) {
            ignoreNextLightCollision = false;
            return;
        }

        if (other.gameObject.tag == "Light1"){
            dropLight1.SetActive(false);
            lightSpawnable = true;
        }else if (other.gameObject.tag == "Light2")
            dropLight2.SetActive(false);

        monster.LightsUpdated(dropLight1, dropLight2);
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Monster") {
            Debug.Log("3 damage");
            TakeDamage(3);
        }
    }

    void TakeDamage(int damage) {
        health -= damage;

        if (health <= 0) {
            Debug.Log("bitch u ded");
            enabled = false;
        }
    }
}
