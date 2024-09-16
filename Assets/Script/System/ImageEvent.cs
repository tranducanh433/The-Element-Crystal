using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageEvent : MonoBehaviour
{
    public Image imageItem;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI downText;

    public Sprite fire;
    public Sprite water;
    public Sprite natural;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetImageEvent(Element element)
    {
        gameObject.SetActive(true);

        if(element == Element.fire)
        {
            nameText.text = "<color=red>Fire Element";
            downText.text = "Fire can kill natural enemy";
            imageItem.sprite = fire;
        }
        if(element == Element.water)
        {
            nameText.text = "<color=blue>Water Element";
            downText.text = "water can kill fire enemy";
            imageItem.sprite = water;
        }
        if(element == Element.plant)
        {
            nameText.text = "<color=green>Natural Element";
            downText.text = "natural can kill water enemy";
            imageItem.sprite = natural;
        }
    }
}
