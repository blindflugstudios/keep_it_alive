using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeepItAlive.Shared
{
    public class EnvironmentalDamageConfiguration : MonoBehaviour
    {
        [SerializeField]
        private float _freezeDamagePerTick;

        [SerializeField]
        private float _radiationDamagePerTick;

        public float FreezeDamagePerTick => _freezeDamagePerTick;

        public float RadiationDamagePerTick => _radiationDamagePerTick;
    }
}