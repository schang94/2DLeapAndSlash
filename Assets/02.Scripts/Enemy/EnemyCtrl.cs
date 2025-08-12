using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyCtrl : MonoBehaviour
{
    private readonly int hashEnd = Animator.StringToHash("End");
    Animator animator;
    Rigidbody2D rb;
    float moveSpeed;
    public bool isDie = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = Random.Range(3f, 5f);
        
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver)
        {
            animator.SetTrigger(hashEnd);
            return;
        }
        Move();

        if (transform.position.x < -12) // 화면에서 사라지면 비활성화
            transform.gameObject.SetActive(false);
    }

    private void Move()
    {
        rb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Impulse);

        if (rb.velocity.x > moveSpeed) // 최대 속도 제한
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        else if (rb.velocity.x < (-1 * moveSpeed))
            rb.velocity = new Vector2((-1 * moveSpeed), rb.velocity.y);
    }
}
