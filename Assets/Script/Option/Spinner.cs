using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float speed = 90;
    private float currentEuler;

    void Update()
    {
        currentEuler += speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, currentEuler);
    }
}
