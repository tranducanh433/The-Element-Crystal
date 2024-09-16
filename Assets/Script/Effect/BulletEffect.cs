using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    public Transform target;

    ParticleSystem myParticleSystem;
    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.ColorOverLifetimeModule overLifetimeModule;
    ParticleSystem.ColorOverLifetimeModule mainOverLifetimeModule;
    private void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
        emissionModule = myParticleSystem.emission;
    }
    void Update()
    {
        if(target != null)
        {
            transform.position = target.position;
        }
        else
        {
            emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(0);
            Destroy(gameObject, 1f);
        }
    }

    public void SetColor(Element element)
    {
        Gradient color = GameManager.instance.fireBall;
        if (element == Element.fire)
        {
            color = GameManager.instance.fireBall;
        }
        if(element == Element.water)
        {
            color = GameManager.instance.waterBall;
        }
        if(element == Element.plant)
        {
            color = GameManager.instance.plantBall;
        }
        if( element == Element.noneElement)
        {
            color = GameManager.instance.purpleBall;
        }

        overLifetimeModule = GetComponent<ParticleSystem>().colorOverLifetime;
        overLifetimeModule.color = color;
        mainOverLifetimeModule = target.GetComponent<ParticleSystem>().colorOverLifetime;
        mainOverLifetimeModule.color = color;

        if(target.GetComponent<Bullet>() != null)
            target.GetComponent<Bullet>().element = element;
        else
            target.GetComponent<EnemyBullet>().element = element;
    }
}
