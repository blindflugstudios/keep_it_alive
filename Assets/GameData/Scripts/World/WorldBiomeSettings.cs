using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private WorldPrefabSettings[] _prefabSettings;

        public Vector2 Offset => _offset; 
        public float Size => _size;
        public float Probability => _probability;
        public Vector2 Strength => _strength;
        public Color Color => _color;
        public int SpawnCount => _spawnCount;

        public WorldPrefab GetRandomPrefab()
        {
            var accProbability = .0f;
            for (var i = 0; i < _prefabSettings.Length; i++)
            {
                accProbability += _prefabSettings[i].Probability;
            }

            var value = Random.Range(.0f, accProbability);
            accProbability = .0f;
            for (var i = 0; i < _prefabSettings.Length; i++)
            {
                accProbability += _prefabSettings[i].Probability;
                if (value <= accProbability)
                {
                    return _prefabSettings[i].Prefab;
                }
            }

            Debug.Log("Default prefab returned");
            return _prefabSettings[0].Prefab;
        }
    }

    [Serializable]
    public sealed class WorldPrefabSettings
    {
        [SerializeField] private WorldPrefab _prefab;
        [SerializeField] private float _probability;

        public WorldPrefab Prefab => _prefab;
        public float Probability => _probability;
    }
}