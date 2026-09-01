using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class CameraReset : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference resetAction;

        [Header("Camera References")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private OrbiterPivot orbiterPivot;

        private Vector3 defaultLocalPosition;
        private float defaultYaw;

        private void Awake()
        {
            defaultLocalPosition = cameraController.transform.localPosition;
            defaultYaw = orbiterPivot.transform.eulerAngles.y;
        }

        private void OnEnable()
        {
            resetAction.action.Enable();
            resetAction.action.performed += OnReset;
        }

        private void OnDisable()
        {
            resetAction.action.performed -= OnReset;
            resetAction.action.Disable();
        }

        private void OnReset(InputAction.CallbackContext context)
        {
            RestartCamera();
        }

        public void RestartCamera()
        {
            SetPrivateField(cameraController, "camPos", defaultLocalPosition);
            cameraController.transform.localPosition = defaultLocalPosition;

            SetPrivateField(orbiterPivot, "camY", defaultYaw);
            orbiterPivot.transform.rotation = Quaternion.Euler(0f, defaultYaw, 0f);
        }

        private void SetPrivateField(object target, string fieldName, object value)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(target, value);
        }
    }
}