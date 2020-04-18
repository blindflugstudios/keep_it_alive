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

		[SerializeField] private AnimationCurve _deathAnimationCurve;

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
                _health = _damageManager.ApplyDamageReturnRemainingHealth(_health);   
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
			
			yield return DeathAnimation(); //Animation goes here
			
			Dead?.Invoke();
			
			Destroy(gameObject);
		}

		private IEnumerator DeathAnimation()
		{
			const float animationTime = 2f;
			var currentTime = 0f;
			while (currentTime < animationTime)
			{
				transform.Rotate(Vector3.forward, 360 * Time.deltaTime);
				transform.localScale = _deathAnimationCurve.Evaluate(currentTime / animationTime) * Vector3.one;
				yield return null;
				currentTime += Time.deltaTime;
			}			
		}

        private bool DieCondition()
        {
            //TODO: Discuss and probably more to come
            return _health < 0.0f;
        }
	}
}
