using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Rendering;

public enum Skill
{
    none,
    rainingElement,
    spinningElement,
    railGun,
    elementWall,
    twoSide
}
public class Boss : Enemy
{
    [Header("Static")]
    public Skill currentSkill;

    [Header("HP Bar")]
    public BossHPBar hpBar;

    [Header("Position")]
    public Transform left1;
    public Transform center1;
    public Transform right1;

    [Header("Sprite")]
    public Sprite defaultSprite;
    public Sprite redIdle;
    public Sprite redAttack;
    public Sprite redAttack2;
    public Sprite blueIdle;
    public Sprite blueAttack;
    public Sprite blueAttack2;
    public Sprite greenIdle;
    public Sprite greenAttack1;
    public Sprite greenAttack2;
    public Sprite tdIdle;
    public Sprite tdAttack;
    public Sprite tdAttack2;

    [Header("Component")]
    public MagicCircle magicCircle;

    [Header("Spinning Element")]
    public Transform[] spinningShootPoint;

    [Header("Railgun")]
    public Transform shootRotate;
    public Transform shootDirection;
    private float euler;
    private int railgunStep;

    [Header("Other")]
    private Vector3 positionToTeleport;
    private float chooseCd = 1;
    private bool isTranform;
    private bool isAppear;
    private float alpha = 1;
    private Skill choosingSkill;
    private Element choosingElement;
    private float attackingCD;
    private float attackingCD2;
    private float changeElementTime;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        material = sr.material;
        sr.sprite = defaultSprite;
        currentHP = HP;
        hpBar.SetValue(currentHP);
        transform.position = right1.position;

        currentSkill = Skill.none;
        chooseCd = 1;

