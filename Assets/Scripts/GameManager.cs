using KeepItAlive.World;
using UnityEngine;

namespace DefaultNamespace
{
    public sealed class GameManager : MonoBehaviour
    {
        public void Start()
        {
            WorldGenerator.Instance.Generate();
        }
    }
}