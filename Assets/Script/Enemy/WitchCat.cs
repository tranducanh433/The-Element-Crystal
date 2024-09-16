using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCat : Enemy
{
    void Start()
    {
        StartFunction();
    }

    void Update()
    {
        Attack();
        PatrolMoving();

        if(boxCollider2D != null)
        {
            RandomPatrolMoving();
        }
    }
}
