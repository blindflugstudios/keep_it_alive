using System;
using UnityEngine;
using KeepItAlive.Shared;

namespace KeepItAlive.Player
{
    public class Player : MonoBehaviour, IEntity
    {
        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private float _health;

        public Sprite Sprite => _sprite;

        public float Health => _health;

        public EnvironmentalDamageConfiguration _environmentalDamageConfiguration;

        public void Update()
        {
            if(DieCondition())
            {
                Die();
            }
        }
        public void Freeze()
        {
            _health -= _environmentalDamageConfiguration.FreezeDamagePerTick;
        }

        public void Radiate()
        {
            _health -= _environmentalDamageConfiguration.RadiationDamagePerTick;
        }

        public void Die()
        {
            DeathAnimation();
            Destroy(gameObject);
        }

        private bool DieCondition()
        {
            //TODO: Discuss and probably more to come
            return _health >= 0.0f;
        }

        private void DeathAnimation()
        {
            //TODO: IMPLEMENT
            throw new NotImplementedException();
        }
    }
}
