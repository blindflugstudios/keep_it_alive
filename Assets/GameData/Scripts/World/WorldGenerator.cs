﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KeepItAlive.World
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private WorldGeneratorSettings _settings;

        [Space] [Header("DEBUG")] 
        [SerializeField] private bool _debugBiomes;
        
        public static WorldGenerator Instance { get; private set; }

        private readonly List<WorldBiome> _spawnedBiomes = new List<WorldBiome>();
        private readonly List<WorldShard> _spawnedShards = new List<WorldShard>();
        private readonly List<WorldPrefab> _spawnedPrefabs = new List<WorldPrefab>();

        private readonly List<float> _probabilities = new List<float>();

        public void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"Instance of {nameof(WorldGenerator)} already exists");
                return;
            }

            Instance = this;
        }

        public void Generate()
        {
            _spawnedBiomes.Clear();
            _spawnedShards.Clear();
            _spawnedPrefabs.Clear();
            
            //Spawning biomes
            var gridCount = Mathf.CeilToInt(_settings.WorldSize / _settings.BiomeGridSize);
            for (var y = 0; y < gridCount; ++y)
            {
                for (var x = 0; x < gridCount; ++x)
                {
                    var biomeID = GetRandomBiome(x, y, gridCount);
                    var biome = _settings.DefaultBiome;
                    if (biomeID >= 0)
                    {
                        biome = _settings.Biomes[biomeID];
                    }
                    var pos = new Vector2(x, y) * _settings.BiomeGridSize + biome.Offset * Random.value;
                    _spawnedBiomes.Add(new WorldBiome(biomeID, pos, biome));
                }
            }
            
            //Spawning Shards
            var shardsToSpawn = _settings.Shards.OrderBy(a => Guid.NewGuid()).ToArray();
            var rowCount = Mathf.CeilToInt(Mathf.Sqrt(shardsToSpawn.Length));
            var rndCells = Enumerable.Range(0, rowCount * rowCount).OrderBy(a => Guid.NewGuid()).ToList();
            var cellSize = _settings.WorldSize / rowCount;
            
            for (var i = 0; i < shardsToSpawn.Length; i++)
            {
                var shard = shardsToSpawn[i];
                var cell = rndCells[i];
                var spawnedShard = Instantiate(shard.gameObject);
                var shardComponent = spawnedShard.GetComponent<WorldShard>();
                
                var (shardOffset, shardSize) = shardComponent.GetSpawnBox();
                var shardMaxSize = Mathf.Max(shardSize.x, shardSize.y);

                var cellPos = (float) cell / rowCount;
                var posY = Mathf.Floor(cellPos) * cellSize + Random.value * (cellSize - shardMaxSize);
                var posX = (cellPos - Mathf.Floor(cellPos)) * cellSize + Random.value * (cellSize - shardMaxSize);

                Debug.Log($"Spawning Shard: {shard.name} on position: [{posX}, {posY}] in cell: {cell}");
                spawnedShard.transform.position = new Vector3(posX, posY, .0f);
                _spawnedShards.Add(spawnedShard.GetComponent<WorldShard>());
            }
            
            for (var i = 0; i < rndCells.Count; i++)
            {
                var cell = rndCells[i];
                var cellPos = (float) cell / rowCount;
                var posY = Mathf.Floor(cellPos) * cellSize;
                var posX = (cellPos - Mathf.Floor(cellPos)) * (rowCount * cellSize);
                Instantiate(_spawnedShards[0], new Vector3(posX, posY), Quaternion.identity);
            }
        }

        private int GetRandomBiome(int x, int y, int gridCount)
        {
            var leftBiomeId = y * gridCount + x - 1;
            var topBiomeId = (y - 1) * gridCount + x;
            WorldBiome leftBiome = null;
            WorldBiome topBiome = null;
            if (leftBiomeId >= 0)
            {
                leftBiome = _spawnedBiomes[leftBiomeId];
            }

            if (topBiomeId >= 0 && leftBiomeId != topBiomeId)
            {
                topBiome = _spawnedBiomes[topBiomeId];
            }

            _probabilities.Clear();
            var accProbability = .0f;
            for (var i = 0; i < _settings.Biomes.Length; i++)
            {
                var biome = _settings.Biomes[i];
                var stickinessFactor = ((leftBiome?.BiomeID == i) ? 1.0f : .0f) + ((topBiome?.BiomeID == i) ? 1.0f : .0f); 
                var probability = biome.Probability + _settings.BiomeStickinessFactor * stickinessFactor;
                accProbability += probability;
                _probabilities.Add(accProbability);
            }

            var rndValue = Random.Range(.0f, accProbability);
            for (var i = 0; i < _probabilities.Count; i++)
            {
                if (rndValue <= _probabilities[i])
                {
                    return i;
                }
            }

            return -1;
        }

        #if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            var color = UnityEditor.Handles.color;
            if (_debugBiomes)
            {
                for (var i = 0; i < _spawnedBiomes.Count; i++)
                {
                    UnityEditor.Handles.color = _spawnedBiomes[i].Color;
                    UnityEditor.Handles.DrawWireCube(_spawnedBiomes[i].Position, new Vector3(_spawnedBiomes[i].Size, _spawnedBiomes[i].Size, 0.02f));
                }
            }
            UnityEditor.Handles.color = color;
        }
        #endif
    }
}