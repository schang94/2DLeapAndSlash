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
    [SerializeField] private List<Transform> player; // �׸���, ����, ��(overlay), ����Ʈ(Gun), ��
    public PlayerData playerData;
    private float jumpPower = 120f;
    private float moveSpeed = 5f;
    private float maxSpeed = 5f;
    private int isJump = 0;

    private SpriteRenderer spriteRenderer;
    private float Height => spriteRenderer.bounds.size.y;

    private float attackTime;
    private bool isEffect = false;
    private float shadowHeight;

    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip attackSound;
    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < transform.childCount; i++)
        {
            player.Add(transform.GetChild(i));
            if (i == 3) isEffect = true;
        }
        shadowHeight = transform.position.y; // �׸��� �ʱ� ��ġ ����
        rb.gravityScale = -Physics2D.gravity.y; // Physics2D.gravity.y�� -9.81
        attackTime = playerData.attackSpeed; // ���ݼӵ�
    }

    
    private void OnEnable()
    {
        StartCoroutine(Connected()); // GameManager ��ٸ���.
    }
    IEnumerator Connected()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }
        
        input.OnJumpStarted += Jump; // ���� �̺�Ʈ ���
        input.OnAttackStarted += Attack; // ���� �̺�Ʈ ���
    }
    private void OnDisable()
    {
        input.OnJumpStarted -= Jump; // ����
        input.OnAttackStarted -= Attack; // ����
    }
    private void Update()
    {
        if (GameManager.Instance.isGameOver) return;
        GameManager.Instance.ScoreUpdate(Time.deltaTime); // ���ھ� �ֽ�ȭ
        attackTime += Time.deltaTime; // ���� �ð� ����
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver) return;
        Move();
    }

    private void LateUpdate()
    {
        // �׸��� ��ġ ����
        player[0].position = new Vector2(transform.position.x, shadowHeight);
        
        // ���� ���̿� ���� �׸��� ũ�� ����
        float shadowScale = Mathf.Clamp(1 + transform.position.y - shadowHeight, 1f, 3f);
        player[0].localScale = new Vector2(shadowScale, 1f);
    }
    

    private void Move()
    {
        if (GameManager.Instance.isHit) return;

        if (input.MoveDir != Vector2.zero)
        {
            rb.AddForce(input.MoveDir.x * Vector2.right * moveSpeed, ForceMode2D.Impulse);

            // �ְ� �ӵ� ����
            if (rb.velocity.x > maxSpeed)
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            else if (rb.velocity.x < (-1 * maxSpeed))
                rb.velocity = new Vector2((-1 * maxSpeed), rb.velocity.y);
        }
    }

    

    private void Jump()
    {
        if (GameManager.Instance.isGameOver) return;
        if (input.IsJumping && isJump < 2) // ����, 2�� ����
        {
            isJump++;
            audioSource.PlayOneShot(jumpSound, 0.5f);
            animator.SetBool(hashJump, true);
            jumpPower = CalulateInitialVelocity(Height);
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            
        }
    }

    private void Attack()
    {
        if (isEffect && isJump > 0) return; // Gun ĳ���� �����ÿ� ������ ���ϰ� �Ѵ�.

        if (GameManager.Instance.isGameOver) return;
        if (input.IsAttacking && attackTime > playerData.attackSpeed)
        {
            attackTime = 0f;
            audioSource.PlayOneShot(attackSound, 0.5f);
            if (transform.childCount > 3) // Gun ĳ�� �Ѿ� �߻�
            {
                var bullet = player[4].transform;
                bullet.GetComponent<BulletCtrl>().damage = playerData.damage;
                bullet.position = player[1].GetChild(0).position;
                bullet.gameObject.SetActive(true);
            }
            StartCoroutine(AttackLenght());
        }
    }

    private float CalulateInitialVelocity(float height)
    {
        float gravityV = Physics2D.gravity.y * Physics2D.gravity.y;
        return Mathf.Sqrt(2f * gravityV * height); // � ������ ����
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
        player[1].gameObject.SetActive(isEnable);
        player[2].gameObject.SetActive(isEnable);
        if (isEffect) player[3].gameObject.SetActive(isEnable);
    }
}
