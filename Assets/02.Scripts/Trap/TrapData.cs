using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrapData", menuName = "Data/TrapData", order = 3)]
public class TrapData : ScriptableObject
{
    public float damage = 10f;
    public float moveSpeed = 5f;
}
