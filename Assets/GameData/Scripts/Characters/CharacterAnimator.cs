using UnityEngine;

namespace KeepItAlive.Characters
{
	[RequireComponent(typeof(Animator))]
	public sealed class CharacterAnimator : MonoBehaviour
	{
		private static readonly int _attackHash = Animator.StringToHash("Attack");
		private static readonly int _moveHash = Animator.StringToHash("Move");
		private static readonly int _damageHash = Animator.StringToHash("Damage");
		private static readonly int _deathHash = Animator.StringToHash("Death");

		private Animator _animator;
		
		public void SetAttack(bool isAttack)
		{
			_animator.SetBool(_attackHash, isAttack);
		}

		public void SetMove(bool isMove)
		{
			_animator.SetBool(_moveHash, isMove);
		}

		public void TriggerDeath()
		{
			_animator.SetTrigger(_deathHash);
		}

		public void TriggerDamage()
		{
			_animator.SetTrigger(_damageHash);
		}

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}
	}
}
