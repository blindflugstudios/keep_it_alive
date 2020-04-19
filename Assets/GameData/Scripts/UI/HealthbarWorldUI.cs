using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarWorldUI : MonoBehaviour
{
    [SerializeField]
    private WorldSpaceLabel _healthLabelPrefab;
    
    [SerializeField]
    private WorldSpaceLabel _radiationDamageLabelPrefab;
    
    [SerializeField]
    private WorldSpaceLabel _freezeDamageLabelPrefab;

    public WorldSpaceLabel HealthLabelPrefab => _healthLabelPrefab;
    
    public WorldSpaceLabel RadiationDamageLabelPrefab => _radiationDamageLabelPrefab;
    
    public WorldSpaceLabel FreezeDamageLabelPrefab => _freezeDamageLabelPrefab;

    public static HealthbarWorldUI Instance;
    
    private void Awake()
    {
        Instance = this;
    }

}
