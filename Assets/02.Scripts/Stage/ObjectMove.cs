using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private WaitForSeconds ws;
    void Awake()
    {
        ws = new WaitForSeconds(0.0003f);
    }

    private void OnEnable()
    {
        StartCoroutine(Connected());
        GameManager.OnRestart += moveObject;
    }

    private void OnDisable()
    {
        StopCoroutine(Connected());
        GameManager.OnRestart -= moveObject;
    }
    IEnumerator Connected()
    {
        // 게임매니저 기다리기
        while(GameManager.Instance == null)
        {
            yield return null;
        }
        moveObject();
    }
    void moveObject()
    {
        if (GameManager.Instance.isGameOver) return;
        // 오른쪽으로 이동
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        StartCoroutine(CMoveObject());
        
    }

    IEnumerator CMoveObject()
    {
        yield return ws;
        moveObject();
    }
}
