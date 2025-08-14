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
        // 화면에서 사라졌을 때 비활성화
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
            // 트랩에 닿았을 때는 넉백을 하지 않는다.
            target.OnDamage(damage);
            //target.DamageEffect();

            transform.gameObject.SetActive(false);
        }
    }

}
