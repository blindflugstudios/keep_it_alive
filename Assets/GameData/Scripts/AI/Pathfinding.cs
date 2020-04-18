using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private NavMeshAgent _agent;
    
    void Update()
    {
        if (_target != null)
        {
            _agent.SetDestination(_target.position);
        }
    }
}
