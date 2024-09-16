using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    public FallingStoneController fallingStoneController;

    public GameObject[] enemyToSummon;
    public Transform[] summonPoint;
    public GameObject[] doors;
    public BoxCollider2D boxCollider2D;

    private bool isStepOn;
    private bool complete;
    private bool summonDone;

    private void Update()
    {
        if (isStepOn == true && IsAllDestroy() && fallingStoneController == null && summonDone == true)
        {
            isStepOn = false;
            complete = true;
            doors[0].SetActive(false);
            doors[1].SetActive(false);
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isStepOn == false && complete == false)
        {
            if(fallingStoneController != null)
            {
                fallingStoneController.Activate();
                isStepOn = true;
            }
            else
            {
                StartCoroutine(SummonCO());
                isStepOn = true;
                doors[0].SetActive(true);
                doors[1].SetActive(true);
            }
        }
    }

    public void ResetCheckPoint()
    {
        isStepOn = false;
        complete = false;
        summonDone = false;

        doors[0].SetActive(false);
        doors[1].SetActive(false);

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public bool IsAllDestroy()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf == true)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator SummonCO()
    {
        for (int i = 0; i < enemyToSummon.Length; i++)
        {
            GameObject enemy = Instantiate(enemyToSummon[i], summonPoint[i].position, Quaternion.identity, transform);
            if(boxCollider2D != null)
            {
                enemy.GetComponent<Enemy>().boxCollider2D = boxCollider2D;
            }
            yield return new WaitForSeconds(0.25f);
        }
        summonDone = true;
    }
}
