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
            target.OnDamage(10f);
            //target.DamageEffect();

            transform.gameObject.SetActive(false);
        }
    }

}
