using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public Sprite[] idle, walk, jump, attack, hurt, jattack;
    public Bullet pfBullet;
    

    protected override void Start () {
        base.Start();
        cur_anim = idle;
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

    float move_speed = 1;

    bool attacking = false;
    void AttackingRelease() { anim_freeze = attacking = false;move_speed = 1; }
    bool hurting = false;
    void HurtingRelease() { hurting = false; }
    bool jattacking = false;
    void JAttackingRelease() { jattacking = false;rb.drag = 0; }

    protected override void Update()
    {
        base.Update();
        if (hurting)
            return;
        if(jattacking)
            return;

        float lrbias = Input.GetAxis("Horizontal");
        //if jump control should not allow, move this code to not jump block.
        if (!(lrbias > 0 && RightColl() && !IsGrand() || lrbias < 0 && LeftColl() && !IsGrand()))
            rb.velocity = new Vector2(lrbias * 2 * move_speed * (IsRunning() ? 2 : 1), rb.velocity.y);

        //jumping
        if (!IsGrand())
        {
            rb.gravityScale = 1;

            SetAnim(jump);
            if (rb.velocity.y > 0)
                idx = 0;
            else
                idx = 1;

            if (lrbias > 0)
                sr.flipX = false;
            else if (lrbias < 0)
                sr.flipX = true;
        }
        //not jump
        else
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);

            if (lrbias > 0)
            {
                SetAnim(walk);
                sr.flipX = false;
            }
            else if (lrbias < 0)
            {
                SetAnim(walk);
                sr.flipX = true;
            }
            else
                SetAnim(idle);

            if (Input.GetButton("Jump") && rb.velocity.y == 0 && IsGrand())
            {
                rb.velocity = new Vector2(rb.velocity.x, 10);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !attacking)
        {
            if (Mathf.Abs(rb.velocity.x) < 3f)
            {
                SetAnim(attack);
                anim_freeze = attacking = true;
                Invoke("AttackingRelease", 0.5f);
                move_speed = 0.5f;
                Bullet b = Instantiate(pfBullet);
                b.GetComponent<Rigidbody2D>().velocity = new Vector2(sr.flipX ? -3 : 3, 0);
                b.from = this;
                b.lifetime = 0.5f;
                b.target_component = "Enemy";
                b.transform.position = transform.position + new Vector3(sr.flipX ? -0.4f : 0.4f, 0.7f, 0);
            }
            else
            {
                SetAnim(jattack);
                jattacking = true;
                Invoke("JAttackingRelease", 0.6f);
                rb.gravityScale = 0.8f;
                rb.velocity = new Vector2(6 * (sr.flipX ? -1 : 1), 3/*rb.velocity.y*/);
                rb.drag = 1;
            }
        }
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
