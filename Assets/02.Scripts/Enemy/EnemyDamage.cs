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
        DieSetting(false);

    }

    public override void OnDamage(float damage) // �������� �޾��� ��
    {
        base.OnDamage(damage);
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10f, ForceMode2D.Impulse); // �ڷ� �и����ϱ�
        animator.SetTrigger(hashHit);
    }

    public override void Die()
    {
        base.Die();
        StartCoroutine(OnDie());
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        // Enemy�� �÷��̾ ����� ��
        PlayerDamage target = col.transform.GetComponent<PlayerDamage>();
        if (target != null && !isDie)
        {
            target.KnockBack(GetComponent<EnemyDamage>(), 10f);
            //target.OnDamage(10f);
        }
    }
    IEnumerator OnDie()
    {
        DieSetting(true);
        animator.SetTrigger(hashDie);
        float time = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);
        transform.gameObject.SetActive(false);
    }

    private void DieSetting(bool isEnable)
    {
        isDie = isEnable;
        GetComponent<Rigidbody2D>().simulated = !isEnable;
        GetComponent<CapsuleCollider2D>().isTrigger = isEnable;
    }
}
