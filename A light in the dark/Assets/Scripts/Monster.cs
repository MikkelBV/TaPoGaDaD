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
    private bool isPlayerTargeted;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = 3;
        isPlayerTargeted = false;

        GoToNextPoint();
    }

    void Update()
    {
        if (isPlayerTargeted) {
            if (Vector3.Distance(transform.position, player.transform.position) > 3.0f) {
                isPlayerTargeted = false;
            } else {
                agent.SetDestination(player.transform.position);
            }
        } else if (!agent.pathPending && agent.remainingDistance < 0.5f){
            GoToNextPoint();
        }
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

    public void OnChildTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            isPlayerTargeted = true;
        }
    }
}
