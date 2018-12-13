using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public int scaffold_col = 0;
    public int left_col = 0;
    public int right_col = 0;

    public abstract void AddDamage(Actor from);
}
