﻿using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private IInteractable _currentItem;
    public bool HasItem => _currentItem != null;

    public bool HasTorch;

    public FuelType HeldFuelType =>
        (_currentItem as FuelInteractable)?.FuelType ?? FuelType.None;

    public TorchInteractable HeldTorch { get; set; }

    public void StoreItem(IInteractable item)
    {
        if (HasItem) DropItem();
        PickUpItem(item);
    }

    private void DropItem()
    {
        var itemAsMono = _currentItem as MonoBehaviour;
        itemAsMono.transform.position = transform.position;
        itemAsMono.gameObject.SetActive(true);
    }

    private void PickUpItem(IInteractable item)
    {
        _currentItem = item;
        (_currentItem as MonoBehaviour).gameObject.SetActive(false);

        //display info abut the item on the screen
    }
    
    public void ConsumeHeldItem()
    {
        //update ui info about held item
        if(_currentItem != null)
        {
            Destroy((_currentItem as MonoBehaviour).gameObject);
            _currentItem = null;
        }
    }
}

public enum FuelType
{
    None,
    Stone,
    Stick,
    Battery,
    MonsterCorpse,
    PlayerCorpse
}