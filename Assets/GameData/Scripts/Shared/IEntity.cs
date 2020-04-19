using UnityEngine;

namespace KeepItAlive.Shared
{
    public interface IEntity
    {
        float Health { get; }

        bool ReceivesRadiationDamage { get; }

        bool ReceivesFreezeDamage { get; }

        void Die();
    }
}

