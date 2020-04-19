using KeepItAlive.Player;
using UnityEngine;

public class FuelInteractable : MonoBehaviour, IInteractable
{
    public FuelType FuelType = FuelType.Stone;
    
    string IInteractable.InteractText(Player player)
    {
        return "Pick up";
    }

    public string NoInteractText => "Can't pick up";

    public void Interact(Player player, InteractionType type)
    {
        if (type == InteractionType.PickUp)
        {
            player.Inventory.StoreItem(this);
        }
    }

    public bool CanPlayerInteract(Player player)
    {
        return gameObject.activeSelf;
    }
}