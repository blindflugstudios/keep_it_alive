using UnityEngine;

namespace KeepItAlive.Shared
{
    public interface IEntity
    {
        //Intent: Force implementing classes to have this fields
        Sprite Sprite { get; }
        float Health { get; }

        void Die();
    }
}

