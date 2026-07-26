using System.Numerics;
using System.Reflection.Metadata;
using DG.Tweening;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Map Bounds")]
        [SerializeField] private float mapBoundsMinX = -1f;
        [SerializeField] private float mapBoundsMaxX = 0f;
        [SerializeField] private float mapBoundsMinZ = -1f;
        [SerializeField] private float mapBoundsMaxZ = 0f;
        [SerializeField] private float camSnapBackDuration = 0.75f;

        [Header("Camera Zoom")]
        [SerializeField, UnityEngine.RangeAttribute(1f, 10f)]
        private float minZoom = 1f;

        [SerializeField, UnityEngine.RangeAttribute(1f,30f)] private float maxZoom = 5f; 
        [SerializeField, UnityEngine.RangeAttribute(1f,50f)] private float smoothSpeed = 20f; 
        [SerializeField, UnityEngine.RangeAttribute(1f,10f)] private float zoomStrength = 5f; 


        private Camera cam;
        private UnityEngine.Vector3 touchStart;
        private float targetZoom;
        private float zoomVelocity;
        private UnityEngine.Vector3 camOffset;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            cam = GetComponent<Camera>();
            targetZoom = cam.orthographicSize;
            camOffset = cam.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    HandleMobileInput();
                    break;
                case RuntimePlatform.WindowsEditor:
                    HandleDevInput();
                    break;
            }
        }

        private void HandleMobileInput()
        {
            
        }

        private void HandleDevInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandlePanStart(Input.mousePosition);
            }
            
            if (Input.GetMouseButton(0))
            {
                HandlePanMove(Input.mousePosition);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                HandlePanEnd();
            }
            
            HandleZoom(Input.GetAxis("Mouse ScrollWheel"));

        }

        private void HandlePanStart(UnityEngine.Vector2 pos)
        {
            touchStart = cam.ScreenToWorldPoint(pos);
        }
        
        private void HandlePanMove(UnityEngine.Vector2 delta)
        {
            UnityEngine.Vector3 direction = touchStart - cam.ScreenToWorldPoint(delta);
            direction.y = 0;
            transform.position+=direction;
        }

        private void HandlePanEnd()
        {
            UnityEngine.Vector3 camPos = cam.transform.position;
            camPos.x = Mathf.Clamp(camPos.x, mapBoundsMinX, mapBoundsMaxX);
            camPos.z = Mathf.Clamp(camPos.z, mapBoundsMinZ, mapBoundsMaxZ);
            camPos.y = camOffset.y;
            transform.DOMove(camPos,camSnapBackDuration).SetEase(Ease.OutQuart);
        }

        private void HandleZoom(float zoomInput)
        {
            if (zoomInput != 0)
            {
                targetZoom -= zoomInput * zoomStrength;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }

            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothSpeed * Time.deltaTime);
        }
    }

}
