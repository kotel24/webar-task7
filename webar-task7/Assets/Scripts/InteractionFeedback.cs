using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractionFeedback : MonoBehaviour
{
    public Color hoverColor = Color.yellow;
    public Color selectColor = Color.green;

    private XRGrabInteractable interactable;
    private Renderer targetRenderer;
    private Color normalColor;

    void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        targetRenderer = GetComponentInChildren<Renderer>();
        normalColor = targetRenderer.material.color;
    }

    void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        UpdateColor();
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        UpdateColor();
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        UpdateColor();
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        UpdateColor();
    }

    void UpdateColor()
    {
        if (interactable.isSelected)
        {
            targetRenderer.material.color = selectColor;
        }
        else if (interactable.isHovered)
        {
            targetRenderer.material.color = hoverColor;
        }
        else
        {
            targetRenderer.material.color = normalColor;
        }
    }
}
