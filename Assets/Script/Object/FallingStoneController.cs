using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FallingStoneController : MonoBehaviour
{
    public GameObject[] stones;
    private float posx = 2;
    private float cd = 0.75f;

    public void Activate()
    {
        StopAllCoroutines();
        cd = 0.75f;
        posx = transform.position.x + 2;

        for (int i = 0; i < stones.Length; i++)
        {
            Rigidbody2D rb = stones[i].GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;

            stones[i].transform.position = new Vector2(posx, transform.position.y);
            posx += 3;
        }

        posx = transform.position.x + 2;

        StartCoroutine(IDontKnowHowToNameThisCo());
    }

    private IEnumerator IDontKnowHowToNameThisCo()
    {
        for (int i = 0; i < stones.Length; i++)
        {
            Rigidbody2D rb = stones[i].GetComponent<Rigidbody2D>();

            rb.gravityScale = 20;
            yield return new WaitForSeconds(cd);
        }
    }
}
