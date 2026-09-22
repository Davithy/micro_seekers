using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Controller
{
    public class InputController : MonoBehaviour
    {
        [Header("Idle Time")]
        [SerializeField, RangeAttribute(1f, 5f)] private float gazeTimer = 3f;

        [Header("Camera Input")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference rotateAction;

        [Header("Gaze UI")]
        [SerializeField] private Image loadingCircle;
        [SerializeField] private Color defaultColor = Color.white;
        

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
                SetFillColor(defaultColor);
                loadingCircle.fillAmount = 0f;
                return;
            }

            idleTimer += Time.deltaTime;
            UpdateFillUI();
            
            if (idleTimer >= gazeTimer && !hasTriggered)
            {
                hasTriggered = true;
                RayCaster();
            }
        }

        private void RayCaster()
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);

            if (!Physics.Raycast(ray, out RaycastHit hitData) || !hitData.transform.TryGetComponent<iClickable>(out iClickable clickable)) 
            {
                SetFillColor(Color.red);
                return;
            }

            clickable.OnClick();
        }

        private void UpdateFillUI()
        {
            if (loadingCircle == null) return;

            float loadingWarmup = gazeTimer * 0.5f;
            float fill;

            if (idleTimer < loadingWarmup)
            {
                fill = 0f;
            }
            else
            {
                fill = (idleTimer - loadingWarmup) / (gazeTimer - loadingWarmup);
            }

            loadingCircle.fillAmount = Mathf.Clamp01(fill);
        }

        public void SetFillColor (Color color)
        {
            if (loadingCircle != null)
            {
                loadingCircle.color = color;
            }
        }
    }
}
