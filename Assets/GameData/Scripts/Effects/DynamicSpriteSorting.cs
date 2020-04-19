using UnityEngine;

namespace KeepItAlive.Effects
{
    public sealed class DynamicSpriteSorting : MonoBehaviour
    {
        private SpriteRenderer[] _renderers;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }
        
        private void Update()
        {
            var sortOrder = 32767 - Mathf.RoundToInt((transform.position.y + 100.0f) * 100.0f);
            for (var i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].sortingOrder = sortOrder;
            }
        }
    }
}