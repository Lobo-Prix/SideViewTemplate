using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected Rigidbody2D rb;
    Vector3 ppos;

    protected virtual void Start()
    {
        ppos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        Vector3 vec = transform.position - ppos;
        //Debug.DrawRay(ppos, vec, Color.green, 1);
        if (rb.gravityScale != 0 && rb.velocity.y <= 0 && Physics2D.Raycast(ppos, vec, vec.magnitude * 1.1f, LayerMask.GetMask(new string[] { "JerkWall" })))
        {
            transform.position = new Vector3(transform.position.x, ppos.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        ppos = transform.position;
    }

    public int scaffold_col = 0;
    public int left_col = 0;
    public int right_col = 0;

    public abstract void AddDamage(Actor from);
}
