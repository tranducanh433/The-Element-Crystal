using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignObject : MonoBehaviour
{
    [TextArea]
    public string textToShow;

    private GameManager GM;

    void Start()
    {
        GM = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            GM.SetTextEvent(textToShow, true);
    }
}
