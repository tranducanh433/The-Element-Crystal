using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float bulletSpeed;
    public Transform shootPos;

    private bool isRight = true;

    private float moveInput;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private GameManager GM;

    private bool isGrounded;
    private bool isFallingStoneFalling;
    private bool isGrounded2;
    private bool isFallingStoneFalling2;
    public Transform groundCheck;
    public Transform checkUp;
    public float checkRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsStone;

    [Header("Attack")]
    public GameObject elementBallPrefab;
    public GameObject bulletEffectPrefab;

    private float attackCD;

    [Header("HP")]
    public int HP;
    public HPBar hpBar;

    private bool canJump = true;
    private bool checkGround;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        GM = GameManager.instance;

        hpBar = GameObject.Find("Heart Container").GetComponent<HPBar>();

        HP = GM.PlayerHP;
        hpBar.SetValue(HP);
    }


    void Update()
    {
        Attack();
        Flip();
        Jump();
        GoDown();
        DeathBecauseOfStone();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isFallingStoneFalling = Physics2D.OverlapCircle(checkUp.position, checkRadius, whatIsStone);
        isGrounded2 = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsStone);
        isFallingStoneFalling2 = Physics2D.OverlapCircle(checkUp.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        Animation();
    }

    void Animation()
    {
        if(moveInput != 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }

        if (isRight)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
    }

    void Flip()
    {
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            isRight = true;
        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isRight = false;
        }
    }

    void Jump()
    {

        if (isGrounded == true && canJump == false && checkGround == true)
        {
            canJump = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && canJump == true)
        {
            rb.velocity = Vector2.up * jumpForce;
            canJump = false;
            StartCoroutine(CheckGroundCO());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity += Vector2.down * 10f;
        }
    }

    private IEnumerator CheckGroundCO()
    {
        checkGround = false;
        yield return new WaitForSeconds(0.05f);
        checkGround = true;
    }

    void GoDown()
    {
        if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && isGrounded == true)
        {
            StartCoroutine(GoDownCo());
        }
    }

    private IEnumerator GoDownCo()
    {
        gameObject.layer = 13;
        yield return new WaitForSeconds(0.25f);
        gameObject.layer = 10;

    }

    void Attack()
    {
        attackCD -= Time.deltaTime;

        if (attackCD <= 0)
        {
            if ((Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.Z))&& GM.unlockFire == true)
            {
                GameObject bullet = Instantiate(elementBallPrefab, shootPos.position, Quaternion.identity);
                Destroy(bullet, 2f);
                Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                bulletRB.velocity = ShootDirec() * bulletSpeed;

                GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
                BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
                bulletEffect.target = bullet.transform;
                bulletEffect.SetColor(Element.fire);

                attackCD = 0.25f;
            }

            else if ((Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.X) ) && GM.unlockWater == true)
            {
                GameObject bullet = Instantiate(elementBallPrefab, shootPos.position, Quaternion.identity);
                Destroy(bullet, 2f);
                Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                bulletRB.velocity = ShootDirec() * bulletSpeed;

                GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
                BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
                bulletEffect.target = bullet.transform;
                bulletEffect.SetColor(Element.water);

                attackCD = 0.25f;
            }

            else if ((Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.C)) && GM.unlockNatural == true)
            {
                GameObject bullet = Instantiate(elementBallPrefab, shootPos.position, Quaternion.identity);
                Destroy(bullet, 2f);
                Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                bulletRB.velocity = ShootDirec() * bulletSpeed;

                GameObject effect = Instantiate(bulletEffectPrefab, bullet.transform.position, Quaternion.Euler(0, 0, 90));
                BulletEffect bulletEffect = effect.GetComponent<BulletEffect>();
                bulletEffect.target = bullet.transform;
                bulletEffect.SetColor(Element.plant);

                attackCD = 0.25f;
            }
        }
    }

    private Vector2 ShootDirec()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            return Vector2.up;
        }
        else if (isRight)
        {
            return Vector2.right;
        }
        else
        {
            return Vector2.left;
        }
    }

    public void TakeDamage()
    {
        if(gameObject.layer != 12)
        {
            HP--;
            hpBar.SetValue(HP);

            if(HP <= 0)
            {
                Death();
            }
            else
                StartCoroutine(ImmuneCo());
        }
    }
    public void ResetHP()
    {
        HP = 3;
        hpBar.SetValue(HP);

        sr.color = new Color(1, 1, 1, 1);
        gameObject.layer = 10;
    }
    void Death()
    {
        GM.PlayerIsDeath(gameObject);
    }

    private IEnumerator ImmuneCo()
    {
        gameObject.layer = 12;
        for (int i = 0; i < 10; i++)
        {
            if(sr.color.a == 0.3f)
                sr.color = new Color(1, 1, 1, 1);
            else
                sr.color = new Color(1, 1, 1, 0.3f);

            yield return new WaitForSeconds(0.1f);
        }
        gameObject.layer = 10;
    }

    void DeathBecauseOfStone()
    {
        if(isFallingStoneFalling == true && isGrounded == true)
        {
            Death();
            gameObject.SetActive(false);
        }
        if (isFallingStoneFalling2 == true && isGrounded2 == true)
        {
            Death();
            gameObject.SetActive(false);
        }
    }
}
