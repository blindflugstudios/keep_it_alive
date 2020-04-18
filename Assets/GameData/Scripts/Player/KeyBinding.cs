using UnityEngine;

[CreateAssetMenu(fileName = "KeyBinding", order = 0, menuName = "Custom/KeyBindings")]
public class KeyBinding : ScriptableObject
{
    [SerializeField] private KeyCode _interactionButton;
    [SerializeField] private KeyCode _pickUpButton;
    [SerializeField] private KeyCode _combatButton;
    public KeyCode InteractionButton => _interactionButton;
    public KeyCode PickUpButton => _pickUpButton;
    public KeyCode CombatButton => _combatButton;
}