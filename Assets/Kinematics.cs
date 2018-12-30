using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinematics : MonoBehaviour {

    public Transform start;
    public Transform dest;
    public float period = 1.0f;
    List<Actor> arr = new List<Actor>();

    private void Start()
    {
        transform.position = start.position;
    }

    float tsum = 0;
    float sign = 1;
	void Update () {
        tsum += Time.deltaTime * sign;

        transform.position = (dest.position - start.position) * tsum * 2 / period + start.position;

        if (tsum * 2 >= period)
            sign *= -1;
        else if (tsum * 2 < 0)
            sign = 1;

        var narr = new List<Actor>();
        foreach(var i in arr)
        {
            if (Mathf.Abs(i.transform.position.y - transform.position.y) < 0.2f)
            {
                narr.Add(i);
                i.transform.Translate((dest.position - start.position) * Time.deltaTime * sign * 2 / period);
            }
        }
        arr = narr;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        Scaffold scf = coll.GetComponent<Scaffold>();
        Actor actor = coll.GetComponentInParent<Actor>();
        if (scf && actor && scf.pos == ScaffoldPos.bottom && !arr.Contains(actor))
        {
            foreach (var i in arr)
                Debug.Log(i.name);
        }

    }
}
