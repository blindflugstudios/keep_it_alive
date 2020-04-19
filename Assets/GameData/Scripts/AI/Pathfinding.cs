using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private float _additionalRotationSpeed;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _jumpDistance;
    [SerializeField] private Vector2 _jumpFrequency;
    [SerializeField] private float _jumpTime;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _target;

    public bool ShouldNavigate;

    private void Start()
    {
        StartCoroutine(DodgeRoutine());
    }

    private void Update()
    {
        if (ShouldNavigate && _target != null)
        {
            _agent.SetDestination(_target.position);
            RotateTowardsTarget();
        }
    }

    private void RotateTowardsTarget()
    {
        var lookrotation = _agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation),
            _additionalRotationSpeed * Time.deltaTime);
    }

    private IEnumerator DodgeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_jumpFrequency.x, _jumpFrequency.y));
            if (ShouldNavigate)
            {
                var dist = Vector2.Distance(transform.position, _target.position);
                var hit = Physics2D.Raycast(transform.position, (_target.position - transform.position).normalized,
                    dist, _layerMask.value);

                if (hit.collider == null)
                {
                    var dot = Vector3.Dot(transform.forward, (_target.position - transform.position).normalized);
                    if (dot > 0.5f)
                    {
                        ShouldNavigate = false;
                        yield return Jump();
                        ShouldNavigate = true;
                    }
                }
            }
        }
    }

    private IEnumerator Jump()
    {
        var timeElapsed = 0f;
        var startPos = transform.position;
        var endPos = startPos + (Random.value > 0.5f ? -1f : 1f) * _jumpDistance * transform.right;
        endPos += 2f * _jumpDistance*(_target.position - transform.position).normalized;
        var dir = (endPos - startPos).normalized;
        var dist = Vector3.Distance(startPos, endPos);
        var hit = Physics2D.Raycast(startPos, dir, dist, _layerMask.value);
        if (hit.collider != null && hit.collider.isTrigger == false)
            endPos = new Vector3(hit.point.x, hit.point.y, transform.position.z);
        var jumpTime = _jumpTime * (Vector3.Distance(startPos, endPos) / _jumpDistance);

        while (timeElapsed < jumpTime)
        {
            timeElapsed += Time.deltaTime;
            var desiredPos = Vector3.Lerp(startPos, endPos, timeElapsed / jumpTime);

            _agent.Warp(desiredPos);
            yield return null;
        }
    }
}