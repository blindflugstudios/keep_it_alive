using KeepItAlive.Player;
using UnityEngine;

public class TorchInteractable : MonoBehaviour, IInteractable
{
    private readonly string _fuelString = "Fuel";
    private readonly string _pickUpString = "Pick Up";
    private readonly string _putDownString = "Put Down";

    public string InteractText(Player player)
    {
        var text = string.Empty;
        if (player.Inventory.HasItem) text = _fuelString+"\n OR \n";

        text += player.Inventory.HasTorch ? _putDownString : _pickUpString;

        return text;
    }

    public string NoInteractText => string.Empty;

    public void Interact(Player player, InteractionType interactionType)
    {
        if (interactionType == InteractionType.Interact)
        {
            var fuelType = player.Inventory.HeldFuelType;
            player.Inventory.ConsumeHeldItem();
            //feed the torch
        }
        else if (interactionType == InteractionType.PickUp)
        {
            if (player.Inventory.HasTorch)
            {
                player.Inventory.HasTorch = false;
                transform.parent = null;
            }
            else
            {
                player.Inventory.HasTorch = true;
                transform.parent = player.transform;
            }
            
        }
    }

    public bool CanPlayerInteract(Player player)
    {
        return true;
    }
}