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
        health = 3;
    }

    void Update() {
        if (Player.LIGHT.activeSelf && !player.isPredator ) {
            agent.speed = 1.5f;
            agent.SetDestination(Player.LIGHT.transform.position);
        } else if (!player.isInvisible) {
            agent.speed = 3f;
            agent.SetDestination(Player.PLAYER.transform.position);
        } else {
            agent.SetDestination(transform.position);
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
