using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    public RectTransform needle;

    [Header("Speed Settings")]
    public float maxSpeed = 200f;
    public float minAngle = -90f;
    public float maxAngle = 90f;
    private float currentSpeed;

    public void UpdateNeedle(float speed)
    {
        currentSpeed = Mathf.Clamp(speed, 0, maxSpeed);
        
        float t = currentSpeed / maxSpeed;
        float angle = Mathf.Lerp(minAngle, maxAngle, t);
        
        needle.localRotation = Quaternion.Euler(0, 0, angle);
    }
}