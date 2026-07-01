using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace LunaGames.Tools.Joystick
{
    public class Joystick : OnScreenControl, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Tooltip("The distance the visual handle can move from the center of the joystick.")]
        [SerializeField, Range(0.5f, 5f), BoxGroup("Settings")] private float handleRange = 1;
        [Tooltip("The distance away from the center input has to be before registering.")]
        [SerializeField, Range(0f, 5), BoxGroup("Settings")] private float deadZone = 0;
        [Tooltip("Snap the horizontal and vertical input to a whole value.")]
        [SerializeField, EnumToggleButtons, BoxGroup("Settings")] Snap SnapAxis;

        [Tooltip("The background's RectTransform component.")]
        [SerializeField, FoldoutGroup("Assaignables")] protected RectTransform background = null;
        [Tooltip("The handle's RectTransform component.")]
        [SerializeField, FoldoutGroup("Assaignables")] private RectTransform handle = null;

        [Tooltip("Which axes the joystick uses.")]
        [EnumToggleButtons, SerializeField, BoxGroup("Options")] AxisOptions axisOptions = AxisOptions.Both;
        public float Horizontal => SnapAxis == Snap.SnapX || SnapAxis == Snap.SnapAll ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x;
        public float Vertical => SnapAxis == Snap.SnapY || SnapAxis == Snap.SnapAll ? SnapFloat(input.y, AxisOptions.Vertical) : input.y;
        public Vector2 Direction => new Vector2(Horizontal, Vertical);

        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;

        [System.Flags]
        public enum Snap
        {
            SnapX = 1 << 1,
            SnapY = 1 << 2,
            SnapAll = SnapX | SnapY
        }
        public float HandleRange
        {
            get => handleRange;
            set => handleRange = Mathf.Abs(value);
        }

        public float DeadZone
        {
            get => deadZone;
            set => deadZone = Mathf.Abs(value);
        }
        public AxisOptions AxisOptions
        {
            get => AxisOptions;
            set => axisOptions = value;
        }
        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }
        private RectTransform baseRect = null;

        private Canvas canvas;
        private Camera cam;

        private Vector2 input = Vector2.zero;

        protected virtual void Start()
        {
            HandleRange = handleRange;
            DeadZone = deadZone;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
            SendValueToControl<Vector2>(Direction);
        }

        public void OnDrag(PointerEventData eventData)
        {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
            SendValueToControl<Vector2>(Direction);
        }

        protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    input = normalised;
            }
            else
                input = Vector2.zero;
        }

        private void FormatInput()
        {
            if (axisOptions == AxisOptions.Horizontal)
                input = new Vector2(input.x, 0f);
            else if (axisOptions == AxisOptions.Vertical)
                input = new Vector2(0f, input.y);
        }

        private float SnapFloat(float value, AxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (axisOptions == AxisOptions.Both)
            {
                float angle = Vector2.Angle(input, Vector2.up);
                if (snapAxis == AxisOptions.Horizontal)
                {
                    if (angle < 22.5f || angle > 157.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                else if (snapAxis == AxisOptions.Vertical)
                {
                    if (angle > 67.5f && angle < 112.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                return value;
            }
            else
            {
                if (value > 0)
                    return 1;
                if (value < 0)
                    return -1;
            }
            return 0;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            SendValueToControl<Vector2>(Vector2.zero);
        }

        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            Vector2 localPoint = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
            {
                Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }
            return Vector2.zero;
        }
    }

    public enum AxisOptions { Both, Horizontal, Vertical }
}