using System;
using UnityEngine;
using KeepItAlive.Shared;

namespace KeepItAlive.Enemies
{
    public class Enemy : MonoBehaviour, IEntity
    {
        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private float _health;

        [SerializeField]
        private GameObject _deathItemPrefab;

        public Sprite Sprite => _sprite;

        public float Health => _health;
        
        public Enemy(float initialHealth)
        {
            _health = initialHealth;
        }

        public void Die()
        {
            DeathAnimation();
            Destroy(gameObject);

            //Turns into other object
            Instantiate(_deathItemPrefab);
        }

        private void DeathAnimation()
        {
            //TODO: IMPLEMENT
            throw new NotImplementedException();
        }
    }
}