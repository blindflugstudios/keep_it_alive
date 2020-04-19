using System;
using KeepItAlive.Shared;
using KeepItAlive.World;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class TorchFuelController : MonoBehaviour
{
	private const float MIN_FUEL = 10f;
	private const float MAX_FUEL = 110f;
	[SerializeField] private Configuration _fuelConfiguration;
	[SerializeField] private SpriteRenderer _torchHead;
	[SerializeField] private Gradient _fuelGradient;
    [SerializeField] private float _beamLenght;
    [SerializeField] private SpriteRenderer _shineRenderer;
    [SerializeField] private float _range;
    
    private LineRenderer _lineRenderer;
	private Vector3 _templeLocation;
    private float nextUpdate = 1.0f;
    private float _fuel;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        
        _fuel = _fuelConfiguration.InitialFuel;
        _lineRenderer.positionCount = 2;
        AdjustSize();
        _templeLocation = WorldGenerator.Instance.FinishPoint.transform.position;
    }
    
    void Update()
    {
        ShowDirectionOfTemple(_fuel >= 100.0f);

        if(Time.time >= nextUpdate)
        {
            AddFuel(-_fuelConfiguration.FuelReductionPerTick);
            nextUpdate = Mathf.FloorToInt(Time.time)+1;
        }
    }

    private void ShowDirectionOfTemple(bool show)
    {
        _lineRenderer.enabled = show;
        if (show)
        {
            var dirNormalized = (_templeLocation - transform.position).normalized;
            _lineRenderer.SetPositions(new[]
            {
                transform.position,
                transform.position + dirNormalized * _beamLenght
            });
        }
    }

    public void AddFuel(float amount)
    {
        _fuel += amount;
        _fuel = Mathf.Clamp(_fuel, MIN_FUEL, MAX_FUEL);

		Color color = GetFuelColor();
		_torchHead.color = color;
		_shineRenderer.color = new Color(color.r, color.g, color.b, 0.15f);

        AdjustSize();
    }

	private Color GetFuelColor()
	{
		float t = Mathf.InverseLerp(MIN_FUEL, MAX_FUEL, _fuel);
		return _fuelGradient.Evaluate(t);
	}

    private void AdjustSize()
    {
        _shineRenderer.transform.localScale = new Vector3(_fuel * _range, _fuel *_range * 0.5f, 0);
    }    
}
