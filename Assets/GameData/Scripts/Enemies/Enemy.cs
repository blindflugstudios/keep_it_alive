﻿using KeepItAlive.Characters;
using UnityEngine;
using KeepItAlive.Shared;
using System.Collections;

namespace KeepItAlive.Enemies
{
    public class Enemy : MonoBehaviour, IEntity
    {
        [SerializeField]
        private float _health;

        [SerializeField]
        private GameObject _deathItemPrefab;
		[SerializeField] private CharacterAnimator _animator;
		[SerializeField] private float _deathAnimationTime;

        private DamageManager _damageManager;
		private bool _isDead;

        public float Health => _health;

        public bool ReceivesRadiationDamage => false;

        public bool ReceivesFreezeDamage => false;

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
			if (_isDead == false)
			{
				StartCoroutine(DeathAnimation());
				_isDead = true;
			}
        }
        
        private bool DieCondition()
        {
            return _health < 0.0f;
        }

        private IEnumerator DeathAnimation()
        {
			_animator?.TriggerDeath();
			yield return new WaitForSeconds(_deathAnimationTime);
			
			//Turns into other object
			Instantiate(_deathItemPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.ArrowTag))
            {
                float remainingHealth = _damageManager.ApplyDamageReturnRemainingHealth(_health);
                if (remainingHealth < _health)
                {
                    _animator?.TriggerDamage();
                }
                _health = remainingHealth;
            }
        }
    }
}
