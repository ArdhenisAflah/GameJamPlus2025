using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    public RectTransform needle;

    [Header("Speed Settings")]
    public float maxSpeed = 100f;
    public float minAngle = 45f;
    public float maxAngle = -45f;
    private float currentSpeed;

    void Start()
    {
        UpdateNeedle(0f);
    }

    public void UpdateNeedle(float speed)
    {
        currentSpeed = Mathf.Clamp(speed, 0, maxSpeed);
        
        float t = currentSpeed / maxSpeed;
        float angle = Mathf.Lerp(minAngle, maxAngle, t);
        
        needle.localRotation = Quaternion.Euler(0, 0, angle);
    }
}