using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KeepItAlive.Effects
{
    public sealed class DynamicSpriteSorting : MonoBehaviour
    {
        private List<SpriteRenderer> _renderers;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        }
        
        private void Update()
        {
            var sortOrder = 32767 - Mathf.RoundToInt((transform.position.y + 100.0f) * 100.0f);
            for (var i = 0; i < _renderers.Count; i++)
            {
                _renderers[i].sortingOrder = sortOrder;
            }
        }

        public void AddRenderer(SpriteRenderer rend)
        {
            _renderers.Add(rend);
        }

        public void RemoveRenderer(SpriteRenderer rend)
        {
            _renderers.Remove(rend);
        }
    }
}