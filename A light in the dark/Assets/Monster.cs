using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Monster : MonoBehaviour {
    private NavMeshAgent agent;

    public GameObject light1;
    public GameObject light2;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;
    }

    public void LightsUpdated(GameObject light1, GameObject light2) {
        if (!light1.activeSelf && !light2.activeSelf) {
            agent.SetDestination(transform.position);
        } else if (!light1.activeSelf) {
            agent.SetDestination(light2.transform.position);
        } else if (!light2.activeSelf) {
            agent.SetDestination(light1.transform.position);
        } else {
            var closest = GetClosestLight(light1, light2);
            agent.SetDestination(closest.transform.position);
        }

    }

    void OnTriggerEnter(GameObject other){
        private Light light1 = GetComponent<Light>();
        if (other.tag == "Light1"){
            light1.color = Color.red;
        }
        else if (other.tag == "Light2"){
            light2.color = Color.red;
        }
    }


    GameObject GetClosestLight(GameObject light1, GameObject light2) {
        var distLight1 = Vector3.Distance(transform.position, light1.transform.position);
        var distLight2 = Vector3.Distance(transform.position, light2.transform.position);

        return distLight1 < distLight2 ? light1 : light2;
    }

    void GoToWaypoint(Vector3 waypoint) {
        agent.destination = waypoint;
    }
}
