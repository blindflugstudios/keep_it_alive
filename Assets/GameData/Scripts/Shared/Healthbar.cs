using UnityEngine;

namespace KeepItAlive.Shared
{
    public class Healthbar : MonoBehaviour
    {
        [SerializeField] 
        private PlayerWorldSpaceUI _playerCanvas;

        [SerializeField]
        private GameObject _entity;

        private IEntity Entity => _entity.GetComponent<IEntity>();

        private float _initialHealth;

        void Start () 
        {
            _initialHealth = Entity.Health;
        }

        private void Update() 
        {
            if(_entity != null && Entity.Health > 0)
            {
                _playerCanvas.PlayerHealthLabelPrefab.DisplayText($"{_initialHealth} / {Entity.Health}", Color.black);
            }
        }
    }
}