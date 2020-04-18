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

        public float Health => _health;

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