using Interfaces;
using NUnit.Framework;
using UnityEngine;

public class ClickabledObjects : MonoBehaviour, iClickable
{
    [SerializeField] private Renderer obj;
    [SerializeField] private Material ogMat;
    [SerializeField] private Material clickedMat;

    public WinManager winManager;

    private bool isClicked = false;

    public void OnClick()
    {
        if (isClicked) return;
        isClicked = true;
        obj.material = clickedMat;
        winManager.FindItem();
    }
}
