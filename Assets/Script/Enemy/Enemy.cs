using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Video;

public enum ShootDerection
{
    up,
    down,
    left,
    right
}
public class Enemy : MonoBehaviour
{
    public int HP = 3;
    protected int currentHP;
    public float speed;
    public Element element;
    public EnemyData enemyData;
    public Transform groundDetection;
    public Material material;
    private Material currentMaterial;

    private Transform player;
    public bool movingRight;
    protected bool moving = true;
    protected Vector2 startPosition;
    public LayerMask whatIsGround;

    //attack
    public float timeBetweenAttack = 1f;
    public float timeStartToAttack = 1f;
    private float attackCD;

    [Header("Range Attack")]
    public Transform shootPos;
    public float bulletSpeed;
    public GameObject elementBallPrefab;
    public GameObject bulletEffectPrefab;
    public ShootDerection shootDerection = ShootDerection.left;
    public float bulletDestroyTime = 1;

    [Header("Moving Patrol")]
    public Transform[] points;
    private int pointNum;

    [Header ("Random Patrol")]
    public BoxCollider2D boxCollider2D;
    private Vector2 patrolPoint;

    protected GameManager GM;
    protected Animator anim;
    protected SpriteRenderer sr;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        currentHP = HP;
        moving = true;
        if (currentMaterial == null)
            currentMaterial = sr.material;
        else
            sr.material = currentMaterial;
    }

    //Start Function
    protected void StartFunction()
    {
        GM = GameManager.instance;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackCD = timeStartToAttack;
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        patrolPoint = transform.position;

        if (movingRight == true)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }
    //Take Damage
    public virtual void TakeDamage()
    {
        currentHP--;
        if(material != null)
        {
            StartCoroutine(TakeDamageEffectCo());
        }

        if(currentHP <= 0)
        {
            Death();
        }
    }

    protected virtual void Death()
    {
        gameObject.SetActive(false);
    }
    private IEnumerator TakeDamageEffectCo()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.material = material;
        yield return new WaitForSeconds(0.1f);
        sr.material = currentMaterial;
    }



    //Movement
    protected void Moving()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.5f, whatIsGround);
        if (moving)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (groundInfo.collider == false)
            {
                StartCoroutine(SwitchDerectionCO());
            }
        }
    }
    protected void PatrolMoving()
    {
        if (moving && points.Length >= 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, points[pointNum].position, speed * Time.deltaTime);
            PatrolFlip();

            if(Vector2.Distance(transform.position, points[pointNum].position) <= 0.1f)
            {
                pointNum++;

                if(pointNum >= points.Length)
                {
                    pointNum = 0;
                }
            }
        }
    }
    void PatrolFlip()
    {
        if(transform.position.x > points[pointNum].position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }

    public void RandomPatrolMoving()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPoint, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, patrolPoint) <= 0.1f)
        {
            ChoosePoint();
        }
    }
    void ChoosePoint()
    {
        Vector2 boxPosition = boxCollider2D.transform.position;
        Debug.Log(boxPosition);
        Vector2 maxPos = new Vector2(boxCollider2D.size.x / 2 + boxPosition.x, boxCollider2D.size.y / 2 + boxPosition.y);
        Vector2 minPos = new Vector2(boxPosition.x - boxCollider2D.size.x / 2, boxPosition.y - boxCollider2D.size.y / 2);

        patrolPoint = new Vector2(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y));
        RandomPatrolFlip();
    }
    void RandomPatrolFlip()
    {
        if (transform.position.x > patrolPoint.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }

    private IEnumerator SwitchDerectionCO()
    {
        moving = false;
        yield return new WaitForSeconds(1.5f);

        if(movingRight == false)
        {
            movingRight = true;
            moving = true;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            movingRight = false;
            moving = true;
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }

    //Attack System
    protected void Attack()
    {
        attackCD -= Time.deltaTime;

        if(attackCD <= 0)
        {
            RangeAttack();
            attackCD = timeBetweenAttack;
        }
    }
    protected void RangeAttack()
    {
        if (element == Element.fire)
        {
            GameObject bullet = Instantiate(elementBallPrefab, shootPos.position, Quaternion.identity);
            Destroy(bullet, bulletDestroyTime);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = DirectionToShoot() * bulletSpeed;

            GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
            BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
            bulletEffect.target = bullet.transform;
            bulletEffect.SetColor(Element.fire);
        }

        else if (element == Element.water)
        {
            GameObject bullet = Instantiate(elementBallPrefab, shootPos.position, Quaternion.identity);
            Destroy(bullet, bulletDestroyTime);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = DirectionToShoot() * bulletSpeed;

            GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
            BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
            bulletEffect.target = bullet.transform;
            bulletEffect.SetColor(Element.water);
        }

        else if (element == Element.plant)
        {
            GameObject bullet = Instantiate(elementBallPrefab, shootPos.position, Quaternion.identity);
            Destroy(bullet, bulletDestroyTime);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = DirectionToShoot() * bulletSpeed;

            GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
            BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
            bulletEffect.target = bullet.transform;
            bulletEffect.SetColor(Element.plant);
        }

        else if(element == Element.noneElement)
        {
            int r = Random.Range(0, 3);
            ShootRandom(r);
        }
    }
    void ShootRandom(int r)
    {
        if (r == 0)
        {
            GameObject bullet = Instantiate(elementBallPrefab, shootPos.position, Quaternion.identity);
            Destroy(bullet, bulletDestroyTime);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = DirectionToShoot() * bulletSpeed;

            GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
            BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
            bulletEffect.target = bullet.transform;
            bulletEffect.SetColor(Element.fire);
        }

        else if (r == 1)
        {
            GameObject bullet = Instantiate(elementBallPrefab, shootPos.position, Quaternion.identity);
            Destroy(bullet, bulletDestroyTime);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = DirectionToShoot() * bulletSpeed;

            GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
            BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
            bulletEffect.target = bullet.transform;
            bulletEffect.SetColor(Element.water);
        }

        else if (r == 2)
        {
            GameObject bullet = Instantiate(elementBallPrefab, shootPos.position, Quaternion.identity);
            Destroy(bullet, bulletDestroyTime);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = DirectionToShoot() * bulletSpeed;

            GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
            BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
            bulletEffect.target = bullet.transform;
            bulletEffect.SetColor(Element.plant);
        }
    }

    private Vector2 DirectionToShoot()
    {
        switch (shootDerection)
        {
            case ShootDerection.down:
                return Vector2.down;

            case ShootDerection.up:
                return Vector2.up;

            case ShootDerection.left:
                return Vector2.left;

            default:
                return Vector2.right;
        }
    }

    protected void MeleeAttack()
    {
        if(Vector2.Distance(transform.position, player.position) <= 0.5f)
        {
            player.GetComponent<Player>().TakeDamage();
        }
    }
}

