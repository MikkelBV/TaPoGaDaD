using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeLight : MonoBehaviour
{

    public GameObject light;
    private Vector3 mouseClickPos;
    void Start()
    {
        
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
    }
}
