using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _jumpDistance;
    [SerializeField] private Vector2 _jumpFrequency;
    [SerializeField] private float _jumpTime;
    [SerializeField] private Transform _target;

    public bool ShouldNavigate;

    private void Start()
    {
        StartCoroutine(DodgeRoutine());
    }

    private void Update()
    {
        if (ShouldNavigate && _target != null) _agent.SetDestination(_target.position);
    }

    private IEnumerator DodgeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_jumpFrequency.x, _jumpFrequency.y));
            if (ShouldNavigate)
            {
                ShouldNavigate = false;
                yield return Jump();
                ShouldNavigate = true;
            }
        }
    }

    private IEnumerator Jump()
    {
        var timeElapsed = 0f;
        var startPos = transform.position;
        var endPos = startPos + (Random.value > 0.5f ? -1f : 1f) * _jumpDistance * transform.right;
        while (timeElapsed < _jumpTime)
        {
            timeElapsed += Time.deltaTime;
            var desiredPos = Vector3.Lerp(startPos, endPos, timeElapsed / _jumpTime);

            _agent.Warp(desiredPos);
            yield return null;
        }
    }
}