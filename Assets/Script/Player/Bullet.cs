using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    fire,
    water,
    plant,
    noneElement
}

public class Bullet : MonoBehaviour
{
    public Element element;
    public GameObject exploxePrefab;

    ParticleSystem.ColorOverLifetimeModule overLifetimeModule;
    ParticleSystem.ColorOverLifetimeModule mainOverLifetimeModule;

    private void OnTriggerEnter2D(Collider2D other)
    {
        FireElement(other);
        WaterElement(other);
        PlantElement(other);

        if (other.CompareTag("ground"))
        {
            DestroyEffect();
        }
    }
    
    void FireElement(Collider2D other)
    {
        if(element == Element.fire)
        {
            if (other.CompareTag("enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();

                if(enemy.element == Element.plant)
                {
                    enemy.TakeDamage();
                }
                DestroyEffect();
            }
            if (other.CompareTag("Enemy Bullet"))
            {
                EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();

                if (enemyBullet.element == Element.plant)
                {
                    enemyBullet.TakeDamage();
                }
                DestroyEffect();
            }
        }
    }
    void WaterElement(Collider2D other)
    {
        if (element == Element.water)
        {
            if (other.CompareTag("enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();

                if (enemy.element == Element.fire)
                {
                    enemy.TakeDamage();
                }
                DestroyEffect();
            }
            if (other.CompareTag("Enemy Bullet"))
            {
                EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();

                if (enemyBullet.element == Element.fire)
                {
                    enemyBullet.TakeDamage();
                }
                DestroyEffect();
            }
        }
    }
    void PlantElement(Collider2D other)
    {
        if (element == Element.plant)
        {
            if (other.CompareTag("enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();

                if (enemy.element == Element.water)
                {
                    enemy.TakeDamage();
                }
                DestroyEffect();
            }
            if (other.CompareTag("Enemy Bullet"))
            {
                EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();

                if (enemyBullet.element == Element.water)
                {
                    enemyBullet.TakeDamage();
                }
                DestroyEffect();
            }
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
