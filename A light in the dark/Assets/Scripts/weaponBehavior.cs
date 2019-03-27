using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotation;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation, rotation, rotation);
    }
}
