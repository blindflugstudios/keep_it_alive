using UnityEngine;

namespace KeepItAlive.Shared
{
    public interface IEntity
    {
        float Health { get; }

        void Die();
    }
}

