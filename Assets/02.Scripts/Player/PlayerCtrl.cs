using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private readonly int hashJump = Animator.StringToHash("IsJump");
    private readonly int hasAttack = Animator.StringToHash("Attack");
    private readonly string groundTag = "Ground";
    private PlayerInputHandler input;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private Transform hand; // overlay
    [SerializeField] private Transform weapone; // 무기
    [SerializeField] private Transform effect; // Gun 캐릭 공격 이펙트
    private Transform shadow;
    public PlayerData playerData;
    private float jumpPower = 120f;
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
        if (transform.childCount > 3) // Gun 캐릭터 사용시
        {
            effect = transform.GetChild(3).transform; 
            if (effect != null) isEffect = true;
        }
        shadowHeight = transform.position.y; // 그림자 초기 위치 설정
        rb.gravityScale = -Physics2D.gravity.y; // Physics2D.gravity.y는 -9.81
        attackTime = playerData.attackSpeed; // 공격속도
    }

    
    private void OnEnable()
    {
        StartCoroutine(Connected()); // GameManager 기다린다.
    }
    IEnumerator Connected()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }
        
        input.OnJumpStarted += Jump; // 점프 이벤트 등록
        input.OnAttackStarted += Attack; // 어택 이벤트 등록
    }
    private void OnDisable()
    {
        input.OnJumpStarted -= Jump; // 삭제
        input.OnAttackStarted -= Attack; // 삭제
    }
    private void Update()
    {
        if (GameManager.Instance.isGameOver) return;
        GameManager.Instance.ScoreUpdate(Time.deltaTime); // 스코어 최신화
        attackTime += Time.deltaTime; // 공격 시간 증가
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver) return;
        Move();
    }

    private void LateUpdate()
    {
        // 그림자 위치 조정
        shadow.position = new Vector2(transform.position.x, shadowHeight);
        
        // 점프 높이에 따라 그림자 크기 조정
        float shadowScale = Mathf.Clamp(1 + transform.position.y - shadowHeight, 1f, 3f);
        shadow.localScale = new Vector2(shadowScale, 1f);
    }
    

    private void Move()
    {
        if (GameManager.Instance.isHit) return;

        if (input.MoveDir != Vector2.zero)
        {
            rb.AddForce(input.MoveDir.x * Vector2.right * moveSpeed, ForceMode2D.Impulse);

            // 최고 속도 제한
            if (rb.velocity.x > maxSpeed)
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            else if (rb.velocity.x < (-1 * maxSpeed))
                rb.velocity = new Vector2((-1 * maxSpeed), rb.velocity.y);
        }
    }

    

    private void Jump()
    {
        if (GameManager.Instance.isGameOver) return;
        if (input.IsJumping && isJump < 2) // 점프, 2단 점프
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
        if (isEffect && isJump > 0) return; // Gun 캐릭은 점프시에 공격을 못하게 한다.

        if (GameManager.Instance.isGameOver) return;
        if (input.IsAttacking && attackTime > playerData.attackSpeed)
        {
            attackTime = 0f;
            if (transform.childCount > 3) // Gun 캐릭 총알 발사
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
        return Mathf.Sqrt(2f * gravityV * height); // 운동 방정식 적용
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isJump = 0;
            animator.SetBool(hashJump, false);
        }
    }

    IEnumerator AttackLenght()
    {
        animator.SetTrigger(hasAttack);
        float time = animator.GetCurrentAnimatorStateInfo(0).length;
        AttackMotion(true);
        yield return new WaitForSeconds(time);
        AttackMotion(false);
    }

    private void AttackMotion(bool isEnable)
    {
        hand.gameObject.SetActive(isEnable);
        weapone.gameObject.SetActive(isEnable);
        if (isEffect) effect.gameObject.SetActive(isEnable);
    }
}
