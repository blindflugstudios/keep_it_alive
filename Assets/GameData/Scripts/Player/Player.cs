using KeepItAlive.Characters;
using System;
using UnityEngine;
using KeepItAlive.Shared;
using System.Collections;
using UnityEngine.InputSystem;

namespace KeepItAlive.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Player : MonoBehaviour, IEntity
    {
        [SerializeField]
        private float _health;

        [SerializeField]
        private EnvironmentalDamageConfiguration _environmentalDamageConfiguration;

		[SerializeField] private CharacterAnimator _animator;
		[SerializeField] private float _deathAnimationTime;

		public event Action Dead;

        private DamageManager _damageManager;
		private PlayerInput _input;

        private float nextUpdate = 1.0f;

        public float Health => _health;

        public PlayerInventory Inventory;
        
        public void Start()
        {
            _damageManager = new DamageManager(_environmentalDamageConfiguration);
			_input = GetComponent<PlayerInput>();
			_input.enabled = false;
			_input.enabled = true;
		}

        public void Update()
        {
            //Apply Environment effects every second
            if(Time.time >= nextUpdate)
			{
				float remainingHealth = _damageManager.ApplyDamageReturnRemainingHealth(_health);
				if (_health > remainingHealth)
				{
					_animator?.TriggerDamage();					
				}
                _health = remainingHealth;   
                nextUpdate = Mathf.FloorToInt(Time.time)+1;
            }

            if(DieCondition())
            {
                Die();
            }
        }

		[ContextMenu("Die")]
        public void Die()
		{
			StartCoroutine(DeathCoroutine());
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
