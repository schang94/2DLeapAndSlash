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
        // ���� ���� �� Ÿ���� LivingEntity�� ������ �ִٸ�
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
            // �������� �޾��� �� �и��� ���� ����
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
        GameManager.Instance.isHit = true; // �������� �޴� ��
        spriteRenderer.color = Color.red; // �� ����
        animator.SetTrigger(hashHit); // ��Ʈ �ִϸ��̼�
        float time = animator.GetCurrentAnimatorStateInfo(0).length; // �ִϸ��̼� ���� ��������
        yield return new WaitForSeconds(time);
        GameManager.Instance.isHit = false;
        spriteRenderer.color = Color.white;
    }

    protected override void OnEnable()
    {
        var data = GetComponent<PlayerCtrl>().playerData; // ĳ���� ������
        startHp = data.maxHp; // ���� hp����
        base.OnEnable();
        slider.gameObject.SetActive(true);
        slider.maxValue = startHp;
        slider.value = Health;
        damage = data.damage;
    }

    public override void OnDamage(float damage) // �������� �޾��� ��
    {
        base.OnDamage(damage);
        slider.value = Health;
    }

    public override void Die() // ������� ��
    {
        base.Die();
        spriteRenderer.color = Color.white;
        animator.SetTrigger(hashDie);
        GameManager.Instance.Die();
    }
}
