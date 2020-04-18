using UnityEngine;

public class WorldSpaceUi : MonoBehaviour
{
    [SerializeField] private WorldSpaceLabel _labelPrefab;

    public WorldSpaceLabel GetLabel()
    {
        return Instantiate(_labelPrefab, transform);
    }
}