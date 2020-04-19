using KeepItAlive.Shared;
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

    private SpriteRenderer _shineRenderer;
    private LineRenderer _lineRenderer;
	private Vector3 _templeLocation;
    private float nextUpdate = 1.0f;
    private float _fuel;
    private Vector3 _eulers;

	public void SetDestinationCoords(Vector3 destinationCoords)
	{
		_templeLocation = destinationCoords;
	}
    
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
        _fuel = Mathf.Clamp(_fuel, MIN_FUEL, MAX_FUEL);

		Color color = GetFuelColor();
		_torchHead.color = color;
		_shineRenderer.color = color;

        AdjustSize();
    }

	private Color GetFuelColor()
	{
		float t = Mathf.InverseLerp(MIN_FUEL, MAX_FUEL, _fuel);
		return _fuelGradient.Evaluate(t);
	}

    private void AdjustSize()
    {
        transform.localScale = new Vector3(_fuel / 70, _fuel / 70 * 0.6f, 0);
    }    
}
