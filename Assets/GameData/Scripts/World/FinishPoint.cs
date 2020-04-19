using System;
using UnityEngine;

namespace KeepItAlive.World
{
    public sealed class FinishPoint : MonoBehaviour
	{
		public event Action DestinationReached;

		private bool _isReached;
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (_isReached == false && other.gameObject.CompareTag(Tags.PlayerTag))
			{
				DestinationReached?.Invoke();
				_isReached = true;
			}
		}
	}
}