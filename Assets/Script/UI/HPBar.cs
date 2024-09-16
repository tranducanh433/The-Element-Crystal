using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public GameObject heartPrefab;

    public void SetValue(int value)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < value; i++)
        {
            Instantiate(heartPrefab, transform);
        }
    }
}
