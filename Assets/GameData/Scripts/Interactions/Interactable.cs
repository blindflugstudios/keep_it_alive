using KeepItAlive.Player;

public interface IInteractable
{
    string InteractText(Player player);
    string NoInteractText { get; }
    void Interact(Player player, InteractionType interactionType);
    bool CanPlayerInteract(Player player);
}

/*public interface IPickupable
{
    string PickUpText { get; }
    string NoPickUpText { get; }
    void PickUp(Player player);
    bool CanPlayerPickUp(Player player);
}*/