using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private Transform forceTransform;
    [SerializeField] private bool oneTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        ForceMode mode = (oneTime) ? ForceMode.Impulse : ForceMode.Force;
        var rb = other.GetComponent<Rigidbody>();
        if (rb == null) rb = other.GetComponentInParent<Rigidbody>();
        rb.AddForce(forceTransform.rotation * Vector3.forward * force, mode);
    }
}
