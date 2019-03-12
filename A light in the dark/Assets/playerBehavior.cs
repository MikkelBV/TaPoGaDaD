using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerBehavior : MonoBehaviour
{
    float clicked = 0;
    float clicktime = 0;
    float clickDelay = 0.8f;

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

    bool DoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked++;
            if (clicked == 1) clicktime = Time.time;
        }
        if (clicked > 1 && Time.time - clicktime < clickDelay)
        {
            clicked = 0;
            clicktime = 0;
            return true;
        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
        return false;
    }
    
    void GoToWaypoint(){
        agent.destination = mouseClickPos;
        //destPoint = (destPoint+1) % navPoints.Length;
    }


    void Update() // check at clickdelay inden i bevægelsen
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (Time.time - clicktime > clickDelay)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    mouseClickPos = hit.point;
                        GoToWaypoint();

                }
            }

            if (DoubleClick())
            {
                Instantiate(light, new Vector3(0 + transform.position.x, 0.20f, 0 + transform.position.z), Quaternion.identity);
            }
           

        }
    }
}
