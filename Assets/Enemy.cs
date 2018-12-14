using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public Sprite[] idle, walk, jump, attack, hurt, jattack;
    public Bullet pfBullet;
    SpriteRenderer sr;
    Sprite[] cur_anim;
    int idx = 0;

	protected override void Start () {
        base.Start();
        sr = GetComponentInChildren<SpriteRenderer>();
        cur_anim = idle;
        InvokeRepeating("Animate", 0, 0.1f);
	}

    float prev_keydown_time, cur_keydown_time = -1;
    bool pright = true, cright = true;
    bool IsRunning()
    {
        bool rdown = Input.GetKeyDown(KeyCode.RightArrow);
        bool ldown = Input.GetKeyDown(KeyCode.LeftArrow);
        if (rdown || ldown)
        {
            prev_keydown_time = cur_keydown_time;
            cur_keydown_time = Time.time;
            pright = cright;
            cright = rdown;
        }
        return cur_keydown_time - prev_keydown_time < 0.2f && pright == cright;
    }

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

    float move_speed = 1;

    bool attacking = false;
    void AttackingRelease() { attacking = false;move_speed = 1; }
    bool hurting = false;
    void HurtingRelease() { hurting = false; }
    bool jattacking = false;
    void JAttackingRelease() { jattacking = false; }

    protected override void Update()
    {
        base.Update();
        if (hurting || jattacking)
            return;

        //float lrbias = Input.GetAxis("Horizontal");
        ////if jump control should not allow, move this code to not jump block.
        if (!IsGrand())
            ;
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
        

        //jumping
        if (!IsGrand())
        {
            rb.gravityScale = 1;

            SetAnim(jump);
            if (rb.velocity.y > 0)
                idx = 0;
            else
                idx = 1;

            //if (lrbias > 0)
            //    sr.flipX = false;
            //else if (lrbias < 0)
            //    sr.flipX = true;
        }
        //not jump
        else
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);

            //if (lrbias > 0)
            //{
            //    SetAnim(walk);
            //    sr.flipX = false;
            //}
            //else if (lrbias < 0)
            //{
            //    SetAnim(walk);
            //    sr.flipX = true;
            //}
            //else
                SetAnim(idle);

            //if (Input.GetButton("Jump") && rb.velocity.y == 0 && IsGrand())
            //{
            //    rb.velocity = new Vector2(rb.velocity.x, 10);
            //}
        }

        //if (Input.GetKeyDown(KeyCode.LeftControl) && !attacking)
        //{
        //    if (Mathf.Abs(rb.velocity.x) < 3f)
        //    {
        //        SetAnim(attack);
        //        attacking = true;
        //        Invoke("AttackingRelease", 0.5f);
        //        move_speed = 0.5f;
        //        Bullet b = Instantiate(pfBullet);
        //        b.GetComponent<Rigidbody2D>().velocity = new Vector2(sr.flipX ? -3 : 3, 0);
        //        b.from = this;
        //        b.lifetime = 0.5f;
        //        b.target_component = "Enemy";
        //        b.transform.position = transform.position + new Vector3(sr.flipX ? -0.4f : 0.4f, 0.7f, 0);
        //    }
        //    else
        //    {
        //        SetAnim(jattack);
        //        jattacking = true;
        //        Invoke("JAttackingRelease", 0.6f);
        //        rb.gravityScale = 0.8f;
        //        rb.velocity = new Vector2(6 * (sr.flipX ? -1 : 1), 3/*rb.velocity.y*/);
        //    }
        //}
    }
	
	void Animate () {
        sr.sprite = cur_anim[idx = (idx+1) % cur_anim.Length];
	}

    void SetAnim(Sprite[] anim)
    {
        if (attacking)
        {
            return;
        }
        if (cur_anim == anim)
            return;
        cur_anim = anim;
        idx = 0;
    }

    public override void AddDamage(Actor from)
    {
        sr.flipX = from.transform.position.x < transform.position.x;

        SetAnim(hurt);
        hurting = true;
        Invoke("HurtingRelease", 0.2f);
        rb.gravityScale = 1;
        rb.velocity = new Vector2(4 * (sr.flipX ? 1 : -1), 2);
    }
}
