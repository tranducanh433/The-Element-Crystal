using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class MagicCircle : MonoBehaviour
{
    private float xRota;
    private float yRota;
    private float zRota;

    private float xRotaCurrent;
    private float yRotaCurrent;
    private float zRotaCurrent;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        Appear();
    }

    void Update()
    {
        xRotaCurrent += xRota * Time.deltaTime;
        yRotaCurrent += yRota * Time.deltaTime;
        zRotaCurrent += zRota * Time.deltaTime;

        transform.eulerAngles = new Vector3(xRotaCurrent, yRotaCurrent, zRotaCurrent);
    }

    public void Appear()
    {
        xRota = Random.Range(-90, 90);
        yRota = Random.Range(-90, 90);
        zRota = Random.Range(-90, 90);
        anim.SetBool("disappear", false);
    }

    public void Disappear()
    {
        anim.SetBool("disappear", true);
    }
}
