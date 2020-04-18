using KeepItAlive.World;
using UnityEngine;

namespace KeepItAlive
{
    public sealed class GameManager : MonoBehaviour
    {
        public void Start()
        {
            WorldGenerator.Instance.Generate();
        }
    }
}