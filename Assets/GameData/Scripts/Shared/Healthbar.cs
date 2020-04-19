using UnityEngine;

namespace KeepItAlive.Shared
{
    public class Healthbar : MonoBehaviour
    {
        private const string DecimalFormat = "0.00";

        [SerializeField] 
        private PlayerWorldSpaceUI _playerCanvas;

        [SerializeField]
        private GameObject _entity;

        [SerializeField]
        private Transform _healthAnchor;

        [SerializeField]
        private Transform _radiationAnchor;

        [SerializeField]
        private Transform _freezeAnchor;

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
                    PlayerWorldSpaceUI.Instance.PlayerHealthLabelPrefab.SetAnchor(_healthAnchor);
                    PlayerWorldSpaceUI.Instance.PlayerHealthLabelPrefab.DisplayText($"{_initialHealth.ToString(DecimalFormat)} / {Entity.Health.ToString(DecimalFormat)}", Color.black);
                }
                else
                {
                     PlayerWorldSpaceUI.Instance.PlayerHealthLabelPrefab.DisplayText(string.Empty);
                }

                if(Entity.ReceivesRadiationDamage)
                {
                    PlayerWorldSpaceUI.Instance.PlayerRadiationDamageLabelPrefab.SetAnchor(_radiationAnchor);
                    PlayerWorldSpaceUI.Instance.PlayerRadiationDamageLabelPrefab.DisplayText("Receiving Radiation Damage!", Color.black);
                }
                else
                {
                    PlayerWorldSpaceUI.Instance.PlayerRadiationDamageLabelPrefab.DisplayText(string.Empty);
                }

                if(Entity.ReceivesFreezeDamage)
                {
                    PlayerWorldSpaceUI.Instance.PlayerFreezeDamageLabelPrefab.SetAnchor(_freezeAnchor);
                    PlayerWorldSpaceUI.Instance.PlayerFreezeDamageLabelPrefab.DisplayText("Receiving Freeze Damage!", Color.black);
                }
                else
                {
                    PlayerWorldSpaceUI.Instance.PlayerFreezeDamageLabelPrefab.DisplayText(string.Empty);
                }
            }
        }
    }
}