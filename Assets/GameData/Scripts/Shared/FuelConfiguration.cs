using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelConfiguration : MonoBehaviour
{
    [SerializeField]
    private float _initialFuel;

    [SerializeField]
    private float _fuelReductionPerTick;
    
    public float InitialFuel => _initialFuel;
    
    public float FuelReductionPerTick => _fuelReductionPerTick;
}
