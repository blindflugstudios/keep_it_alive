using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TorchFuelController : MonoBehaviour
{
    [SerializeField]
    private FuelConfiguration _fuelConfiguration;

    private SpriteRenderer _shineRenderer;

    private float nextUpdate = 1.0f;

    private float _fuel;

    void Start()
    {
        _shineRenderer = GetComponent<SpriteRenderer>();
        _fuel = _fuelConfiguration.InitialFuel;

        AdjustSize();
    }
    
    void Update()
    {
        if(Time.time >= nextUpdate)
        {
            AddFuel(-_fuelConfiguration.FuelReductionPerTick);
            AdjustSize();
            nextUpdate = Mathf.FloorToInt(Time.time)+1;
        }
    }

    public void AddFuel(float amount)
    {
        _fuel += amount;
        _fuel = Mathf.Clamp(_fuel, 0.0f, 100.0f);

        AdjustSize();
    }

    private void AdjustSize()
    {
        transform.localScale = new Vector3(_fuel / 100, _fuel / 100, 0);
    }    
}
