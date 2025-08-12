using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerDamage : LivingEntity
{
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashDie = Animator.StringToHash("Die");
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    public Slider slider;
    private float damage;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        slider = GameObject.Find("Canvas_UI").transform.GetChild(0).GetComponent<Slider>();
       
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // 공격 했을 때 타켓이 LivingEntity를 가지고 있다면
        LivingEntity target = col.GetComponent<LivingEntity>();
        if (target != null)
        {
            target.OnDamage(damage);
        }
    }

    public void KnockBack(EnemyDamage target, float damage)
    {
        if (GameManager.Instance.isHit) return;
        //var target = col.transform.GetComponent<EnemyDamage>();
        if (target != null && !target.isDie)
        {
            Vector2 offset = (target.transform.position - transform.position).normalized;
            // 데미지를 받았을 때 밀리는 방향 설정
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
                rb.AddForce(Vector2.left * 20f * Mathf.RoundToInt(offset.x), ForceMode2D.Impulse);
            else
                rb.AddForce(Vector2.left * 15f + Vector2.up * 10f, ForceMode2D.Impulse);

            OnDamage(damage);
            DamageEffect();
        }
    }
    public void DamageEffect()
    {
        StartCoroutine(OnDamageEffect());
    }
    IEnumerator OnDamageEffect()
    {
        GameManager.Instance.isHit = true; // 데미지를 받는 중
        spriteRenderer.color = Color.red; // 색 변경
        animator.SetTrigger(hashHit); // 히트 애니메이션
        float time = animator.GetCurrentAnimatorStateInfo(0).length; // 애니메이션 길이 가져오기
        yield return new WaitForSeconds(time);
        GameManager.Instance.isHit = false;
        spriteRenderer.color = Color.white;
    }

    protected override void OnEnable()
    {
        var data = GetComponent<PlayerCtrl>().playerData; // 캐릭터 데이터
        startHp = data.maxHp; // 시작 hp설정
        base.OnEnable();
        slider.gameObject.SetActive(true);
        slider.maxValue = startHp;
        slider.value = Health;
        damage = data.damage;
    }

    public override void OnDamage(float damage) // 데미지를 받았을 때
    {
        base.OnDamage(damage);
        slider.value = Health;
    }

    public override void Die() // 사망했을 때
    {
        base.Die();
        spriteRenderer.color = Color.white;
        animator.SetTrigger(hashDie);
        GameManager.Instance.Die();
    }
}
