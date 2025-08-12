using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyJump : MonoBehaviour
{
    private readonly int hashJump = Animator.StringToHash("Jump");
    private readonly string groundTag = "Ground";
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float Height => spriteRenderer.bounds.size.y;
    private float jumpPower;
    private float jumpTime;
    private bool isJump = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = -Physics2D.gravity.y * 0.5f;
    }
    private void Update()
    {
        jumpTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!isJump && jumpTime > 2f)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (GameManager.Instance.isGameOver) return;
        isJump = true;
        jumpTime = 0f;
        animator.SetTrigger(hashJump);
        jumpPower = CalulateInitialVelocity(Height);
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private float CalulateInitialVelocity(float height)
    {
        float gravityV = Physics2D.gravity.y * Physics2D.gravity.y;
        return Mathf.Sqrt(2f * gravityV * height); // 운동 방정식 적용
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(groundTag))
        {
            isJump = false;
        }
    }
}
