using KeepItAlive.Shared;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class TorchFuelController : MonoBehaviour
{
    
	[SerializeField] private Configuration _fuelConfiguration;
    [SerializeField] private float _beamLenght;
    [SerializeField] private Vector3 _templeLocation;

    private SpriteRenderer _shineRenderer;
    private LineRenderer _lineRenderer;
    private float nextUpdate = 1.0f;
    private float _fuel;
    private Vector3 _eulers;
    
    void Start()
    {
        _eulers = Vector3.up*90f;
        _shineRenderer = GetComponent<SpriteRenderer>();
        _lineRenderer = GetComponent<LineRenderer>();
        
        _fuel = _fuelConfiguration.InitialFuel;
        _lineRenderer.positionCount = 2;
        AdjustSize();
    }
    
    void Update()
    {
        ShowDirectionOfTemple(_fuel >= 100.0f);

        if(Time.time >= nextUpdate)
        {
            AddFuel(-_fuelConfiguration.FuelReductionPerTick);
            AdjustSize();
            nextUpdate = Mathf.FloorToInt(Time.time)+1;
        }
    }

    private void ShowDirectionOfTemple(bool show)
    {
        _lineRenderer.enabled = show;
        if (show)
        {
            var dirNormalized = (_templeLocation - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dirNormalized) *
                                 Quaternion.Euler(_eulers);
            _lineRenderer.SetPositions(new Vector3[]
            {
                transform.position,
                transform.position + dirNormalized * _beamLenght
            });
        }
    }

    public void AddFuel(float amount)
    {
        _fuel += amount;
        _fuel = Mathf.Clamp(_fuel, 10.0f, 110.0f);

        AdjustSize();
    }

    private void AdjustSize()
    {
        transform.localScale = new Vector3(_fuel / 100, _fuel / 100, 0);
    }    
}
