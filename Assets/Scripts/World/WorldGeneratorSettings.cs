using System;
using UnityEngine;

namespace KeepItAlive.World
{
    [Serializable]
    public sealed class WorldGeneratorSettings
    {
        [Header("General")]
        [SerializeField] private float _worldSize;

        public float WorldSize => _worldSize;

        [Space] [Header("Biomes")] 
        [SerializeField] private float _biomeStickinessFactor;
        [SerializeField] private float _biomeGridSize;
        [SerializeField] private WorldBiomeSettings[] _biomes;
        
        public float BiomeStickinessFactor => _biomeStickinessFactor;
        public float BiomeGridSize => _biomeGridSize;
        public WorldBiomeSettings[] Biomes => _biomes;
        public WorldBiomeSettings DefaultBiome => _biomes[0];
        
        [Space] [Header("Shards")]
        [SerializeField] private WorldShard[] _shards;

        public WorldShard[] Shards => _shards;
    }
}