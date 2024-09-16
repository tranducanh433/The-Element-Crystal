using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCrystal : MonoBehaviour
{
    public Element element;
    private GameManager GM;

    private void Start()
    {
        GM = GameManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            switch (element)
            {
                case Element.fire:
                    if(GM.unlockFire == false)
                    GM.unlockFireBall();
                    break;

                case Element.water:
                    if (GM.unlockWater == false)
                        GM.unlockWaterBall();
                    break;

                case Element.plant:
                    if (GM.unlockNatural == false)
                        GM.unlockNaturalBall();
                    break;
            }
        }
        
    }
}
