using UnityEngine;

namespace KeepItAlive.World
{
    [RequireComponent(typeof(Collider2D))]
    public class WorldPrefab : MonoBehaviour
    {
        private Collider2D _collider;

        public void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public (Vector2, Vector2) GetSpawnBox()
        {
            return (new Vector2(transform.position.x, transform.position.y) + _collider.offset,
                _collider.bounds.extents);
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