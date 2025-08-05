using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float startHp = 100;
    public float Health { get; private set; }
    public bool Dead { get; private set; }
    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        Dead = false;
        Health = startHp;
    }

    public virtual void OnDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (OnDeath != null)
        {
            print("´ÙÀÌ");
            OnDeath();
        }
        Dead = true;
    }
}
