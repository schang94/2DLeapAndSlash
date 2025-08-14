using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public List<GameObject> prefabList;
    public static GameManager Instance;
    public bool isGameOver = false;
    public bool isHit = false;
    public float score = 0;
    public TMP_Text scoreTxt;
    public static Action<float> OnScoreAction;
    public static Action OnDieAction;
    public static Action OnRestart;
    private Coroutine enemyRespawn;
    private Coroutine trapRespawn;
    private GameObject player;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        RroutineStart();
        SelectCharacter();
    }

    IEnumerator EnemyRespawn()
    {
        while (true)
        {
            float time = UnityEngine.Random.Range(2f, 4f);
            float mobNumber = UnityEngine.Random.Range(0, 10);
            yield return new WaitForSeconds(time);
            var createEnemy = mobNumber % 2 == 0 ? PoolingManger.p_Instance.GetEnemy() : PoolingManger.p_Instance.GetFrog();
   
            createEnemy.transform.position = new Vector2(12f, -2.58f);
            createEnemy.SetActive(true);
        }
    }

    IEnumerator TrapRespawn()
    {
        while (true)
        {
            float time = UnityEngine.Random.Range(1f, 5f);
            yield return new WaitForSeconds(time);
            var createTrap = PoolingManger.p_Instance.GetTrap();

            createTrap.transform.position = new Vector2(12f, UnityEngine.Random.Range(-1, 2));
            createTrap.SetActive(true);
        }
    }

    void SelectCharacter() // �÷��̾� ����
    {
        player = Instantiate(prefabList[SelectManager.s_Instance.character]);
        player.transform.position = new Vector3(-6.03f, -2.76f, 0);
        player.SetActive(true);
    }

    public void ScoreUpdate(float score) // ���ھ� ������Ʈ
    {
        this.score += (score * 100);
        OnScoreAction?.Invoke(this.score);
    }

    public void Die() // �÷��̾� ���
    {
        isGameOver = true;
        RroutineStop(); // ������ ��ƾ ����
        OnDieAction?.Invoke();
        print("����");
    }

    public void ReStart()
    {
        isGameOver = false;
        isHit = false;
        player.SetActive(false);
        score = 0;
        OnScoreAction?.Invoke(this.score);
        OnRestart?.Invoke();
        PoolingManger.p_Instance.SetEnemy();
        PoolingManger.p_Instance.SetFrog();
        PoolingManger.p_Instance.SetTrap();
        RroutineStart(); // ������ ��ƾ ����

        player.transform.position = new Vector3(-6.03f, -2.76f, 0);
        player.SetActive(true);
    }

    public void RroutineStart()
    {
        if (enemyRespawn == null) enemyRespawn = StartCoroutine(EnemyRespawn());
        if (trapRespawn == null)  trapRespawn = StartCoroutine(TrapRespawn());
    }

    public void RroutineStop()
    {
        if (enemyRespawn != null)
        {
            StopCoroutine(enemyRespawn);
            enemyRespawn = null;
        }
        if (trapRespawn != null)
        {
            StopCoroutine(trapRespawn);
            trapRespawn = null;
        }
    }
}
