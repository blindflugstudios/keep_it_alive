using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldSpaceUI : MonoBehaviour
{
    [SerializeField]
    private WorldSpaceLabel _playerHealthLabelPrefab;
    
    [SerializeField]
    private WorldSpaceLabel _playerRadiationDamageLabelPrefab;
    
    [SerializeField]
    private WorldSpaceLabel _playerFreezeDamageLabelPrefab;

    public WorldSpaceLabel PlayerHealthLabelPrefab => _playerHealthLabelPrefab;
    
    public WorldSpaceLabel PlayerRadiationDamageLabelPrefab => _playerRadiationDamageLabelPrefab;
    
    public WorldSpaceLabel PlayerFreezeDamageLabelPrefab => _playerFreezeDamageLabelPrefab;

    public static PlayerWorldSpaceUI Instance;
    
    private void Awake()
    {
        Instance = this;
    }

}
