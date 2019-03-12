using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerBehavior : MonoBehaviour
{

    public GameObject light;
    private Vector3 mouseClickPos;

    private Transform waypoint;
    private int waypointIndex;
    private NavMeshAgent agent;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;
    }
    
    void GoToWaypoint(){
        agent.destination = mouseClickPos;
        //destPoint = (destPoint+1) % navPoints.Length;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)){
                mouseClickPos = hit.point;
            }

            Instantiate(light, new Vector3(0+mouseClickPos.x, 0.20f, 0+mouseClickPos.z), Quaternion.identity);

            Debug.Log(mouseClickPos);
        }

        if (Input.GetMouseButtonDown(1)){
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)){
                mouseClickPos = hit.point;
                GoToWaypoint();
                //if (!agent.pathPending && agent.remainingDistance < 0.5f){
                //}
            }
        }

    }
}
