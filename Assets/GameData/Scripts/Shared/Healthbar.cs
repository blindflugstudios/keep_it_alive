using UnityEngine;

namespace KeepItAlive.Shared
{
    public class Healthbar : MonoBehaviour
    {
        private const string DecimalFormat = "0.00";

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
                    HealthbarWorldUI.Instance.HealthLabelPrefab.SetAnchor(_healthAnchor);
                    HealthbarWorldUI.Instance.HealthLabelPrefab.DisplayText($"{_initialHealth.ToString(DecimalFormat)} / {Entity.Health.ToString(DecimalFormat)}", Color.black);
                }
                else
                {
                    HealthbarWorldUI.Instance.HealthLabelPrefab.DisplayText(string.Empty);
                }

                if(Entity.ReceivesRadiationDamage)
                {
                    HealthbarWorldUI.Instance.RadiationDamageLabelPrefab.SetAnchor(_radiationAnchor);
                    HealthbarWorldUI.Instance.RadiationDamageLabelPrefab.DisplayText("Receiving Radiation Damage!", Color.black);
                }
                else
                {
                    HealthbarWorldUI.Instance.RadiationDamageLabelPrefab.DisplayText(string.Empty);
                }

                if(Entity.ReceivesFreezeDamage)
                {
                    HealthbarWorldUI.Instance.FreezeDamageLabelPrefab.SetAnchor(_freezeAnchor);
                    HealthbarWorldUI.Instance.FreezeDamageLabelPrefab.DisplayText("Receiving Freeze Damage!", Color.black);
                }
                else
                {
                    HealthbarWorldUI.Instance.FreezeDamageLabelPrefab.DisplayText(string.Empty);
                }
            }
        }
    }
}