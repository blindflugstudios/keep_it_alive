using System.Collections.Generic;
using UnityEngine;

namespace KeepItAlive.World
{
    public sealed class WorldBiome
    {
        public int ID { get; }
        public Vector2 Position { get; }
        public float Size { get; }
        public Rect Box => new Rect(Position - new Vector2(Size, Size) / 2.0f, new Vector2(Size, Size));
        public Color Color { get; }
        public float Strength { get; }

        private readonly List<WorldPrefab> _objectsInBiome;
        public IReadOnlyList<WorldPrefab> ObjectsInBiome => _objectsInBiome;

        public WorldBiome(int id, Vector2 pos, WorldBiomeSettings biomeSettings)
        {
            //spawn biome
            ID = id;
            Position = pos;
            Size = biomeSettings.Size;
            Color = biomeSettings.Color;
            Strength = Random.Range(biomeSettings.Strength.x, biomeSettings.Strength.y);
            _objectsInBiome = new List<WorldPrefab>();
        }

        public void AddObjectToBiome(WorldPrefab worldObject)
        {
            _objectsInBiome.Add(worldObject);
        }
    }
}