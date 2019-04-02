using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Monster : MonoBehaviour {
    private NavMeshAgent agent;
    public Player player;
    private int health;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;
        health = 3;
    }

    void Update() {
        if (Player.LIGHT.activeSelf) {
            agent.SetDestination(Player.LIGHT.transform.position);
        } else {
            agent.SetDestination(Player.PLAYER.transform.position);
        }
    }

    //set light attributes on monster collision
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Light1" && !player.isPredator){
            //light2 = other.GetComponent<Light>();
            other.gameObject.gameObject.GetComponent<Light>().color = Color.red;
            other.gameObject.GetComponent<Light>().range = 2;
            other.gameObject.GetComponent<Light>().intensity = 1;
        } else if (other.gameObject.tag == "Light1" && player.isPredator){
            health--;
            other.gameObject.gameObject.SetActive(false);
            Debug.Log(health);
            Debug.Break();
        }
    }
}
