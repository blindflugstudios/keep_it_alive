using KeepItAlive.Camera;
using KeepItAlive.Characters;
using KeepItAlive.World;
using UnityEngine;

namespace KeepItAlive
{
    public sealed class GameManager : MonoBehaviour
	{
		[SerializeField] private CameraController _camera;
		[SerializeField] private Player.Player[] _playerPrefabs;

		private Player.Player _player;
		
        public void Start()
        {
            WorldGenerator.Instance.Generate();
			SpawnPlayer(Vector3.zero);
        }

		private void SpawnPlayer(Vector3 position)
		{
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