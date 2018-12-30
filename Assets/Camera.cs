using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float free_radius_x;
    public float free_radius_y;
    public float z = -10;

    void Start()
    {
        
    }
    
    void Update()
    {
        var xdiff = transform.position.x - target.transform.position.x;
        var ydiff = transform.position.y - target.transform.position.y;
        if (Mathf.Abs(xdiff) > free_radius_x)
            transform.position = new Vector3(target.transform.position.x + xdiff / Mathf.Abs(xdiff) * free_radius_x, transform.position.y, z);
        if (Mathf.Abs(ydiff) > free_radius_y)
            transform.position = new Vector3(transform.position.x, target.transform.position.y + ydiff / Mathf.Abs(ydiff) * free_radius_y, z);
    }
}
