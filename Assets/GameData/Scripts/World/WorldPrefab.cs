using UnityEngine;

namespace KeepItAlive.World
{
    public class WorldPrefab : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;

        public (Vector2, Vector2) GetSpawnBox()
        {
            return (_collider.bounds.center, _collider.bounds.extents);
        }
        
        #if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            var color = Gizmos.color;
            Gizmos.color = new Color(1.0f, .0f, .0f, 0.2f);
            Gizmos.DrawWireCube(transform.position + new Vector3(_collider.offset.x, _collider.offset.y, .0f), _collider.bounds.extents);
            Gizmos.color = color;
        }
        #endif
    }
}