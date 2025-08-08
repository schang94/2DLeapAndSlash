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
    private bool isHit = false;
    public Slider slider;
    private float damage;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        slider = GameObject.Find("Canvas_UI").transform.GetChild(0).GetComponent<Slider>();
       
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isHit) return;
        var target = col.transform.GetComponent<EnemyDamage>();
        if (target != null && !target.isDie)
        {
            Vector2 offset = (col.transform.position - transform.position).normalized;
            if (Mathf.Abs(offset.x)>Mathf.Abs(offset.y))
                rb.AddForce(Vector2.left * 20f * Mathf.RoundToInt(offset.x), ForceMode2D.Impulse);
            else
                rb.AddForce(Vector2.left * 15f + Vector2.up * 10f, ForceMode2D.Impulse);
            DamageEffect();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        LivingEntity target = col.GetComponent<LivingEntity>();
        if (target != null)
        {
            print("АјАн");
            target.OnDamage(damage);
        }
    }

    public void DamageEffect()
    {
        StartCoroutine(OnDamageEffect());
    }
    IEnumerator OnDamageEffect()
    {
        isHit = true;
        GameManager.Instance.isHit = true;
        spriteRenderer.color = Color.red;
        animator.SetTrigger(hashHit);
        float time = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);
        isHit = false;
        GameManager.Instance.isHit = false;
        spriteRenderer.color = Color.white;
    }

    protected override void OnEnable()
    {
        var data = GetComponent<PlayerCtrl>().playerData;
        startHp = data.maxHp;
        base.OnEnable();
        slider.gameObject.SetActive(true);
        slider.maxValue = startHp;
        slider.value = Health;
        damage = data.damage;
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        slider.value = Health;
    }

    public override void Die()
    {
        base.Die();
        spriteRenderer.color = Color.white;
        animator.SetTrigger(hashDie);
        GameManager.Instance.Die();
    }
}
