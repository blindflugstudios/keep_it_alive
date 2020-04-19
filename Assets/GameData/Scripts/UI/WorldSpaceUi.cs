using System;
using UnityEngine;

public class WorldSpaceUi : MonoBehaviour
{
    [SerializeField] private WorldSpaceLabel _labelPrefab;

    public static WorldSpaceUi Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    public WorldSpaceLabel GetLabel()
    {
        return Instantiate(_labelPrefab, transform);
    }
}