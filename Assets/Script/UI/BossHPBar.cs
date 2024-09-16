using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Slider slider;

    public void SetValue(int value)
    {
        slider.value = value;
    }
}
