using System;
using UnityEngine;
using KeepItAlive.Shared;

namespace KeepItAlive.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Player : MonoBehaviour, IEntity
    {
        [SerializeField]
        private float _health;

        [SerializeField]
        private EnvironmentalDamageConfiguration _environmentalDamageConfiguration;

        private DamageManager _damageManager;

        private float nextUpdate = 1.0f;

        public float Health => _health;

        public PlayerInventory Inventory;
        
        public void Start()
        {
            _damageManager = new DamageManager(_environmentalDamageConfiguration);
        }

        public void Update()
        {
            //Apply Environment effects every second
            if(Time.time >= nextUpdate)
            {
                _health = _damageManager.ApplyDamageReturnRemainingHealth(_health);   
                nextUpdate = Mathf.FloorToInt(Time.time)+1;
            }

            if(DieCondition())
            {
                Die();
            }
        }

        public void Die()
        {
            DeathAnimation();
            Destroy(gameObject);
        }

        private bool DieCondition()
        {
            //TODO: Discuss and probably more to come
            return _health < 0.0f;
        }

        private void DeathAnimation()
        {
            //TODO: IMPLEMENT
            throw new NotImplementedException();
        }
    }
}
