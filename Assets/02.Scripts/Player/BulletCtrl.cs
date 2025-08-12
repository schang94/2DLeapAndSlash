using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    Rigidbody2D rb;
    public float damage;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(Connected());
    }

    private void FixedUpdate()
    {
        MoveLimited();
    }

    IEnumerator Connected()
    {
        // GameManager 기다리기
        while (GameManager.Instance == null) 
        {
            yield return null;
        }

        rb.AddForce(Vector2.right * 80f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        
    }

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        var target = col.GetComponent<EnemyDamage>();
        if (target != null)
        {
            target.OnDamage(damage);
            transform.gameObject.SetActive(false);
        }
    }

    private void MoveLimited()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if (pos.x >= 1f)
            transform.gameObject.SetActive(false);
    }
}
