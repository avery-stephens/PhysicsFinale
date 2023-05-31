using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Respawnable : MonoBehaviour
{
    GameObject prefab;

    public void Start()
    {
        //prefab = GetComponentInChildren<GameObject>();
        Respawn();        
    }

    public void Respawn()
    {
        // destroy all children
        //Destroy(GetComponentInChildren<GameObject>());
        //Instantiate
        //Instantiate(prefab, transform.position, transform.rotation, transform);
        
        //gameObject.SetActive(true);
    }
}
