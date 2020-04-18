using KeepItAlive.Characters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KeepItAlive.Player
{
	[RequireComponent(typeof(CharacterMotor))]
	public class PlayerController : MonoBehaviour
	{
		private CharacterMotor _motor;

		public void OnMoveInput(InputAction.CallbackContext context)
		{
			var moveVector = context.ReadValue<Vector2>();
			_motor.Move(moveVector.x, moveVector.y);
		}
		
		private void Awake()
		{
			_motor = GetComponent<CharacterMotor>();
		}
	}
}
