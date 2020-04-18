using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class TorchFuelController : MonoBehaviour
{
    [SerializeField]
    private FuelConfiguration _fuelConfiguration;

    private SpriteRenderer _shineRenderer;

    private LineRenderer _lineRenderer;

    private float nextUpdate = 1.0f;

    private float _fuel;

    void Start()
    {
        _shineRenderer = GetComponent<SpriteRenderer>();
        _lineRenderer = GetComponent<LineRenderer>();
        
        _fuel = _fuelConfiguration.InitialFuel;

        AdjustSize();
    }
    
    void Update()
    {
        if(_fuel >= 100.0f)
        {
            ShowDirectionOfTemple(new Vector2(20.0f, 20.0f));
        }

        if(Time.time >= nextUpdate)
        {
            AddFuel(-_fuelConfiguration.FuelReductionPerTick);
            AdjustSize();
            nextUpdate = Mathf.FloorToInt(Time.time)+1;
        }
    }

    private void ShowDirectionOfTemple(Vector2 templeLocation)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0,0,Mathf.Asin(templeLocation.y) * Mathf.Rad2Deg * (templeLocation.x < 0? -1: 1)));
        
        _lineRenderer.SetPositions(new Vector3[] 
        {
             transform.position, 
             new Vector3(templeLocation.x, templeLocation.y, 0)
        });
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
