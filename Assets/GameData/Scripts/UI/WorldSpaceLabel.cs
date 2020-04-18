using System;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceLabel : MonoBehaviour
{
    [SerializeField] private Text _text;
    private Transform _anchor;
    public void DisplayText(string text, Color color)
    {
        _text.text = text;
        _text.color = color;
    }

    public void DisplayText(string text)
    {
        DisplayText(text, Color.white);
    }

    public void SetAnchor(Transform anchor)
    {
        _anchor = anchor;
    }

    private void Update()
    {
        if (_anchor != null)
        {
            transform.position = _anchor.position;
        }
    }
}