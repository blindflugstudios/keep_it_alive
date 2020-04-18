using KeepItAlive.Shared;
using UnityEngine;

namespace KeepItAlive.Player
{
    public class DamageManager
    {
        private bool receivesFreezeDamage;
        private bool receivesRadiationDamage;

        private EnvironmentalDamageConfiguration _environmentalDamageConfiguration;

        public DamageManager(EnvironmentalDamageConfiguration environmentalDamageConfiguration)
        {
            _environmentalDamageConfiguration = environmentalDamageConfiguration;
        }

        public float ApplyDamageReturnRemainingHealth(float health)
        {
            if(receivesFreezeDamage)
            {
                health = Freeze(health);
            }

            if(receivesRadiationDamage)
            {
                health = Radiate(health);
            }

            return health;
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