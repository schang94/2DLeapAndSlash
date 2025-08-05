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
        if (transform.position.x < -11f && transform.gameObject.activeSelf)
        {
            transform.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var target = col.GetComponent<LivingEntity>();
        if (target != null)
        {
            target.OnDamage(10f);
            var player = target.gameObject.GetComponent<PlayerDamage>();
            if (player != null)
            {
                player.DamageEffect();
            }
            transform.gameObject.SetActive(false);
        }
    }

}
