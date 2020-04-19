using Unity.Mathematics;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private GameObject _visualPrefab;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackDistance;

    private float _currentCd = -1f;
    
    public bool Attack(Transform target)
    {
        var dist = Vector2.Distance(transform.position, target.position);
        if ( _currentCd <= 0f && dist < _attackDistance)
        {
            var dir = (target.position - transform.position).normalized;
            var scale = Vector3.one;
            scale.x *= Mathf.Sign(transform.GetChild(0).localScale.x);
            var prefab =Instantiate(_visualPrefab, transform.position, Quaternion.LookRotation(dir,Vector3.back));
            prefab.transform.localScale = scale;
            _currentCd = _attackCooldown;
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (_currentCd > 0f)
        {
            _currentCd -= Time.deltaTime;
        }
    }
}
