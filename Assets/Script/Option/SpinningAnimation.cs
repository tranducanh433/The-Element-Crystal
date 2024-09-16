using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningAnimation : MonoBehaviour
{
    public float speed;

    private float angle;

    void Update()
    {
        angle += speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, angle);   
    }
}
