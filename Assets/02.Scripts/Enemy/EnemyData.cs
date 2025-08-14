using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 2)]
public class EnemyData : ScriptableObject
{
    public float maxHp = 100f;
    public float damage = 20f;
    public float moveSpeed = 10f;
}
