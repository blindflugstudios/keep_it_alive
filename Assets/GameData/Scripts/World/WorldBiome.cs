using System.Collections.Generic;
using UnityEngine;

namespace KeepItAlive.World
{
    public sealed class WorldBiome
    {
        public int BiomeID { get; }
        public Vector2 Position { get; }
        public float Size { get; }
        public Color Color { get; }
        public List<WorldPrefab> PrefabsInBiome { get; }

        public WorldBiome(int id, Vector2 pos, WorldBiomeSettings biomeSettings)
        {
            //spawn biome
            BiomeID = id;
            Position = pos;
            Size = biomeSettings.Size;
            Color = biomeSettings.Color;
            PrefabsInBiome = new List<WorldPrefab>();
        }
    }
}