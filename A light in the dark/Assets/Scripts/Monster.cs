using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Monster : MonoBehaviour
{
    public NavMeshAgent agent;
    public Player player;
    private int health;

    public Transform[] points;
    private int destPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = 3;

        GoToNextPoint();
    }

    void Update()
    {

        if (!agent.pathPending && agent.remainingDistance <0.5f){
            GoToNextPoint();
        }
        /*
        if (Player.LIGHT.activeSelf && !player.isPredator ) {
            agent.speed = 1.5f;
            agent.SetDestination(Player.LIGHT.transform.position);
        } else if (!player.isInvisible) {
            agent.speed = 1.5f;
            agent.SetDestination(Player.PLAYER.transform.position);
        } else {
            agent.SetDestination(transform.position);
        }
        */
    }

    public void GoToNextPoint()
    {
        if (points.Length == 0)
        {
            return;
        }

        agent.destination = points[destPoint].position;

        destPoint = (destPoint + 1) % points.Length;
    }

    //set light attributes on monster collision
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Light1" && !player.isPredator)
        {
            health += 3;
            player.ResetLight();
        }
        else if (other.gameObject.tag == "Light1" && player.isPredator)
        {
            health--;
            other.gameObject.gameObject.SetActive(false);
            Debug.Log(health);
            Debug.Break();
        }
    }
    */
}
