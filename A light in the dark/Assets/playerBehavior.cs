using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerBehavior : MonoBehaviour
{
    float clicked = 0;
    float clicktime = 0;
    float clickDelay = 0.8f;

    public GameObject dropLight1;
    public GameObject dropLight2;
    private int lightCount;
    private Vector3 mouseClickPos; 
    private float distL1, distL2;

    private Transform waypoint;
    private int waypointIndex;
    private NavMeshAgent agent;
    
    void Start()
    {
        lightCount = 0;
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

    void CheckLight(){
        GameObject Light1 = GameObject.Find("Drop Light 1(Clone)");
        GameObject Light2 = GameObject.Find("Drop Light 2(Clone)");

        if (lightCount < 2){
            if (!Light1){
                Instantiate(dropLight1, new Vector3(0 + transform.position.x, 0.20f, 0 + transform.position.z), transform.rotation);
                lightCount++;
            }
            else if (!Light2){
                Instantiate(dropLight2, new Vector3(0 + transform.position.x, 0.20f, 0 + transform.position.z), transform.rotation);
                lightCount++;
            }
        }

        if (Light1 && Vector3.Distance(Light1.transform.position, transform.position) < 0.3){
            Destroy(Light1);
            lightCount--;
        }
        if (Light2 && Vector3.Distance(Light2.transform.position, transform.position) < 0.3){
            Destroy(Light2);
            lightCount--;
        }

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
                /*
                lights.Add(
                    Instantiate(dropLight, new Vector3(0 + transform.position.x, 0.20f, 0 + transform.position.z), transform.rotation) as GameObject
                );            

                Debug.Log(lights.Count);
                */
            }
        }
        if (Input.GetMouseButtonDown(1)){
                CheckLight();
        }



    }
}
