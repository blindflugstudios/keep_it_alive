using UnityEngine;

namespace KeepItAlive.World
{
    public sealed class StartPoint : MonoBehaviour
	{
		[SerializeField] private TorchInteractable _torch;

		public TorchInteractable Torch => _torch;
	}
}