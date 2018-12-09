using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    public Sprite[] idle, walk, jump;
    SpriteRenderer sr;
    Sprite[] cur_anim;
    int idx = 0;

    Rigidbody2D rb;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
        cur_anim = idle;
        InvokeRepeating("Animate", 0, 0.1f);

        rb = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        float lrbias = Input.GetAxis("Horizontal");
        //if jump control should not allow, move this code to not jump block.
        rb.velocity = new Vector2(lrbias * 2, rb.velocity.y);

        //jump, this method has a little dangers. while jump max pos, velocity.y can be 0. Imagine parabola.
        if (rb.velocity.y != 0)
        {
            cur_anim = jump;
            idx = 0;

            if (lrbias > 0)
                sr.flipX = false;
            else if (lrbias < 0)
                sr.flipX = true;
        }
        //not jump
        else
        {
            if (lrbias > 0)
            {
                if (cur_anim != walk)
                {
                    cur_anim = walk;
                    idx = 0;
                }
                sr.flipX = false;
            }
            else if (lrbias < 0)
            {
                if (cur_anim != walk)
                {
                    cur_anim = walk;
                    idx = 0;
                }
                sr.flipX = true;
            }
            else
            {
                if (cur_anim != idle)
                {
                    cur_anim = idle;
                    idx = 0;
                }
            }

            if (Input.GetButtonDown("Jump"))
                rb.velocity = new Vector2(rb.velocity.x, 4);
        }
    }
	
	void Animate () {
        sr.sprite = cur_anim[idx = (idx+1) % cur_anim.Length];
	}
}
