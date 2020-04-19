using KeepItAlive.Shared;
using UnityEngine;

namespace KeepItAlive.Player
{
    public class DamageManager
    {
        public bool ReceivesFreezeDamage { get; set; }
        public bool ReceivesRadiationDamage { get; set; }

        private EnvironmentalDamageConfiguration _environmentalDamageConfiguration;

        public DamageManager(EnvironmentalDamageConfiguration environmentalDamageConfiguration)
        {
            _environmentalDamageConfiguration = environmentalDamageConfiguration;
        }

        public float ApplyDamageReturnRemainingHealth(float health)
        {
            if(ReceivesFreezeDamage)
            {
                health = Freeze(health);
            }

            if(ReceivesRadiationDamage)
            {
                health = Radiate(health);
            }

            return health;
        }

        public float ApplyEnemyDamageReturnRemainingHealth(float health)
        {
            //TODO: Make damage configurable per enemy
            return health - 15;
        }


        private float Freeze(float health)
        {
            return health - _environmentalDamageConfiguration.FreezeDamagePerTick;
        }

        private float Radiate(float health)
        {
           return health - _environmentalDamageConfiguration.RadiationDamagePerTick;
        }
    }
}