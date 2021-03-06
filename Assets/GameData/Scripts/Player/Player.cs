﻿using KeepItAlive.Characters;
using System;
using UnityEngine;
using KeepItAlive.Shared;
using System.Collections;
using UnityEngine.InputSystem;

namespace KeepItAlive.Player
{
    public class Player : MonoBehaviour, IEntity
    {
        [SerializeField]
        private float _health;

        [SerializeField]
        private Configuration _configuration;

		[SerializeField] private CharacterAnimator _animator;
		[SerializeField] private float _deathAnimationTime;

        [SerializeField]
        private HealthbarWorldUI _playerCanvas;

		public event Action Dead;

        private DamageManager _damageManager;
        
		private PlayerInput _input;

        private float nextUpdate = 1.0f;

		private bool _isDead;

        public float Health => _health;

        public bool ReceivesRadiationDamage => _damageManager.ReceivesRadiationDamage;

        public bool ReceivesFreezeDamage => _damageManager.ReceivesFreezeDamage;        
        
        public PlayerInventory Inventory;

		public void OnDestinationReached()
		{
			_input.enabled = false;
		}

		[ContextMenu("Die")]
		public void Die()
		{
			if (_isDead == false)
			{
				StartCoroutine(DeathCoroutine());
				_isDead = true;
			}
		}

        private void Awake()
        {
             _damageManager = new DamageManager(_configuration);
             _damageManager.ReceivesFreezeDamage = true;
        }

		private void Start()
        {
			_input = GetComponent<PlayerInput>();
			_input.enabled = false;
			_input.enabled = true;
		}

		private void Update()
        {
	        if (Inventory.HasTorch)
	        {
		        _damageManager.ReceivesFreezeDamage = false;
		        _damageManager.ReceivesRadiationDamage = true;
	        }
	        
            //Apply Environment effects every second
            if(Time.time >= nextUpdate)
			{
				DealDamage();
                nextUpdate = Mathf.FloorToInt(Time.time)+1;
            }

            if(DieCondition())
            {
                Die();
            }
        }

		private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag(Tags.RadioactiveTag))
            {
                _damageManager.ReceivesFreezeDamage = false;
                _damageManager.ReceivesRadiationDamage = true;
            }

            if (other.CompareTag(Tags.EnemyTag))
            {
                DealDamage();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.CompareTag(Tags.RadioactiveTag))
            {
                _damageManager.ReceivesFreezeDamage = true;
                _damageManager.ReceivesRadiationDamage = false;
            }
        }

		private void DealDamage()
		{
			float remainingHealth = _damageManager.ApplyDamageReturnRemainingHealth(_health);
			if (_health > remainingHealth)
			{
				_animator?.TriggerDamage();					
			}
			_health = remainingHealth; 
		}

		public void DealDamage(float damge)
		{
			_animator?.TriggerDamage();
			_health -= damge;
		}


		private IEnumerator DeathCoroutine()
		{
			_input.enabled = false;
			
			_animator?.TriggerDeath();
			yield return new WaitForSeconds(_deathAnimationTime); //Animation goes here
			
			Dead?.Invoke();
			
			Destroy(gameObject);
		}

		private bool DieCondition()
        {
            //TODO: Discuss and probably more to come
            return _health < 0.0f;
        }
	}
}
