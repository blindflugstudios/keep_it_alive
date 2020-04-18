using System;
using UnityEngine;

namespace KeepItAlive.World
{
    [Serializable]
    public sealed class WorldBiomeSettings
    {
        [SerializeField] private Vector2 _offset;
        [SerializeField] private float _size;
        [SerializeField] private Vector2 _strength;
        [SerializeField] private float _probability;
        [SerializeField] private int _spawnCount;
        [SerializeField] private Color _color;
        [SerializeField] private WorldPrefab[] _worldPrefabs;

        public Vector2 Offset => _offset; 
        public float Size => _size;
        public float Probability => _probability;
        public Vector2 Strength => _strength;
        public Color Color => _color;
        public int SpawnCount => _spawnCount;
        public WorldPrefab[] WorldPrefabs => _worldPrefabs;
    }
}