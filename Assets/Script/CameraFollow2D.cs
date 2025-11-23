using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // script kontol pembikin jitter
    public float pixelsPerUnit = 100f;

    void LateUpdate()
    {
        if (target == null) return;



        Vector3 desired = target.position + offset;
        // Clamp the Y BEFORE pixel snapping
        desired.y = Mathf.Clamp(desired.y, 0.0f, 3.0f);


        // Convert world units to pixel units
        float unit = 1f / pixelsPerUnit;

        // Pixel-snap camera to prevent jitter
        desired.x = Mathf.Round(desired.x / unit) * unit;
        desired.y = Mathf.Round(desired.y / unit) * unit;

        transform.position = desired;

    }
}
