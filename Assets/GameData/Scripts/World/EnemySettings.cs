using System;
using KeepItAlive.Enemies;
using UnityEngine;

namespace KeepItAlive.World
{
    [Serializable]
    public struct EnemySpawnSettings
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private float _probability;

        public Enemy Enemy => _enemy;
        public float Probability => _probability;
    }
}