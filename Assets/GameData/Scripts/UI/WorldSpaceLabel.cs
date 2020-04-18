using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceLabel : MonoBehaviour
{
    [SerializeField] private Text _text;

    public void DisplayText(string text, Color color)
    {
        _text.text = text;
        _text.color = color;
    }

    public void DisplayText(string text)
    {
        DisplayText(text, Color.white);
    }

    public void SetAnchor(Vector3 position)
    {
        transform.position = position;
    }
}