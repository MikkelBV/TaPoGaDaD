using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Monster : MonoBehaviour {
    public NavMeshAgent agent;
    public Player player;
    private int health;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;
        health = 3;
    }

    void Update() {
        if (Player.LIGHT.activeSelf && !player.isPredator ) {
            agent.SetDestination(Player.LIGHT.transform.position);
        } else if (!player.isInvisible) {
            agent.SetDestination(Player.PLAYER.transform.position);
        }
    }

    //set light attributes on monster collision
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Light1" && !player.isPredator){
            health += 3;
            player.ResetLight();
        } else if (other.gameObject.tag == "Light1" && player.isPredator){
            health--;
            other.gameObject.gameObject.SetActive(false);
            Debug.Log(health);
            Debug.Break();
        }
    }
}
