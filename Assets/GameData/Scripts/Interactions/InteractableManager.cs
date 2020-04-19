using System.Collections.Generic;
using System.Linq;
using KeepItAlive.Player;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private List<InteractableData> _interactablesInRange = new List<InteractableData>();
    [SerializeField] private KeyBinding _keyBinding;
    [SerializeField] private Player _player;

    private InteractableData ClosestInteractable
    {
        get
        {
            _interactablesInRange = _interactablesInRange.OrderBy(i => i.DistanceToPlayer).ToList();
            return _interactablesInRange.FirstOrDefault(a => a.InteractionPossible);
        }
    }

    private void Update()
    {
        foreach (var interactableData in _interactablesInRange) interactableData.Update();

        var closestInteractable = ClosestInteractable;
        if (closestInteractable != null)
        {
            closestInteractable.DisplayInteractionText();
            if (Input.GetKeyDown(_keyBinding.InteractionButton))
            {
                closestInteractable.Interactable.Interact(_player, InteractionType.Interact);
            }
            else if (Input.GetKeyDown(_keyBinding.PickUpButton))
            {
                closestInteractable.Interactable.Interact(_player, InteractionType.PickUp);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.isTrigger)
        {
            var interactable = col.GetComponent<IInteractable>();
            if (interactable != null && _interactablesInRange.Any(i => i.Interactable == interactable) == false)
            {
                var interactionData = new InteractableData(interactable, WorldSpaceUi.Instance.GetLabel(), _player);
                _interactablesInRange.Add(interactionData);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.isTrigger)
        {
            var interactable = col.GetComponent<IInteractable>();
            if (interactable != null && _interactablesInRange.Count > 0)
            {
                var data = _interactablesInRange.FirstOrDefault(i => i.Interactable == interactable);
                if (data != null)
                {
                    data.Destroy();
                    _interactablesInRange.Remove(data);
                }
            }
        }
    }

    private class InteractableData
    {
        private readonly WorldSpaceLabel _label;
        private readonly Player _player;
        public float DistanceToPlayer;
        public readonly IInteractable Interactable;

        public InteractableData(IInteractable interactable, WorldSpaceLabel label, Player player)
        {
            Interactable = interactable;
            _player = player;
            _label = label;
            _label.SetAnchor((interactable as MonoBehaviour).transform);
            Update();
        }

        public bool InteractionPossible => Interactable.CanPlayerInteract(_player);

        public void Update()
        {
            if (Interactable != null)
            {
                DistanceToPlayer = Vector3.Distance((Interactable as MonoBehaviour).transform.position,
                    _player.transform.position);
                if (InteractionPossible == false)
                    _label.DisplayText(Interactable.NoInteractText, Color.red);
                else
                    _label.DisplayText("");
            }
        }

        public void DisplayInteractionText()
        {
            _label.DisplayText(Interactable.InteractText(_player), Color.white);
        }

        public void Destroy()
        {
            Object.Destroy(_label.gameObject);
        }
    }
}
public enum InteractionType
{
    PickUp,
    Interact
}