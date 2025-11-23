using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;          // How fast the fish moves left
    public float sineAmplitude = 1.0f;// Height of the wave
    public float sineFrequency = 2f;  // Speed of the wave

    private float startY;             // Original Y position
    private float timeOffset;         // For desync movement

    void Start()
    {
        startY = transform.position.y;
        timeOffset = Random.Range(0f, 10f); // Makes each fish unique wave
    }

    void Update()
    {
        // Move left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Sine wave vertical movement
        float newY = startY + Mathf.Sin((Time.time + timeOffset) * sineFrequency) * sineAmplitude;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Auto destroy when out of screen
        if (transform.position.x < -20f) // adjust if needed
            Destroy(gameObject);
    }
}
