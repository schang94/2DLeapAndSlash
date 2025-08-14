using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public TrapData trapData;
    private float damage;
    void Start()
    {
        GetComponent<ObjectMove>().moveSpeed = trapData.moveSpeed;
        damage = trapData.damage;
    }

    void Update()
    {
        // ȭ�鿡�� ������� �� ��Ȱ��ȭ
        if (transform.position.x < -11f && transform.gameObject.activeSelf)
        {
            transform.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var target = col.GetComponent<PlayerDamage>();
        if (target != null)
        {
            // Ʈ���� ����� ���� �˹��� ���� �ʴ´�.
            target.OnDamage(damage);
            //target.DamageEffect();

            transform.gameObject.SetActive(false);
        }
    }

}
