using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRelocation : MonoBehaviour
{
    private BoxCollider2D boxCol;
    private float w;
    private WaitForSeconds ws;
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        w = boxCol.size.x * transform.localScale.x;
        ws = new WaitForSeconds(0.0003f);
        Relocation();
    }

    void Relocation()
    {
        if (transform.position.x < -w * 1.98f)
        {
            Vector2 offset = new Vector2(w * 2.99f, 0);
            transform.position = (Vector2)transform.position + offset;
        }
        StartCoroutine(CRelocation());
    }
    IEnumerator CRelocation()
    {
        yield return ws;
        Relocation();
    }
}
