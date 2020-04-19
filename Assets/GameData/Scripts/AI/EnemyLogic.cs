using System.Collections;
using KeepItAlive.Characters;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _jumpDistance;
    [SerializeField] private Vector2 _jumpFrequency;
    [SerializeField] private float _jumpTime;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _target;
    [SerializeField] private CharacterAnimator _animator;
    [SerializeField] private Attacker _attacker;
    private Transform _visual;
    public bool ShouldNavigate;
    
    
    private void Start()
    {
        StartCoroutine(DodgeRoutine());
        _visual = transform.GetChild(0);
    }

    private void Update()
    {
        var shouldMove = ShouldNavigate && _target != null;
        _animator.SetMove(shouldMove);
        if (shouldMove)
        {
            _agent.SetDestination(_target.position);
            var visualScale = _visual.localScale;
            visualScale.x = Mathf.Abs(visualScale.x) * -Mathf.Sign(_target.position.x - transform.position.x);
            _visual.localScale = visualScale;
            if (_attacker.Attack(_target))
            {
                _animator.SetAttack();
                StartCoroutine(DelayMovement(0.4f));
            }
        }
    }

    private IEnumerator DelayMovement(float seconds)
    {
        ShouldNavigate = false;
        _agent.isStopped = true;
        yield return new WaitForSeconds(seconds);
        _agent.isStopped = false;
        ShouldNavigate = true;
    }

    private IEnumerator DodgeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_jumpFrequency.x, _jumpFrequency.y));
            if (ShouldNavigate && _target!= null)
            {
                var dist = Vector2.Distance(transform.position, _target.position);
                var hit = Physics2D.Raycast(transform.position, (_target.position - transform.position).normalized,
                    dist, _layerMask.value);

                if (hit.collider == null)
                {
                    var lookRotation = (_agent.steeringTarget - transform.position).normalized;
                    var dot = Vector3.Dot(lookRotation, (_target.position - transform.position).normalized);
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
        var dirToEnemy = (_target.position - transform.position).normalized;
        var endPos = startPos + (Random.value > 0.5f ? -1f : 1f) * _jumpDistance *Vector3.Cross(Vector3.back, dirToEnemy);
        endPos += 2f * _jumpDistance*(_target.position - transform.position).normalized;
        var dir = (endPos - startPos).normalized;
        var dist = Vector3.Distance(startPos, endPos);
        var hit = Physics2D.Raycast(startPos, dir, dist, _layerMask.value);
        if (hit.collider != null && hit.collider.isTrigger == false)
            endPos = new Vector3(hit.point.x, hit.point.y, transform.position.z);
        var jumpTime = _jumpTime * (Vector3.Distance(startPos, endPos) / _jumpDistance);
        
        _animator.TriggerDash();
        while (timeElapsed < jumpTime)
        {
            timeElapsed += Time.deltaTime;
            var desiredPos = Vector3.Lerp(startPos, endPos, timeElapsed / jumpTime);
            _agent.Warp(desiredPos);
            yield return null;
        }
    }
}