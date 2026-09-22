using System.Collections;
using Controller;
using Interfaces;
using UnityEngine;
using UnityEngine.VFX;

public class ClickabledObjects : MonoBehaviour, iClickable
{
    public MeshRenderer meshRenderer;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private Material meshMaterials;

    void Start()
    {
        if(meshRenderer != null)
            meshMaterials = meshRenderer.material;
    }

    [SerializeField] private Renderer obj;
    [SerializeField] private Material ogMat;
    [SerializeField] private Controller.InputController inputController;

    public WinManager winManager;

    private bool isClicked = false;

    public void OnClick()
    {
        if (isClicked) return;
        isClicked = true;

        inputController.SetFillColor(Color.green);
        StartCoroutine(DissolveCo());
        winManager.FindItem();
    }

    IEnumerator DissolveCo ()
    {
        if(VFXGraph != null)
        {
            VFXGraph.Play();
        }

        float counter = 0;

        while(meshMaterials.GetFloat("_DissolveAmount") < 1)
        {
            counter += dissolveRate;
            meshMaterials.SetFloat("_DissolveAmount", counter);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
