using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public Sprite[] idle, walk, jump, attack, hurt, jattack;
    public Bullet pfBullet;

	protected override void Start () {
        base.Start();
        cur_anim = idle;
    }
    
    bool hurting = false;
    void HurtingRelease() { hurting = false; }
    bool jattacking = false;
    void JAttackingRelease() { jattacking = false; }

    protected override void Update()
    {
        base.Update();
        if (hurting || jattacking)
            return;

        ////if jump control should not allow, move this code to not jump block.
        if (IsGrand())
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
            
        }
        //not jump
        else
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);

            SetAnim(idle);
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
