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
            if(_entity != null && Entity != null)
            {
                if(Entity.Health > 0)
                {
                    _playerCanvas.PlayerHealthLabelPrefab.SetAnchor(transform);
                    _playerCanvas.PlayerHealthLabelPrefab.DisplayText($"{_initialHealth} / {Entity.Health}", Color.black);
                }

                if(Entity.ReceivesFreezeDamage)
                {
                    _playerCanvas.PlayerFreezeDamageLabelPrefab.SetAnchor(transform);
                    _playerCanvas.PlayerFreezeDamageLabelPrefab.DisplayText("Receiving Freeze Damage!", Color.black);
                }

                if(Entity.ReceivesRadiationDamage)
                {
                    _playerCanvas.PlayerRadiationDamageLabelPrefab.SetAnchor(transform);
                    _playerCanvas.PlayerRadiationDamageLabelPrefab.DisplayText("Receiving Radiation Damage!", Color.black);
                }
            }
        }
    }
}