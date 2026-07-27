using Interfaces;
using NUnit.Framework;
using UnityEngine;

public class ClickabledObjects : MonoBehaviour, iClickable
{
    [SerializeField] private Renderer obj;
    [SerializeField] private Material ogMat;
    [SerializeField] private Material clickedMat;

    private bool isClicked;

    public void OnClick()
    {
        isClicked = !isClicked;
        if (isClicked == true)
        {
            obj.material = clickedMat;
        }
        else
        {
            obj.material = ogMat;
        }
    }
}
