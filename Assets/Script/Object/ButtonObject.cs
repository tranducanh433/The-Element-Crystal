using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    public GameObject objectToDestroy;
    public Sprite activateImage;
    public Sprite inactivateImage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
            GetComponent<SpriteRenderer>().sprite = activateImage;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().sprite = inactivateImage;

        }
    }
}
