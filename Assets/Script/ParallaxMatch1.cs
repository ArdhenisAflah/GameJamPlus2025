using UnityEngine;

public class ParallaxMatch1 : MonoBehaviour
{
    public Transform cam;

    [Header("Follow Aaxes just X now..")]
    public bool followX = true;
    public bool followY = true;

    private Vector3 offset;

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        // pliss bekerja kontol
        offset = transform.position - cam.position;
    }

    void LateUpdate()
    {
        Vector3 targetPos = cam.position + offset;

        transform.position = new Vector3(
            followX ? targetPos.x : transform.position.x,
            followY ? targetPos.y : transform.position.y,
            transform.position.z
        );
    }
}
