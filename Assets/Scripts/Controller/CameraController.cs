using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Bounds")]
        [SerializeField] private float mapBoundsMinX = -1f;
        [SerializeField] private float mapBoundsMaxX = 0f;
        [SerializeField] private float mapBoundsMinZ = -1f;
        [SerializeField] private float mapBoundsMaxZ = 0f;

        [Header("Camera Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float smoothSpeed = 20f;

        [Header("Camera Input")]
        [SerializeField] private InputActionReference moveAction;

        private Vector3 camPos;

        private void Awake()
        {
            camPos = transform.localPosition;
        }

        private void OnEnable()
        {
            moveAction.action.Enable();
        }

        private void OnDisable()
        {
            moveAction.action.Disable();
        }

        private void Update()
        {
            Vector2 move = moveAction.action.ReadValue<Vector2>();

            camPos +=  Time.deltaTime * moveSpeed * new Vector3(move.x, 0f, move.y * 1.5f);

            camPos.x = Mathf.Clamp(camPos.x, mapBoundsMinX, mapBoundsMaxX);
            camPos.z = Mathf.Clamp(camPos.z, mapBoundsMinZ, mapBoundsMaxZ);

            transform.localPosition = Vector3.Lerp(transform.localPosition, camPos, smoothSpeed * Time.deltaTime);
        }
    }
}