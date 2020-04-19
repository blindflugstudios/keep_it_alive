using KeepItAlive.Camera;
using KeepItAlive.Characters;
using KeepItAlive.World;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KeepItAlive
{
    public sealed class GameManager : MonoBehaviour
	{
		[SerializeField] private CameraController _camera;
		[SerializeField] private Player.Player[] _playerPrefabs;
		[SerializeField] private GameObject _gameFinishedScreen;

		private Player.Player _player;
		private FinishPoint _finishPoint;

		private void OnDestinationReached()
		{
			_finishPoint.DestinationReached -= OnDestinationReached;
			_player.OnDestinationReached();
			_gameFinishedScreen.SetActive(true);
		}

		private void Start()
		{
			WorldGenerator.SpawnInfo spawnInfo = WorldGenerator.Instance.Generate();
			spawnInfo.FinishPoint.DestinationReached += OnDestinationReached;
			_finishPoint = spawnInfo.FinishPoint;
			SpawnPlayer(spawnInfo.StartPoint.transform.position);
		}

		private void SpawnPlayer(Vector3 position)
		{
			if (_playerPrefabs == null || _playerPrefabs.Length <= 0)
			{
				return;
			}
			
			if (_player != null)
			{
				_player.Dead -= OnPlayerDead;
			}
			_player = Instantiate(_playerPrefabs[Random.Range(0, _playerPrefabs.Length)], position, Quaternion.identity);
			_player.Dead += OnPlayerDead;
			_camera.PlayerMotor = _player.GetComponent<CharacterMotor>();
		}

		private void OnPlayerDead()
		{
			Vector3 position = _player.transform.position;
			SpawnPlayer(position);
		}
	}
}