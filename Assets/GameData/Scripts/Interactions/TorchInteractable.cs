using KeepItAlive.Effects;
using KeepItAlive.Player;
using UnityEngine;

public class TorchInteractable : MonoBehaviour, IInteractable
{
    private readonly string _pickUpString = "Pick Up";

    [SerializeField]
    private TorchFuelController _torchFuelController;
    [SerializeField] private SpriteRenderer[] _renderers;
    [SerializeField] private SpriteRenderer[] _reparentRenderers;
  
    public string InteractText(Player player)
    {
        return player.Inventory.HasTorch ? string.Empty : _pickUpString;
    }

    public string NoInteractText => string.Empty;

    public void Interact(Player player, InteractionType interactionType)
    {
        if (interactionType == InteractionType.Interact)
        {
            var fuelType = player.Inventory.HeldFuelType;
            player.Inventory.ConsumeHeldItem();
            
            //feed the torch
            switch(fuelType)
            {
                case FuelType.Stone:
                    _torchFuelController.AddFuel(10.0f);
                    break;
                case FuelType.Battery:
                    _torchFuelController.AddFuel(20.0f);
                    break;
                case FuelType.Stick:
                    _torchFuelController.AddFuel(5.0f);
                    break;
                case FuelType.MonsterCorpse:
                    _torchFuelController.AddFuel(25.0f);
                    break;
                case FuelType.PlayerCorpse:
                    _torchFuelController.AddFuel(50.0f);
                    break;
            }   
        }
        else if (interactionType == InteractionType.PickUp)
        {
            if (player.Inventory.HasTorch == false)
            {
                player.Inventory.HasTorch = true;
                player.Inventory.HeldTorch = this;
                transform.position = player.transform.position;
                transform.parent = player.transform;
                for (var i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].enabled = false;
                }
                for (var i = 0; i < _reparentRenderers.Length; i++)
                {
                    player.GetComponentInParent<DynamicSpriteSorting>().AddRenderer(_reparentRenderers[i]);
                }
            }            
        }
    }

    public void DropTorch(Player player)
    {
        if (player.Inventory.HasTorch)
        {
            player.Inventory.HasTorch = false;
            player.Inventory.HeldTorch = null;
            transform.position = player.transform.position;
            transform.parent = null;
            for (var i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].enabled = true;
            }
            for (var i = 0; i < _reparentRenderers.Length; i++)
            {
                player.GetComponentInParent<DynamicSpriteSorting>().RemoveRenderer(_reparentRenderers[i]);
            }
        }
    }

    public bool CanPlayerInteract(Player player)
    {
        return player.Inventory.HasTorch == false;
    }
}