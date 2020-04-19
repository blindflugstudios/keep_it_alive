using UnityEngine;

namespace KeepItAlive.Characters
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class CharacterMotor : MonoBehaviour
	{
		private const float SKIN_WIDTH = 0.1f;
		
		[SerializeField] private float _moveSpeed;
		[Range(0f, 1f)]
		[SerializeField] private float _depthMovementScale;
		[SerializeField] private LayerMask _obstacleLayer;
		[SerializeField] private CharacterAnimator _animator;
		
		private BoxCollider2D _collider;
		private Transform _transform;

		private Vector2 _currentVelocity;
		private RaycastHit2D[] _hits;
		private Vector2[] _raycastOrigins;

		public Vector2 CurrentVelocity => _currentVelocity;

		public void Move(float x, float y)
		{
			_currentVelocity = _moveSpeed * new Vector2(x, y * _depthMovementScale);
		}

		private void Update()
		{
			UpdateRaycastOrigins();
			MoveOneFrame(Time.deltaTime);
			
			_animator?.SetMove(_currentVelocity.magnitude > 0f);
		}

		private void MoveOneFrame(float deltaTime)
		{
			Vector2 frameVelocity = _currentVelocity * deltaTime;

			MakeRaycasts(frameVelocity);
			ResolveCollisions(ref frameVelocity);

			_transform.Translate(frameVelocity.x, frameVelocity.y, 0f);
		}
		
		private void MakeRaycasts(Vector2 frameVelocity)
		{
			for (int i = 0; i < _hits.Length; i++)
			{
				_hits[i] = default;
			}

			//0 and 1 are top-left-top and top-right-top 
			//2 and 3 are bottom-left-down and bottom-right-down 
			//4 and 5 are top-left-left and bottom-left-left
			//6 and 7 are top-right-right and bottom-right-right
			Vector2 direction;
			if (frameVelocity.x > 0)
			{
				direction = new Vector2(frameVelocity.x, 0f);
				_hits[6] = Raycast(direction, 1);
				_hits[7] = Raycast(direction, 2);
			}
			else if (frameVelocity.x < 0)
			{
				direction = new Vector2(frameVelocity.x, 0f);
				_hits[4] = Raycast(direction, 3);
				_hits[5] = Raycast(direction, 0);
			}

			if (frameVelocity.y > 0)
			{
				direction = new Vector2(0f, frameVelocity.y);
				_hits[0] = Raycast(direction, 0);
				_hits[1] = Raycast(direction, 1);
			}
			else if (frameVelocity.y < 0)
			{
				direction = new Vector2(0f, frameVelocity.y);
				_hits[2] = Raycast(direction, 2);
				_hits[3] = Raycast(direction, 3);
			}
		}
		
		private void ResolveCollisions(ref Vector2 frameVelocity)
		{
			float topCollisionDistance = GetMinCollisionDistance(0, 1);
			float bottomCollisionDistance = GetMinCollisionDistance(2, 3);
			if (topCollisionDistance >= 0)
			{
				frameVelocity.y = topCollisionDistance;
			}
			else if (bottomCollisionDistance >= 0)
			{
				frameVelocity.y = -bottomCollisionDistance;
			}

			float leftCollisionDistance = GetMinCollisionDistance(4, 5);
			float rightCollisionDistance = GetMinCollisionDistance(6, 7);
			if (leftCollisionDistance >= 0)
			{
				frameVelocity.x = -leftCollisionDistance;
			}
			else if (rightCollisionDistance >= 0)
			{
				frameVelocity.x = rightCollisionDistance;
			}
		}

		private float GetMinCollisionDistance(int hitIndex1, int hitIndex2)
		{
			float distance1 = _hits[hitIndex1].collider != null ? Mathf.Max(_hits[hitIndex1].distance - SKIN_WIDTH, 0f) : -1f;
			float distance2 = _hits[hitIndex2].collider != null ? Mathf.Max(_hits[hitIndex2].distance - SKIN_WIDTH, 0f) : -1f;
			
			return distance1 * distance2 <= 0 ? Mathf.Max(distance1, distance2) : Mathf.Min(distance1, distance2);
		}

		private void UpdateRaycastOrigins()
		{
			Vector3 position = _transform.position;
			Vector2 center = new Vector2(position.x, position.y) + _collider.offset;
			Vector2 size = _collider.size * 0.5f - SKIN_WIDTH * Vector2.one;
			_raycastOrigins[0] = center + new Vector2(-size.x, size.y);
			_raycastOrigins[1] = center + new Vector2(size.x, size.y);
			_raycastOrigins[2] = center + new Vector2(size.x, -size.y);
			_raycastOrigins[3] = center + new Vector2(-size.x, -size.y);
		}

		private RaycastHit2D Raycast(Vector2 direction, int originIndex)
		{
			Debug.DrawRay(_raycastOrigins[originIndex], (direction.magnitude + SKIN_WIDTH) * direction.normalized, Color.red);
			return Physics2D.Raycast(_raycastOrigins[originIndex], direction.normalized, direction.magnitude + SKIN_WIDTH, _obstacleLayer);
		}
		
		private void Awake()
		{
			_transform = transform;
			_collider = GetComponent<BoxCollider2D>();
			_hits = new RaycastHit2D[8];
			_raycastOrigins = new Vector2[4];
		}
	}
}
