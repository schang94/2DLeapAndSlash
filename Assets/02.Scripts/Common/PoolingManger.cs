using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingManger : MonoBehaviour
{
    
    public static PoolingManger p_Instance;

    private GameObject EnemyPrefab;
    private List<GameObject> enemyPool;
    private int maxEnemy = 5;

    private GameObject FrogPrefab;
    private List<GameObject> frogPool;
    private int maxFrog = 5;

    private GameObject TrapPrefab;
    private List<GameObject> trapPool;
    private int maxTrap = 10;

    void Awake()
    {
        if (p_Instance == null)
            p_Instance = this;
        else if (p_Instance != this)
            Destroy(gameObject);

        // Resources에서 할당
        EnemyPrefab = Resources.Load<GameObject>("Mask");
        TrapPrefab = Resources.Load<GameObject>("Saw");
        FrogPrefab = Resources.Load<GameObject>("Frog");

        StartCoroutine(CreateEnemy());
        StartCoroutine(CreateFrog());
        StartCoroutine(CreateTrap());
    }

    // Enemy 오브젝트 풀링
    IEnumerator CreateEnemy()
    {
        yield return new WaitForSeconds(0.03f);
        enemyPool = new List<GameObject>(maxEnemy);
        GameObject eo = new GameObject("EnemyObjects");
        for (int i = 0; i < maxEnemy; i++)
        {
            var enemy = Instantiate(EnemyPrefab, eo.transform);
            enemy.name = $"Enemy {i + 1}";
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    public GameObject GetEnemy()
    {
        foreach(var enemy in enemyPool)
        {
            if (!enemy.activeSelf)
                return enemy;
        }

        return null;
    }
    public void SetEnemy() // 플레이어 사망시 적용할 메서드
    {
        foreach (var enemy in enemyPool)
        {
            enemy.SetActive(false);
        }
    }

    // Frog 오브젝트 풀링
    IEnumerator CreateFrog()
    {
        yield return new WaitForSeconds(0.03f);
        frogPool = new List<GameObject>(maxFrog);
        GameObject eo = new GameObject("FrogObjects");
        for (int i = 0; i < maxFrog; i++)
        {
            var frog = Instantiate(FrogPrefab, eo.transform);
            frog.name = $"Enemy {i + 1}";
            frog.SetActive(false);
            frogPool.Add(frog);
        }
    }

    public GameObject GetFrog()
    {
        foreach (var frog in frogPool)
        {
            if (!frog.activeSelf)
                return frog;
        }

        return null;
    }
    public void SetFrog() // 플레이어 사망시 적용할 메서드
    {
        foreach (var frog in frogPool)
        {
            frog.SetActive(false);
        }
    }

    // Trap 오브젝트 풀링
    IEnumerator CreateTrap()
    {
        yield return new WaitForSeconds(0.03f);
        trapPool = new List<GameObject>(maxTrap);
        GameObject obj = new GameObject("TrapObjects");
        for (int i = 0;i < maxTrap;i++)
        {
            var trap = Instantiate(TrapPrefab, obj.transform);
            trap.name = $"{i + 1} trap";
            trap.SetActive(false);
            trapPool.Add(trap);
        }
    }

    public GameObject GetTrap()
    {
        foreach (var trap in trapPool)
        {
            if (!trap.activeSelf)
                return trap;
        }
        return null;
    }

    public void SetTrap() // 플레이어 사망시 적용할 메서드
    {
        foreach (var trap in trapPool)
        {
            trap.SetActive(false);
        }
    }
}
