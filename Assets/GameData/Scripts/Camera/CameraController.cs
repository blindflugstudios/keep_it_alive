using KeepItAlive.Characters;
using System;
using UnityEngine;

namespace KeepItAlive.Camera
{
	[RequireComponent(typeof(UnityEngine.Camera))]
	public class CameraController : MonoBehaviour
	{
		[Range(0f, 1f)]
		[SerializeField] private float _followRatio;
		[SerializeField] private float _followLerpRatio;
		[SerializeField] private float _returnLerpRatio;
		[SerializeField] private CharacterMotor _player;
		
		private CharacterMotor _playerMotor;
		private Transform _playerTransform;
		private UnityEngine.Camera _camera;
		private Vector3 _currentVelocity;

		private float _cameraZ;
		
		public CharacterMotor PlayerMotor
		{
			get => _playerMotor;
			set
			{
				_playerMotor = value;
				_playerTransform = _playerMotor.transform;
			}
		}

		private void LateUpdate()
		{
			if (_playerTransform == null)
			{
				return;
			}

			if (PlayerMotor.CurrentVelocity.magnitude > 0)
			{
				MoveToPoint(GetFollowPoint(), _followLerpRatio);
			}
			else
			{
				MoveToPoint(Vector3.zero, _returnLerpRatio);
			}
		}

		private Vector3 GetFollowPoint()
		{
			Vector2 velocity = PlayerMotor.CurrentVelocity;
			float horizontalSize = _camera.orthographicSize * _camera.aspect;
			float verticalSize = _camera.orthographicSize;
			float x = Math.Sign(velocity.x) * horizontalSize * _followRatio;
			float y = Math.Sign(velocity.y) * verticalSize * _followRatio;
			
			return new Vector3(x, y);
		}

		private void MoveToPoint(Vector3 offset, float lerpRatio)
		{
			Vector3 GetTargetPosition()
			{
				Vector3 target = _playerTransform.position + offset;
				target.z = _cameraZ;
				return target;
			}
			
			Vector3 currentPosition = transform.position;
			Vector3 targetPosition = GetTargetPosition();
			currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref _currentVelocity, lerpRatio);
			transform.position = currentPosition;
		}

		private void Awake()
		{
			_camera = GetComponent<UnityEngine.Camera>();
			_cameraZ = transform.position.z;
		}
	}
}
