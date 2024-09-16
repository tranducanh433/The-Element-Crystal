using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStone : MonoBehaviour
{
    public float speed;
    public float startAtkSpeed;
    public float atkSpeed;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    private float cd;
    private bool isGround;
    private bool moveDown;
    private Vector2 defaultPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultPosition = transform.position;
        cd = startAtkSpeed;
    }

    void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.01f, whatIsGround);

        cd -= Time.deltaTime;
        if (cd <= 0)
        {
            moveDown = true;
        }
        if (isGround == true)
        {
            cd = atkSpeed;
            moveDown = false;
        }

        if (moveDown)
            rb.gravityScale = speed;
        else
            rb.gravityScale = 0;
            transform.position = Vector2.MoveTowards(transform.position, defaultPosition, speed / 5 * Time.deltaTime);
    }
}
