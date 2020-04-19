using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KeepItAlive.Enemies;
using KeepItAlive.Science;
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
        private readonly List<WorldPrefab> _spawnedWorldObjects = new List<WorldPrefab>();
        
        private readonly List<float> _probabilities = new List<float>();

        public WorldGeneratorSettings Settings => _settings;

        public void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"Instance of {nameof(WorldGenerator)} already exists");
                return;
            }

            Instance = this;
        }

        public SpawnInfo Generate()
        {
            _spawnedBiomes.Clear();
            _spawnedShards.Clear();
            _spawnedWorldObjects.Clear();

            SpawnBiomes();
            SpawnInfo spawnInfo = SpawnShards();
            SpawnDecals();
			StartCoroutine(SpawnWorld());

			return spawnInfo;
        }

        private IEnumerator SpawnWorld()
        {
            yield return null;
            SpawnPrefabs();
            yield return null;
            SpawnEnemies();
        }

        private void SpawnBiomes()
        {
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
                var stickinessFactor = ((leftBiome?.ID == i) ? 1.0f : .0f) + ((topBiome?.ID == i) ? 1.0f : .0f); 
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

        private SpawnInfo SpawnShards()
        {
			var spawnInfo = new SpawnInfo();
            var shardsToSpawn = _settings.Shards.ToList();
            shardsToSpawn.AddRange(new[]
                {_settings.StartPoint.GetComponent<WorldShard>(), _settings.FinishPoint.GetComponent<WorldShard>()});
            var rowCount = Mathf.CeilToInt(Mathf.Sqrt(shardsToSpawn.Count));
            var rndCells = Enumerable.Range(0, rowCount * rowCount).OrderBy(a => Guid.NewGuid()).ToList();
            var cellSize = _settings.WorldSize / rowCount;

            for (var i = 0; i < shardsToSpawn.Count; i++)
            {
                var shard = shardsToSpawn[i];
                var cell = rndCells[i];
                var spawnedShard = Instantiate(shard.gameObject);
                var shardComponent = spawnedShard.GetComponent<WorldShard>();

				if (spawnedShard.TryGetComponent(out StartPoint startPoint))
				{
					spawnInfo.StartPoint = startPoint;
				}
				else if (spawnedShard.TryGetComponent(out FinishPoint finishPoint))
				{
					spawnInfo.FinishPoint = finishPoint;
				}

				var shardRect = shardComponent.Box;
                var shardSize = shardRect.size;
                var shardMaxSize = Mathf.Min(Mathf.Max(shardSize.x, shardSize.y) + cellSize / 4.0f, cellSize);

                var cellPos = (float) cell / rowCount;
                var posY = Mathf.Floor(cellPos) * cellSize + Random.value * (cellSize - shardMaxSize);
                var posX = (cellPos - Mathf.Floor(cellPos)) * (rowCount * cellSize) + Random.value * (cellSize - shardMaxSize);

                spawnedShard.transform.position = new Vector3(posX, posY, .0f);
                _spawnedShards.Add(spawnedShard.GetComponent<WorldShard>());
            }

			return spawnInfo;
		}
        
        private void SpawnPrefabs()
        {
            var biomeObjects = new List<WorldPrefab>();
            for (var i = 0; i < _spawnedBiomes.Count; i++)
            {
                biomeObjects.Clear();
                var biome = _spawnedBiomes[i];
                var nearbyBiomes = GetNearbyBiomes(i);
                var numSpawn = _settings.Biomes[biome.ID].SpawnCount;
                var biomeSettings = _settings.Biomes[biome.ID];
                WorldPrefab prevWorldObject = null;
                for (var b = 0; b < nearbyBiomes.Count; b++)
                {
                    biomeObjects.AddRange(nearbyBiomes[b].ObjectsInBiome);
                }

                for (var j = 0; j < numSpawn; ++j)
                {
                    WorldPrefab worldObject;
                    if (prevWorldObject != null)
                    {
                        worldObject = prevWorldObject;
                        prevWorldObject = null;
                    }
                    else
                    {
                        worldObject = Instantiate(biomeSettings.GetRandomPrefab());
                    }

                    var pos = (biome.Position - new Vector2(biome.Size, biome.Size) / 2.0f) + new Vector2(biome.Size * Random.value, biome.Size * Random.value);
                    var collisionBounds = worldObject.Bounds;
                    var center = collisionBounds.center + new Vector3(pos.x, pos.y, .0f);
                    center.z = .0f;
                    collisionBounds.center = center;
                    if (CheckForCollision(collisionBounds, biome, nearbyBiomes, biomeObjects) == false)
                    {
                        worldObject.transform.position = pos;
                        _spawnedWorldObjects.Add(worldObject);
                        biome.AddObjectToBiome(worldObject);
                        biomeObjects.Add(worldObject);
                    }
                    else
                    {
                        prevWorldObject = worldObject;
                    }

                    if (j == numSpawn - 1 && prevWorldObject != null)
                    {
                        Destroy(prevWorldObject.gameObject);
                    }
                }
            }
        }
        
        private List<WorldBiome> GetNearbyBiomes(int biomeCell)
        {
            var gridCount = Mathf.CeilToInt(_settings.WorldSize / _settings.BiomeGridSize);
            
            var biomes = new List<WorldBiome>();
            var biomeRow = biomeCell / gridCount;
            for (var i = biomeRow - 1; i <= biomeRow + 1; ++i)
            {
                if (i < 0 || i >= gridCount)
                {
                    continue;
                }

                var biomeColumn = biomeCell - (biomeRow * gridCount);
                for (var j = biomeColumn - 1; j <= biomeColumn + 1; ++j)
                {
                    var cell = i * gridCount + j;
                    if (j >= 0 && j < gridCount && cell != biomeCell)
                    {
                        biomes.Add(_spawnedBiomes[cell]);
                    }
                }
            }

            return biomes;
        }

        private bool CheckForCollision(Bounds collisionBounds, WorldBiome biome, List<WorldBiome> nearbyBiomes,
            List<WorldPrefab> biomeObjects)
        {
            //biome collision
            for (var i = 0; i < nearbyBiomes.Count; i++)
            {
                if (biome.Strength < nearbyBiomes[i].Strength && (nearbyBiomes[i].Box.Contains(collisionBounds.min) || nearbyBiomes[i].Box.Contains(collisionBounds.max)))
                {
                    return true;
                }
            }

            if (Physics2D.OverlapBox(collisionBounds.center, collisionBounds.size, .0f))
            {
                return true;
            }
            
            //objects collision
            for (var i = 0; i < biomeObjects.Count; i++)
            {
                var objectBounds = biomeObjects[i].Bounds;
                var center = objectBounds.center + new Vector3(biomeObjects[i].transform.position.x, biomeObjects[i].transform.position.y, .0f);
                center.z = .0f;
                objectBounds.center = center;
                if (objectBounds.Overlap(collisionBounds))
                {
                    return true;
                }
            }
            return false;
        }
        
        public void SpawnEnemies()
        {
            var accProbability = .0f;
            for (var i = 0; i < _settings.EnemySettings.Length; i++)
            {
                accProbability += _settings.EnemySettings[i].Probability;
            }

            for (var i = 0; i < _settings.StartingEnemies; ++i)
            {
                var rndProbability = Random.Range(.0f, accProbability);
                var enemy = GetRandomEnemy(rndProbability);

                var rndPosition = new Vector2(Random.Range(.0f, Settings.WorldSize), Random.Range(.0f, Settings.WorldSize));
                while (Physics2D.OverlapPoint(rndPosition) != null)
                {
                    rndPosition = new Vector2(Random.Range(.0f, Settings.WorldSize), Random.Range(.0f, Settings.WorldSize));
                }

                Instantiate(enemy, rndPosition, Quaternion.identity);
            }
        }
        
        private Enemy GetRandomEnemy(float rndProbability)
        {
            var enemyProbability = .0f;
            for (var i = 0; i < _settings.EnemySettings.Length; i++)
            {
                enemyProbability += _settings.EnemySettings[i].Probability;
                if (rndProbability <= enemyProbability)
                {
                    return _settings.EnemySettings[i].Enemy;
                }
            }

            return _settings.EnemySettings[0].Enemy;
        }

        public void SpawnDecals()
        {
            for (var i = 0; i < _settings.DecalAmount; ++i)
            {
                var rndPosition = new Vector2(Random.Range(.0f, Settings.WorldSize), Random.Range(.0f, Settings.WorldSize));
                Instantiate(_settings.Decals[Random.Range(0, _settings.Decals.Length)], rndPosition, Quaternion.identity);
            }
        }
		
		public struct SpawnInfo
		{
			public StartPoint StartPoint { get; set; }
			public FinishPoint FinishPoint { get; set; }
		}

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            var worldSize = new Vector3(_settings.WorldSize, _settings.WorldSize, .1f);
            Gizmos.DrawWireCube(transform.position + worldSize / 2.0f, worldSize);

            var color = UnityEditor.Handles.color;
            if (_debugBiomes)
            {
                for (var i = 0; i < _spawnedBiomes.Count; i++)
                {
                    UnityEditor.Handles.color = _spawnedBiomes[i].Color;
                    UnityEditor.Handles.DrawWireCube(_spawnedBiomes[i].Position, new Vector3(_spawnedBiomes[i].Size, _spawnedBiomes[i].Size, 0.02f));
                }
            }

            UnityEditor.Handles.color = Color.magenta;
            for (var i = 0; i < _spawnedWorldObjects.Count; i++)
            {
                var spawnedWorldObject = _spawnedWorldObjects[i];
                UnityEditor.Handles.DrawWireCube(spawnedWorldObject.Bounds.center, spawnedWorldObject.Bounds.size);
            }
            UnityEditor.Handles.color = color;
        }
        #endif
    }
}
