using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    void Start()
    {
        
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
            target.OnDamage(10f);
            //target.DamageEffect();

            transform.gameObject.SetActive(false);
        }
    }

}
