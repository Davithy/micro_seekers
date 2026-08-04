using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class OrbiterPivot : MonoBehaviour
    {
        [Header("Pivot Point")]
        [SerializeField] private Transform platform;


        [Header("Rotation")]
        [SerializeField, RangeAttribute(1f, 500f)] private float rotateSpeed = 250f;
        [SerializeField, RangeAttribute(1f, 200f)] private float smoothSpeed = 20f;
        [SerializeField] private InputActionReference rotateAction;

        private float camY;

        void Awake()
        {
            camY = transform.eulerAngles.y;     
        }

        private void OnEnable()
        {
            rotateAction.action.Enable();
        }

        private void OnDisable()
        {
            rotateAction.action.Disable();
        }

        private void Update()
        {
            transform.position = platform.position;

            float rotate = rotateAction.action.ReadValue<float>();
            camY += rotate * rotateSpeed * Time.deltaTime;

            Quaternion camRot = Quaternion.Euler(0f, camY, 0f);
            transform.rotation = Quaternion.Slerp (transform.rotation, camRot, smoothSpeed * Time.deltaTime);
        }
    }
}
