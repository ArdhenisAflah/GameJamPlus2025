using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // drag player di Inspector
    public Vector3 offset;      // jarak kamera terhadap player

    void LateUpdate()
    {
        if (target == null) return;

        // Kamera langsung mengikuti player tanpa smooth
        transform.position = target.position + offset;
    }
}
