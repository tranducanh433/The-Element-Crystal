using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    void Start()
    {
        StartFunction();   
    }

    void Update()
    {
        Moving();
        MeleeAttack();
        SlimeAnimation();
    }

    void SlimeAnimation()
    {
        anim.SetBool("move", moving);
    }
}
