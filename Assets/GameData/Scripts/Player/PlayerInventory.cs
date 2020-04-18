using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private IInteractable _currentItem;
    public bool HasItem => _currentItem != null;

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
}