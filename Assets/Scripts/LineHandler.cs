using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineHandler : MonoBehaviour
{
    [SerializeField] private Transform origin;
    public Transform destination;
    private LineRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<LineRenderer>();
        //if (destination == null) destination = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (renderer.enabled)
        {
            renderer.SetPosition(0, origin.position);

            renderer.SetPosition(1, destination.position);
        }
    }
    public void Activate()
    {
        renderer.enabled = true;
    }
    public void Deactivate()
    {
        renderer.enabled = false;
    }
    public void Toggle()
    {
        renderer.enabled = (renderer.enabled) ? false : true;
    }
}
