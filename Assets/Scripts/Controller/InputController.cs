using Interfaces;
using UnityEngine;

namespace Controller
{
    public class InputController : MonoBehaviour
    {
        private Camera camera;
        private void Awake()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            // if (Application.platform == RuntimePlatform.WindowsEditor)
            // {
            //     if (!Input.GetMouseButtonDown(0)) return;
            //     Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            //     if (!Physics.Raycast(ray, out RaycastHit hitData)) return;
            //     HandleClickable(hitData);

            // }
        }

        private void HandleClickable(RaycastHit hitData)
        {
            // if (hitData.transform.TryGetComponent<iClickable>(out iClickable clickable))
            // {
            //     clickable.OnClick();   
            // }
        }
    }
}
