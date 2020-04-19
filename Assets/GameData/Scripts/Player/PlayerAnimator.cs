using UnityEngine;

namespace KeepItAlive.Player
{
	public class PlayerAnimator : MonoBehaviour
	{
		[SerializeField] private Animator _animator;

		public void SetMovement(float vertical, float horizontal)
		{
			_animator.SetBool("IsLeft", horizontal < 0);
			_animator.SetBool("IsMove", Mathf.Abs(horizontal + vertical) >= 0.01f);
			_animator.SetFloat("Horizontal", horizontal);
			_animator.SetFloat("Vertical", vertical);
		}
	}
}
