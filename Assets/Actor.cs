using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected Rigidbody2D rb;
    Collider2D coll;
    Vector3 ppos;

    protected virtual void Start()
    {
        ppos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        Vector3 vec = transform.position - ppos;
        //Debug.DrawRay(ppos, vec, Color.green, 1);
        if (rb.gravityScale != 0 && rb.velocity.y <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(ppos, vec, vec.magnitude * 1.1f, LayerMask.GetMask(new string[] { "TriggerWall" }));
            if (hit /*&& hit.collider.bounds.Intersects()*/)
            {
                transform.position = new Vector3(transform.position.x, ppos.y, transform.position.z);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                hit.transform.SendMessage("OnTriggerEnter2D", coll, SendMessageOptions.DontRequireReceiver);
            }
        }
        ppos = transform.position;
    }

    public int scaffold_col = 0;
    public int left_col = 0;
    public int right_col = 0;

    public abstract void AddDamage(Actor from);
}
