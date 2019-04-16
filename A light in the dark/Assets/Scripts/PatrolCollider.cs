using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolCollider : MonoBehaviour
{
    Monster monster;
    // Start is called before the first frame update
    void Start()
    {
        monster = transform.parent.GetComponent<Monster>();
    }

    private void OnTriggerEnter(Collider other) {
        monster.OnChildTriggerEnter(other);
    }
}
