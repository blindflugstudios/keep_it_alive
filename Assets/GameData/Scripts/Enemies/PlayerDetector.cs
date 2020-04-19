using KeepItAlive.Player;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private EnemyLogic _logic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            _logic.Target = other.transform;
        }
    }
}
