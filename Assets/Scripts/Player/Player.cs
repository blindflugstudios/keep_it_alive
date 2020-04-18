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
        
        public Player(float initialHealth)
        {
            _health = initialHealth;
        }

        public void Die()
        {
            DeathAnimation();
            Destroy(gameObject);
        }

        private void DeathAnimation()
        {
            //TODO: IMPLEMENT
            throw new NotImplementedException();
        }
    }
}
