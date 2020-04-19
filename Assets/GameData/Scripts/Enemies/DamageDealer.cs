using System.Collections;
using System.Collections.Generic;
using KeepItAlive.Player;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private float _collisionOffset;
    [SerializeField] private float _collisionDelay;
    [SerializeField] private float _collisionRadius;
    [SerializeField] private float _damage;
    [SerializeField] private ParticleSystem _bloodParticles;
    [SerializeField] private int _particlesToEmit;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_collisionDelay);
        var hits = Physics2D.CircleCastAll(transform.position + transform.forward * _collisionOffset, _collisionRadius,
            transform.forward, 0.01f);
        foreach (var hit in hits)
        {
            var player = hit.transform.GetComponent<Player>();
            if (player)
            {
                player.DealDamage(_damage);
                _bloodParticles.Emit(_particlesToEmit);
                yield break;
            }
        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
