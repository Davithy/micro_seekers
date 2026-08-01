using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class InputController : MonoBehaviour
    {
        [Header("Idle Time")]
        [SerializeField, RangeAttribute(1f, 5f)] private float gazeTimer = 3f;

        [Header("Camera Input")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference rotateAction;

        private Camera camera;
        private float idleTimer;
        private bool hasTriggered;
        private bool isIdle;
        
        private void Awake()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            Vector2 move = moveAction.action.ReadValue<Vector2>();
            float rotate = rotateAction.action.ReadValue<float>();

            if (move == Vector2.zero && rotate == 0f) { isIdle = true; } 
            else { isIdle = false; }

            if (!isIdle)
            {
                idleTimer = 0f;
                hasTriggered = false;
                return;
            }

            idleTimer += Time.deltaTime;

            if (idleTimer >= gazeTimer && !hasTriggered)
            {
                hasTriggered = true;
                RayCaster();
            }
        }

        private void RayCaster()
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            if (!Physics.Raycast(ray, out RaycastHit hitData)) return;
            HandleClickable(hitData);
        }

        private void HandleClickable(RaycastHit hitData)
        {
            if (hitData.transform.TryGetComponent<iClickable>(out iClickable clickable))
            {
                clickable.OnClick();   
            }
        }
    }
}
