using KeepItAlive.Characters;
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

		public event Action Dead;

        private DamageManager _damageManager;
        
		private PlayerInput _input;

        private float nextUpdate = 1.0f;

        public float Health => _health;

        public PlayerInventory Inventory;

		public void OnDestinationReached()
		{
			_input.enabled = false;
		}

		[ContextMenu("Die")]
		public void Die()
		{
			StartCoroutine(DeathCoroutine());
		}

        private void Awake()
        {
             _damageManager = new DamageManager(_configuration);
        }

		private void Start()
        {
            _damageManager.ReceivesFreezeDamage = true;

			_input = GetComponent<PlayerInput>();
			_input.enabled = false;
			_input.enabled = true;
		}

		private void Update()
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

		private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag(Tags.RadioactiveTag))
            {
                _damageManager.ReceivesRadiationDamage = true;
            }

            if (other.CompareTag(Tags.EnemyTag))
            {
                _health = _damageManager.ApplyEnemyDamageReturnRemainingHealth(_health);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.CompareTag(Tags.RadioactiveTag))
            {
                _damageManager.ReceivesRadiationDamage = false;
            }
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

        private void OnGUI() 
        {
            GUI.contentColor = Color.black;

            if(_damageManager.ReceivesFreezeDamage)
            {
                GUI.Box(new Rect(700, 50, 200, 20), "Receiving Freeze Damage!");
            }

            if(_damageManager.ReceivesRadiationDamage)
            {
                GUI.Box(new Rect(700, 80, 200, 20), "Receiving Radiation Damage!");
            }
        } 
	}
}
