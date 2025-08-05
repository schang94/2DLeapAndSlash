using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingManger : MonoBehaviour
{
    
    public static PoolingManger p_Instance;

    private GameObject EnemyPrefab;
    private List<GameObject> enemyPool;
    private int maxEnemy = 10;

    private GameObject TrapPrefab;
    private List<GameObject> trapPool;
    private int maxTrap = 10;

    void Awake()
    {
        if (p_Instance == null)
            p_Instance = this;
        else if (p_Instance != this)
            Destroy(gameObject);


        EnemyPrefab = Resources.Load<GameObject>("Enemy");
        TrapPrefab = Resources.Load<GameObject>("Trap");

        StartCoroutine(CreateEnemy());
        StartCoroutine(CreateTrap());
    }

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
    public void SetEnemy()
    {
        foreach (var enemy in enemyPool)
        {
            enemy.SetActive(false);
        }
    }

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

    public void SetTrap()
    {
        foreach (var trap in trapPool)
        {
            trap.SetActive(false);
        }
    }
}
