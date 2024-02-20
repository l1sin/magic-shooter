using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [Serializable]
    public struct Tier
    {
        public int Health;
        public int Damage;
        public int Speed;
    }

    public Tier[] Data;
}