        alpha = 1;
        material.SetFloat("_Fade", 1);
        isAppear = false;
        isTranform = false;
    }

    private void Start()
    {
        GM = GameManager.instance;
        sr = GetComponent<SpriteRenderer>();
        material = sr.material;
        currentHP = HP;
    }

    private void Update()
    {
        if (chooseCd <= 0)
        {
            ChooseSkill();
        }
        else
        {
            chooseCd -= Time.deltaTime;
        }

        FadedEffect();
        //skill
        RainingElement();
        SpiningElement();
        RailGun();
        ElementWall();
        TwoSide();
    }

    void FadedEffect()
    {
        if (isTranform)
        {
            if (alpha != 0 && isAppear == false)
            {
                alpha -= Time.deltaTime;
                material.SetFloat("_Fade", alpha);

                if (alpha <= 0.5f)
                {
                    magicCircle.Disappear();
                }
                if (alpha <= 0)
                {
                    isAppear = true;
                    transform.position = positionToTeleport;
                    magicCircle.Appear();
                    element = choosingElement;
                    SwitchSprite();
                    GM.SetSceneColor(element);
                }
            }
            else if (isAppear == true)
            {
                alpha += 2 * Time.deltaTime;
                material.SetFloat("_Fade", alpha);

                if (alpha >= 1)
                {
                    StartCoroutine(ReadyToAttackCo());
                    isAppear = false;
                    isTranform = false;
                }
            }
        }

    }

    void ChooseSkill()
    {
        alpha = 1;
        isTranform = true;
        currentSkill = Skill.none;
        choosingElement = randomElement();

        int r = Random.Range(1, 5);

        if (currentHP <= 30)
        {
            chooseCd = 99999999999;
            choosingSkill = Skill.twoSide;
            positionToTeleport = center1.position;
        }
        else if (r == 1)
        {
            choosingSkill = Skill.rainingElement;
            positionToTeleport = center1.position;
            chooseCd = 10;
        }
        else if (r == 2)
        {
            chooseCd = 5;
            choosingSkill = Skill.spinningElement;
            int r2 = Random.Range(0, 2);
            if (r2 == 0)
            {
                positionToTeleport = right1.position;
            }
            else
            {
                positionToTeleport = left1.position;
            }
        }
        else if (r == 3)
        {
            chooseCd = 9999;
            choosingSkill = Skill.railGun;
            positionToTeleport = center1.position;
        }
        else if (r == 4)
        {
            chooseCd = 15;
            choosingSkill = Skill.elementWall;

            int r2 = Random.Range(0, 2);
            if (r2 == 0)
            {
                positionToTeleport = right1.position;
            }
            else
            {
                positionToTeleport = left1.position;
            }
        }
    }


    //Skill
    void RainingElement()
    {
        if (currentSkill == Skill.rainingElement)
        {
            attackingCD -= Time.deltaTime;

            if (attackingCD <= 0)
            {
                Vector2 pos = new Vector2(Random.Range(transform.position.x - 10, transform.position.x + 10), transform.position.y + 1.5f);

                SummonBulletWithVector(pos, randomElement(), Vector2.down);

                attackingCD = 0.125f;
            }
        }
    }
    void SpiningElement()
    {
        if (currentSkill == Skill.spinningElement)
        {

            attackingCD -= Time.deltaTime;

            if (attackingCD <= 0)
            {
                for (int i = 0; i < spinningShootPoint.Length; i++)
                {
                    Vector2 direction = spinningShootPoint[i].position - transform.position;
                    direction.Normalize();

                    if (i == 0)
                        SummonBulletWithVector(transform.position, Element.fire, direction);
                    if (i == 1)
                        SummonBulletWithVector(transform.position, Element.water, direction);
                    if (i == 2)
                        SummonBulletWithVector(transform.position, Element.plant, direction);
                }

                attackingCD = 0.075f;
            }
        }
    }

    void RailGun()
    {
        if (currentSkill == Skill.railGun)
        {
            if (attackingCD <= 0)
            {
                if (railgunStep == 0)
                {
                    shootRotate.eulerAngles = new Vector3(0, 0, euler);
                    Vector2 direction = shootDirection.position - transform.position;
                    direction.Normalize();

                    SummonBullet(Element.noneElement, direction, bulletSpeed * 2);
                    euler += 1f;
                    if (euler >= 140)
                    {
                        railgunStep = 1;
                        euler = 180;
                    }
                }
                if (railgunStep == 1)
                {
                    shootRotate.eulerAngles = new Vector3(0, 0, euler);
                    Vector2 direction = shootDirection.position - transform.position;
                    direction.Normalize();

                    SummonBullet(Element.noneElement, direction, bulletSpeed * 2);
                    euler -= 1f;
                    if (euler <= 40)
                    {
                        railgunStep = 0;
                        currentSkill = Skill.none;
                        chooseCd = 0;
                    }
                }
            }
            else
            {
                attackingCD -= Time.deltaTime;
            }
        }
    }
    void ElementWall()
    {
        if (currentSkill == Skill.elementWall)
        {
            if (attackingCD <= 0)
            {
                if (transform.position == right1.position)
                {
                    int r = Random.Range(0, 2);

                    if (r == 0)
                    {
                        float add = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 pointup = new Vector2(right1.position.x + 2.5f, right1.position.y + add);

                            SummonBulletWithVector(pointup, Element.noneElement, Vector2.left);
                            add -= 0.5f;
                        }
                    }
                    else
                    {
                        float add = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 pointdown = new Vector2(right1.position.x + 2.5f, right1.position.y - 2.5f + add);

                            SummonBulletWithVector(pointdown, Element.noneElement, Vector2.left);
                            add -= 0.5f;
                        }
                    }
                }
                else if (transform.position == left1.position)
                {
                    int r = Random.Range(0, 2);

                    if (r == 0)
                    {
                        float add = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 pointup = new Vector2(left1.position.x - 2.5f, right1.position.y + add);

                            SummonBulletWithVector(pointup, Element.noneElement, Vector2.right);
                            add -= 0.5f;
                        }
                    }
                    else
                    {
                        float add = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 pointdown = new Vector2(left1.position.x - 2.5f, right1.position.y - 2.5f + add);

                            SummonBulletWithVector(pointdown, Element.noneElement, Vector2.right);
                            add -= 0.5f;
                        }
                    }
                }

                attackingCD = 1f;
            }
            else
            {
                attackingCD -= Time.deltaTime;
            }

        }
    }
    void TwoSide()
    {
        if (currentSkill == Skill.twoSide)
        {
            if (attackingCD <= 0)
            {
                float r1 = Random.Range(-4.5f, 1f);
                float r2 = Random.Range(-4.4f, 1f);

                Vector2 rightSide = new Vector2(right1.position.x + 2.5f, right1.position.y + r1);
                Vector2 leftSide = new Vector2(left1.position.x - 2.5f, right1.position.y + r2);


                SummonBulletWithVector(rightSide, randomElement(), Vector2.left, 3f);
                SummonBulletWithVector(leftSide, randomElement(), Vector2.right, 3f);

                attackingCD = 1f;
            }
            else
            {
                attackingCD -= Time.deltaTime;
            }

            //Low HP
            //Change Element
            if (changeElementTime <= 0)
            {
                choosingElement = randomElement();
                isTranform = true;

                changeElementTime = 10;
            }
            else
            {
                changeElementTime -= Time.deltaTime;
            }

            //Shoot Bullet
            if (attackingCD2 <= 0)
            {
                Vector2 direction = shootDirection.position - transform.position;
                direction.Normalize();

                SummonBullet(element, direction, bulletSpeed - 2);
                attackingCD2 = 0.25f;
            }
            else
            {
                attackingCD2 -= Time.deltaTime;
            }

            //rotate
            euler += 2f;
            shootRotate.eulerAngles = new Vector3(0, 0, euler);

        }
    }

    private Element randomElement()
    {
        int r = Random.Range(0, 3);

        switch (r)
        {
            case 0:
                return Element.fire;

            case 1:
                return Element.water;

            default:
                return Element.plant;
        }
    }

    private IEnumerator ReadyToAttackCo()
    {
        yield return new WaitForSeconds(1.5f);
        currentSkill = choosingSkill;
    }

    void SummonBullet(Element element, Vector2 direction, float speed = 0)
    {
        if (speed == 0)
            speed = this.speed;

        GameObject bullet = Instantiate(elementBallPrefab, transform.position, Quaternion.identity);
        Destroy(bullet, bulletDestroyTime);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.velocity = direction * speed;

        GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
        BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
        bulletEffect.target = bullet.transform;

        bulletEffect.SetColor(element);
    }
    void SummonBulletWithVector(Vector3 position, Element element, Vector2 direction, float speed = 0)
    {
        if (speed == 0)
            speed = bulletSpeed;

        GameObject bullet = Instantiate(elementBallPrefab, position, Quaternion.identity);
        Destroy(bullet, bulletDestroyTime);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.velocity = direction * speed;

        GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
        BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
        bulletEffect.target = bullet.transform;

        bulletEffect.SetColor(element);
    }

    void SwitchSprite()
    {
        if (choosingSkill == Skill.rainingElement || choosingSkill == Skill.railGun || choosingSkill == Skill.twoSide)
        {
            if (element == Element.fire)
                sr.sprite = redAttack2;
            if (element == Element.plant)
                sr.sprite = greenAttack2;
            if (element == Element.water)
                sr.sprite = blueAttack2;
        }
        if (choosingSkill == Skill.elementWall)
        {
            if (element == Element.fire)
                sr.sprite = redAttack;
            if (element == Element.plant)
                sr.sprite = greenAttack1;
            if (element == Element.water)
                sr.sprite = blueAttack;
        }
        if (choosingSkill == Skill.spinningElement)
        {
            if (element == Element.fire)
                sr.sprite = redIdle;
            if (element == Element.plant)
                sr.sprite = greenIdle;
            if (element == Element.water)
                sr.sprite = blueIdle;
        }

        if (positionToTeleport == left1.position)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    public override void TakeDamage()
    {
        currentHP--;
        hpBar.SetValue(currentHP);
        StartCoroutine(TakeDamageEffectCo());

        if (currentHP <= 0)
        {
            Death();
        }
    }

    protected override void Death()
    {
        base.Death();

        GM.WinTheGame();
    }

    private IEnumerator TakeDamageEffectCo()
    {
        if (sr.sprite == blueAttack2 || sr.sprite == greenAttack2 || sr.sprite == redAttack2)
        {
            sr.sprite = tdAttack2;
        }
        if (sr.sprite == blueAttack || sr.sprite == greenAttack1 || sr.sprite == redAttack)
        {
            sr.sprite = tdAttack;
        }
        if (sr.sprite == blueIdle || sr.sprite == greenIdle || sr.sprite == redIdle)
        {
            sr.sprite = tdIdle;
        }

        yield return new WaitForSeconds(0.1f);
        SwitchSprite();
    }
}
