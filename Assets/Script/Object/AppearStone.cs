using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearStone : MonoBehaviour
{
    public Animator[] stones;
    public bool active1;
    public bool active2;
    private float cd;

    void Update()
    {
        cd -= Time.deltaTime;

        if (cd < 0)
        {
            if (active1 == false && active2 == false)
                active1 = true;
            else if (active1 == true && active2 == false)
                active2 = true;
            else if (active1 == true && active2 == true)
                active1 = false;
            else
                active2 = false;


            UpdateAnim();
            cd = 2;
        }

    }

    void UpdateAnim()
    {
        stones[0].SetBool("appear", active1);
        stones[1].SetBool("appear", active2);
    }
}
