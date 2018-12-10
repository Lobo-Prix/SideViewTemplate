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
        sr = GetComponentInChildren<SpriteRenderer>();
        cur_anim = idle;
        InvokeRepeating("Animate", 0, 0.1f);

        rb = GetComponent<Rigidbody2D>();
	}

    public int scaffold_col = 0;
    public int left_col = 0;
    public int right_col = 0;
    bool IsGrand()
    {
        return scaffold_col > 0;
        //지금 모서리 끝의 한가운데 있으면, 두 ray는 양끝에 있어서 Ground판정이 안뜬다.
        //Player Collider 바로밑에 겹치지 않게 Trigger를 깔아서 Enter시 +1, Exit시 -1해서 0이면 충돌 없음, 1이면 충돌있음으로 처리해야할듯.(Ray가 아니라 영역으로 처리하는것임)
        //물론 Trigger는 직사각형 영역이지만, 세로높이는 매우 작게해야 이상하지 않을것이다.
        float tmp = 0.28f;
        Debug.DrawRay(transform.position + Vector3.left * tmp, Vector2.down * 0.1f, Color.green, 1);
        Debug.DrawRay(transform.position + Vector3.right * tmp, Vector2.down * 0.1f, Color.green, 1);
        return Physics2D.Raycast(transform.position + Vector3.left * tmp, Vector2.down, 0.1f) || Physics2D.Raycast(transform.position + Vector3.right * tmp, Vector2.down, 0.1f);
    }
    bool LeftColl()
    {
        return left_col > 0;
        Physics2D.Raycast(transform.position, Vector2.left, 0.4f);
    }

    bool RightColl()
    {
        return right_col > 0;
        Physics2D.Raycast(transform.position, Vector2.right, 0.4f);
    }

    void Update()
    {
        if (IsGrand())
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        else
            rb.gravityScale = 1;

        float lrbias = Input.GetAxis("Horizontal");
        //if jump control should not allow, move this code to not jump block.
        if (lrbias > 0 && RightColl() && !IsGrand()
            || lrbias < 0 && LeftColl() && !IsGrand())
            ;
        else
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
            if (Input.GetButton("Jump") && rb.velocity.y == 0 && IsGrand())
            {
                rb.velocity = new Vector2(rb.velocity.x, 10);
            }
        }
    }
	
	void Animate () {
        sr.sprite = cur_anim[idx = (idx+1) % cur_anim.Length];
	}
}
