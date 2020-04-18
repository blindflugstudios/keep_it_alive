using System;
using UnityEngine;

namespace KeepItAlive.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private float _health;

        [SerializeField]
        private string _name;

        [SerializeField]
        private GameObject _deathItemPrefab;
        
        public Enemy(float initialHealth, string name)
        {
            _health = initialHealth;
        }

        private void Die()
        {
            DeahAnimation();
            Destroy(gameObject);

            //Turns into other object
            Instantiate(_deathItemPrefab);
        }

        private void DeahAnimation()
        {
            //TODO: IMPLEMENT
            throw new NotImplementedException();
        }
    }
}