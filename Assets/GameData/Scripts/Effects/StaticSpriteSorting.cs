using System.Collections;
using UnityEngine;

namespace KeepItAlive.Effects
{
    public sealed class StaticSpriteSorting : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            var sortOrder = 32767 - Mathf.RoundToInt((transform.position.y + 100.0f) * 100.0f);
            for (var i = 0; i < renderers.Length; i++)
            {
                renderers[i].sortingOrder = sortOrder;
            }
        }
    }
}