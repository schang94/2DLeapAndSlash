using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private readonly int hashJump = Animator.StringToHash("IsJump");
    private readonly int hasAttack = Animator.StringToHash("Attack");
    private PlayerInputHandler input;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform weapone;
    [SerializeField] private Transform effect;
    private Transform shadow;
    public PlayerData playerData;
    private float jumpPower = 100f;
    private float moveSpeed = 5f;
    private float maxSpeed = 5f;
    public int isJump = 0;

    private SpriteRenderer spriteRenderer;
    public float Height => spriteRenderer.bounds.size.y;

    private float attackTime;
    private bool isEffect = false;
    private float shadowHeight;
    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hand = transform.GetChild(2).transform;
        weapone = transform.GetChild(1).transform;
        shadow = transform.GetChild(0).transform;
        if (transform.childCount > 3)
        {
            effect = transform.GetChild(3).transform;
            if (effect != null) isEffect = true;
        }
        shadowHeight = transform.position.y;
        rb.gravityScale = -Physics2D.gravity.y;
        attackTime = playerData.attackSpeed;
    }

    
    private void OnEnable()
    {
        StartCoroutine(Connected());
    }
    IEnumerator Connected()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }
        
        input.OnJumpStarted += Jump;
        input.OnAttackStarted += Attack;
    }
    private void OnDisable()
    {
        input.OnJumpStarted -= Jump;
        input.OnAttackStarted -= Attack;
    }
    private void Update()
    {
        if (GameManager.Instance.isGameOver) return;
        GameManager.Instance.ScoreUpdate(Time.deltaTime);
        attackTime += Time.deltaTime;
    }

    private void LateUpdate()
    {
        shadow.position = new Vector2(transform.position.x, shadowHeight);
        
        float shadowScale = Mathf.Clamp(1 + transform.position.y - shadowHeight, 1f, 3f);
        shadow.localScale = new Vector2(shadowScale, 1f);
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver) return;
        Move();
    }

    private void Move()
    {
        if (GameManager.Instance.isHit) return;

        if (input.MoveDir != Vector2.zero)
        {
            rb.AddForce(input.MoveDir.x * Vector2.right * moveSpeed, ForceMode2D.Impulse);

            if (rb.velocity.x > maxSpeed)
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            else if (rb.velocity.x < (-1 * maxSpeed))
                rb.velocity = new Vector2((-1 * maxSpeed), rb.velocity.y);
        }
    }

    

    private void Jump()
    {
        if (GameManager.Instance.isGameOver) return;
        if (input.IsJumping && isJump < 2)
        {
            isJump++;
            
            animator.SetBool(hashJump, true);
            jumpPower = CalulateInitialVelocity(Height);
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            
        }
    }

    private void Attack()
    {
        if (isEffect && isJump > 0) return;

        if (GameManager.Instance.isGameOver) return;
        if (input.IsAttacking && attackTime > playerData.attackSpeed)
        {
            attackTime = 0f;
            if (transform.childCount > 3)
            {
                var bullet = transform.GetChild(4).transform;
                bullet.GetComponent<BulletCtrl>().damage = playerData.damage;
                bullet.position = transform.GetChild(1).GetChild(0).position;
                bullet.gameObject.SetActive(true);
            }
            StartCoroutine(AttackLenght());
        }
    }

    private float CalulateInitialVelocity(float height)
    {
        float gravityV = Physics2D.gravity.y * Physics2D.gravity.y;
        return Mathf.Sqrt(2f * gravityV * height);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJump = 0;
        animator.SetBool(hashJump, false);
    }

    IEnumerator AttackLenght()
    {
        animator.SetTrigger(hasAttack);
        float time = animator.GetCurrentAnimatorStateInfo(0).length;
        hand.gameObject.SetActive(true);
        weapone.gameObject.SetActive(true);
        if (isEffect) effect.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        hand.gameObject.SetActive(false);
        weapone.gameObject.SetActive(false);
        if (isEffect) effect.gameObject.SetActive(false);
    }
}
