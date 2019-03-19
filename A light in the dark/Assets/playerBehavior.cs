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

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;

        dropLight1.SetActive(false);
        dropLight2.SetActive(false);

        health = 3;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                mouseClickPos = hit.point;
                GoToWaypoint();
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            CheckLight();
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
        light.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        light.SetActive(true);
        ignoreNextLightCollision = true;
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

        if (other.gameObject == dropLight1)
            dropLight1.SetActive(false);
        else if (other.gameObject == dropLight2)
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
