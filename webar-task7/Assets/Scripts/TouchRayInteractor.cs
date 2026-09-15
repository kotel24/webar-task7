using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using XRScaleMode = UnityEngine.XR.Interaction.Toolkit.Interactors.ScaleMode;

[DefaultExecutionOrder(-200)]
[RequireComponent(typeof(XRRayInteractor))]
public class TouchRayInteractor : MonoBehaviour
{
    public float pinchSpeed = 1f;

    private XRRayInteractor ray;
    private float previousGap;
    private bool wasPinching;

    void Awake()
    {
        ray = GetComponent<XRRayInteractor>();
        ray.manipulateAttachTransform = true;
        ray.scaleMode = XRScaleMode.ScaleOverTime;
        ray.selectInput.inputSourceMode = XRInputButtonReader.InputSourceMode.ManualValue;
        ray.scaleToggleInput.inputSourceMode = XRInputButtonReader.InputSourceMode.ManualValue;
        ray.scaleOverTimeInput.inputSourceMode = XRInputValueReader.InputSourceMode.ManualValue;
    }

    void Start()
    {
        ray.scaleToggleInput.QueueManualState(true, 1f, true, false);
    }

    void Update()
    {
        Camera view = Camera.main;
        if (view == null)
        {
            return;
        }

        Vector2 first = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 second = first;
        int count = 0;

        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (!touch.press.isPressed)
                {
                    continue;
                }
                if (count == 0)
                {
                    first = touch.position.ReadValue();
                }
                else if (count == 1)
                {
                    second = touch.position.ReadValue();
                }
                count++;
            }
        }

        if (count < 2)
        {
            Ray screenRay = view.ScreenPointToRay(first);
            transform.SetPositionAndRotation(screenRay.origin, Quaternion.LookRotation(screenRay.direction));
        }

        bool pressed = count > 0;
        ray.selectInput.QueueManualState(pressed, pressed ? 1f : 0f);

        float scale = 0f;
        bool pinching = count >= 2;
        if (pinching)
        {
            float gap = Vector2.Distance(first, second);
            if (wasPinching && Time.deltaTime > 0f)
            {
                scale = (gap - previousGap) / Screen.height / Time.deltaTime * pinchSpeed;
            }
            previousGap = gap;
        }
        wasPinching = pinching;
        ray.scaleOverTimeInput.manualValue = new Vector2(0f, scale);
    }
}
