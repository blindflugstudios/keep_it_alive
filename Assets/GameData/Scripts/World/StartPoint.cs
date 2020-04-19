using UnityEngine;

namespace KeepItAlive.World
{
    public sealed class StartPoint : MonoBehaviour
	{
		[SerializeField] private TorchFuelController _torch;

		public TorchFuelController Torch => _torch;
	}
}