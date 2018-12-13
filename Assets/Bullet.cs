using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Actor from;
    public string target_component;
    public float lifetime;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if ((lifetime -= Time.deltaTime) < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Actor target = (Actor)collision.GetComponent(target_component);
        if (target)
            target.AddDamage();
    }
}
