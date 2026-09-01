using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace Controller
{
    public class RTSCameraController : MonoBehaviour
    {
        [Header("Input")]

        [SerializeField] InputActionReference moveAction;
        [SerializeField] string sprintActionName = "Sprint";
        [SerializeField] float inputDeadZone = 0.1f;

        private bool hasMoveInput = false;
        private Vector3 moveInput3D;
        private bool sprintInput;
        private InputAction sprintAction;
        float currentSpeedMultiplier = 1f;

        [Header("Movement")]
        [SerializeField] float moveSpeed = 15f;
        [SerializeField] float sprintSpeedMultiplier = 4f;
        [SerializeField] float acceleration = 50f;
        [SerializeField] float decelerateDuration = 1f;
        [SerializeField] float mapBoundsMaxX = 1f;
        [SerializeField] float mapBoundsMinX = 1f;
        [SerializeField] float mapBoundsMaxZ = 1f;
        [SerializeField] float mapBoundsMinZ = 1f;
        [SerializeField] AnimationCurve decelerateCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

        Vector3 currentVelocity = Vector3.zero;
        float decelTimer = 0f;
        Vector3 decelStartVelocity;

        [Header("Components")]
        [SerializeField] CinemachineCamera camera;
        [SerializeField] CinemachineOrbitalFollow orbitalFollow;
        [SerializeField] Transform cameraTarget;

        [Header("Reset")]
        private InputAction resetAction;
        private Vector3 defaultTargetPosition;
        private float defaultHorizontal;
        private float defaultVertical;
        private float defaultRadial;
        
        #region Unity Methods

        private void OnValidate()
        {
            if (camera == null) camera = GetComponent<CinemachineCamera>();
            if (orbitalFollow == null) orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        }

        private void Awake()
        {
            sprintAction = InputSystem.actions.FindAction(sprintActionName);
            resetAction = InputSystem.actions.FindAction("reset");
        }

        private void Start()
        {
            defaultTargetPosition = cameraTarget.position;
            defaultHorizontal = orbitalFollow.HorizontalAxis.Value;
            defaultVertical = orbitalFollow.VerticalAxis.Value;
            defaultRadial = orbitalFollow.RadialAxis.Value;
        }

        private void Update()
        {
            HandleInput();
            UpdateMovement();
            ResetPosition();
        }

        #endregion

        void HandleInput()
        {
            Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

            Vector3 forward = camera.transform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = camera.transform.right;
            right.y = 0f;
            right.Normalize();

            moveInput3D = forward * moveInput.y + right * moveInput.x;

            hasMoveInput = moveInput.sqrMagnitude > inputDeadZone * inputDeadZone;

            // ===================
            // sprint input
            // ===================
            sprintInput = sprintAction.IsPressed();

            float targetMultiplier = 1f;

            if(sprintInput)
            {
                targetMultiplier = sprintSpeedMultiplier;
            }

            currentSpeedMultiplier = Mathf.Lerp(
                currentSpeedMultiplier, 
                targetMultiplier, 
                10 * Time.unscaledDeltaTime);
        }

        void UpdateMovement()
        {
            if (hasMoveInput) // accelerating
            {  
                Vector3 targetVelocity = moveInput3D * moveSpeed * currentSpeedMultiplier;

                float maxDelta = acceleration * Time.unscaledDeltaTime;

                currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, maxDelta);

                decelTimer = 0f;
                decelStartVelocity = currentVelocity;
            }
            else // decelerating
            {
                float normalizedTime = 1f;

                if (decelerateDuration >= 0.01f)
                {
                    normalizedTime = decelTimer / decelerateDuration;
                }

                float curveValue = decelerateCurve.Evaluate(normalizedTime);

                currentVelocity = Vector3.Lerp(Vector3.zero, decelStartVelocity, curveValue);

                decelTimer += Time.unscaledDeltaTime;
            }


            Vector3 targetPos = cameraTarget.position + currentVelocity * Time.unscaledDeltaTime;

            if (targetPos.x <= mapBoundsMinX || targetPos.x >= mapBoundsMaxX)
            {
                currentVelocity.x = 0f;
            }
            if (targetPos.z <= mapBoundsMinZ || targetPos.z >= mapBoundsMaxZ)
            {
                currentVelocity.z = 0f;
            }

            targetPos.x = Mathf.Clamp(targetPos.x, mapBoundsMinX, mapBoundsMaxX);
            targetPos.z = Mathf.Clamp(targetPos.z, mapBoundsMinZ, mapBoundsMaxZ);

            cameraTarget.position = targetPos;
        }

        void ResetPosition()
        {
            if (resetAction.WasPressedThisFrame())
            {
                cameraTarget.position = defaultTargetPosition;

                currentVelocity = Vector3.zero;
                decelStartVelocity = Vector3.zero;
                decelTimer = 0f;

                orbitalFollow.HorizontalAxis.Value = defaultHorizontal;
                orbitalFollow.VerticalAxis.Value = defaultVertical;
                orbitalFollow.RadialAxis.Value = defaultRadial;
            }
        }
    }
}