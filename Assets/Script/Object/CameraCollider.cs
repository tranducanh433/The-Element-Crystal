using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    public bool canExit;
    public bool activate1;

    private bool no;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && no == false)
        {
            transform.GetChild(0).gameObject.SetActive(true);

            if (activate1)
                no = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canExit == false)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
