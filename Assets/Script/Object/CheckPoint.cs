using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isFirstTime = true;
    public bool isDefault;
    public GameObject objectToSetActive;
    private GameManager GM;

    void Start()
    {
        GM = GameManager.instance;

        if (isDefault)
        {
            GM.checkPoint = this;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void ResetEnemy()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        if(GetComponent<Challenge>() != null)
        {
            GetComponent<Challenge>().ResetCheckPoint();
        }
    }
    public void CompleteCheckpoint()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFirstTime)
        {
            if (other.CompareTag("Player"))
            {
                GM.SetCheckPoint(this);
                if(objectToSetActive != null)
                    objectToSetActive.SetActive(true);
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }

                isFirstTime = false;
            }
        }
    }
}
