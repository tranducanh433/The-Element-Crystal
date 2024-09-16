using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Element element;
    public int HP = 1;
    public GameObject exploxePrefab;

    ParticleSystem.ColorOverLifetimeModule overLifetimeModule;
    ParticleSystem.ColorOverLifetimeModule mainOverLifetimeModule;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage();
            DestroyEffect();
        }
        if (other.CompareTag("ground"))
        {
            DestroyEffect();
        }
    }

    public void TakeDamage()
    {
        HP--;

        if(HP <= 0)
        {
            DestroyEffect();
        }
    }

    public void DestroyEffect()
    {
        GameObject effect = Instantiate(exploxePrefab, transform.position, Quaternion.identity);
        overLifetimeModule = effect.GetComponent<ParticleSystem>().colorOverLifetime;
        mainOverLifetimeModule = GetComponent<ParticleSystem>().colorOverLifetime;

        overLifetimeModule.color = mainOverLifetimeModule.color;
        Destroy(gameObject);
    }
}
