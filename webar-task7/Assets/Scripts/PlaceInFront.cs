using UnityEngine;
using WebXR;

public class PlaceInFront : MonoBehaviour
{
    public float distance = 1f;

    private bool wasInAR;

    void Start()
    {
        Place();
    }

    void Update()
    {
        bool inAR = WebXRManager.Instance != null && WebXRManager.Instance.XRState == WebXRState.AR;
        if (inAR && !wasInAR)
        {
            Place();
        }
        wasInAR = inAR;
    }

    void Place()
    {
        Transform view = Camera.main.transform;
        transform.position = view.position + view.forward * distance;
    }
}
