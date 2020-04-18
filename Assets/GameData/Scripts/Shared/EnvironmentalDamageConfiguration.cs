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

        private EnvironmentalDamageConfiguration _instance;

        private void Awake()
        {
            // if the singleton hasn't been initialized yet
            if (_instance != null && _instance != this) 
            {
                Destroy(gameObject);
            }
    
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}