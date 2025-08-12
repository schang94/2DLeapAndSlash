using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyDamage : LivingEntity
{
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashDie = Animator.StringToHash("Die");
    private Animator animator;
    public bool isDie = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    protected override void OnEnable()
    {
        startHp = 100;
        base.OnEnable();
        isDie = false;
        
    }

    public override void OnDamage(float damage) // 데미지를 받았을 때
    {
        base.OnDamage(damage);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10f, ForceMode2D.Impulse); // 뒤로 밀리게하기
        animator.SetTrigger(hashHit);
    }

    public override void Die()
    {
        base.Die();
        isDie = true;
        StartCoroutine(OnDie());
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Enemy와 플레이어서 닿았을 때
        LivingEntity target = col.transform.GetComponent<PlayerDamage>();
        if (target != null && !isDie)
        {
            target.OnDamage(10f);
        }
    }

    IEnumerator OnDie()
    {
        animator.SetTrigger(hashDie);
        float time = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);
        transform.gameObject.SetActive(false);

    }
}
