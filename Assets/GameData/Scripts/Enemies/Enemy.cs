using System;
using UnityEngine;
using KeepItAlive.Shared;

namespace KeepItAlive.Enemies
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Enemy : MonoBehaviour, IEntity
    {
        [SerializeField]
        private float _health;

        [SerializeField]
        private GameObject _deathItemPrefab;

        private DamageManager _damageManager;

        public float Health => _health;

        private void Start()
        {
            _damageManager = new DamageManager();
        }

        private void Update()
        {
            if(DieCondition())
            {
                Die();
            }
        }

        public void Die()
        {
            DeathAnimation();
            Destroy(gameObject);

            //Turns into other object
            Instantiate(_deathItemPrefab);
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Arrow"))
            {
                _health = _damageManager.ApplyDamageReturnRemainingHealth(_health);
            }
        }
    }
}