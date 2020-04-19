using UnityEngine;

namespace KeepItAlive.Shared
{
	[CreateAssetMenu(fileName = nameof(Configuration), menuName = "Game/Configuration", order = 0)]
	public class Configuration : ScriptableObject
	{
		[Header("ENVIRONMENTAL DAMAGE")]
		[SerializeField] private float _freezeDamagePerTick;
		[SerializeField] private float _radiationDamagePerTick;
		[Header("FUEL")]
		[SerializeField] private float _initialFuel;
		[SerializeField] private float _fuelReductionPerTick;

		public float FreezeDamagePerTick => _freezeDamagePerTick;
		public float RadiationDamagePerTick => _radiationDamagePerTick;
		public float InitialFuel => _initialFuel;
		public float FuelReductionPerTick => _fuelReductionPerTick;
	}
}
