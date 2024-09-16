using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEvent : MonoBehaviour
{
    public bool isUp;
    private string textNeedToShow;

    private bool isPrintAll;
    private float timeToDisappear = 1;
    private TextMeshProUGUI text;
    private float a = 1;

    void Update()
    {
        if(isPrintAll == true)
        {
            timeToDisappear -= Time.deltaTime;

            if(timeToDisappear <= 0)
            {
                a -= 2 * Time.deltaTime;
                text.color = new Color(1, 1, 1, a);

                if(a <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }



    public void SetTextEvent(string textValue)
    {
        StopAllCoroutines();
        text = GetComponent<TextMeshProUGUI>();
        textNeedToShow = textValue;
        timeToDisappear = 2;
        isPrintAll = false;
        a = 1;
        text.text = "";
        text.color = new Color(1, 1, 1, 1);
        gameObject.SetActive(true);

        if (isUp)
            text.text = textNeedToShow;
        else
            StartCoroutine(ShowTextCo());
    }

    private IEnumerator ShowTextCo()
    {
        foreach(char alpha in textNeedToShow)
        {
            text.text += alpha;
            yield return null;
        }

        isPrintAll = true;
    }
}
