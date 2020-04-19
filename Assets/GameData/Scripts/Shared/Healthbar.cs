using UnityEngine;

namespace KeepItAlive.Shared
{
    public class Healthbar : MonoBehaviour
    {
        private const string DecimalFormat = "0.00";

        [SerializeField] 
        private HealthbarWorldUI _playerCanvas;

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
                    HealthbarWorldUI.Instance.PlayerHealthLabelPrefab.SetAnchor(_healthAnchor);
                    HealthbarWorldUI.Instance.PlayerHealthLabelPrefab.DisplayText($"{_initialHealth.ToString(DecimalFormat)} / {Entity.Health.ToString(DecimalFormat)}", Color.black);
                }
                else
                {
                    HealthbarWorldUI.Instance.PlayerHealthLabelPrefab.DisplayText(string.Empty);
                }

                if(Entity.ReceivesRadiationDamage)
                {
                    HealthbarWorldUI.Instance.PlayerRadiationDamageLabelPrefab.SetAnchor(_radiationAnchor);
                    HealthbarWorldUI.Instance.PlayerRadiationDamageLabelPrefab.DisplayText("Receiving Radiation Damage!", Color.black);
                }
                else
                {
                    HealthbarWorldUI.Instance.PlayerRadiationDamageLabelPrefab.DisplayText(string.Empty);
                }

                if(Entity.ReceivesFreezeDamage)
                {
                    HealthbarWorldUI.Instance.PlayerFreezeDamageLabelPrefab.SetAnchor(_freezeAnchor);
                    HealthbarWorldUI.Instance.PlayerFreezeDamageLabelPrefab.DisplayText("Receiving Freeze Damage!", Color.black);
                }
                else
                {
                    HealthbarWorldUI.Instance.PlayerFreezeDamageLabelPrefab.DisplayText(string.Empty);
                }
            }
        }
    }
}