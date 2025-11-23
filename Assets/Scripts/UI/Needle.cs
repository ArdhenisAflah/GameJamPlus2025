using UnityEngine;

public class SpeedometerNeedle : MonoBehaviour
{
    public float maxSpeed = 50f;
    public float minAngle = -45f;
    public float maxAngle = 45f;

    public void UpdateNeedle(float speed)
    {
        float t = Mathf.Clamp01(speed / maxSpeed);
        float angle = Mathf.Lerp(minAngle, maxAngle, t);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}