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
    private float curJumpTime;
    [SerializeField] private float jumpTime = 1.5f;
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
        curJumpTime += Time.deltaTime;
    }
    private void OnEnable()
    {
        jumpTime = Random.Range(1f, 2f);
    }
    private void FixedUpdate()
    {
        // ���鿡 ���� �� 1~2�� ���̿� �ѹ� ����
        if (!isJump && curJumpTime > jumpTime)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (GameManager.Instance.isGameOver) return;
        isJump = true;
        animator.SetTrigger(hashJump);
        jumpPower = CalulateInitialVelocity(Height);
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private float CalulateInitialVelocity(float height)
    {
        float gravityV = Physics2D.gravity.y * Physics2D.gravity.y;
        return Mathf.Sqrt(2f * gravityV * height); // � ������ ����
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // ���鿡 ����� �� ���� �ʱ�ȭ
        if (col.gameObject.CompareTag(groundTag))
        {
            curJumpTime = 0f;
            isJump = false;
        }
    }
}
