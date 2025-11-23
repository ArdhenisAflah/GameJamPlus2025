using UnityEngine;

public class RocketAutoLaunch : MonoBehaviour
{
    private Rigidbody2D rb;
    private RocketController controller;
    private RocketStats stats;

    private bool launched = false;
    private bool controlEnabled = false;

    [Header("Launch Delay")]
    public float launchDelay = 1f;   // ⬅ delay sebelum roket mulai meluncur

    [Header("Launch Tilt Settings")]
    public float launchTargetTilt = 25f;
    public float launchTiltSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<RocketController>();
        stats = GetComponent<RocketStats>();

        // Disable manual controller dulu
        if (controller != null)
            controller.enabled = false;

        // Jalankan peluncuran setelah delay
        StartCoroutine(DelayedLaunch());
    }

    // ===============================
    // LAUNCH DELAY
    // ===============================
    System.Collections.IEnumerator DelayedLaunch()
    {
        yield return new WaitForSeconds(launchDelay);
        LaunchRocket();
    }

    void FixedUpdate()
    {
        if (launched && !controlEnabled)
        {
            ApplyLaunchTilt();
        }
    }

    void LaunchRocket()
    {
        if (launched) return;
        launched = true;

        rb.velocity = Vector2.zero;

        rb.AddForce(
            new Vector2(stats.launchForwardForce, stats.launchUpwardForce),
            ForceMode2D.Impulse
        );

        Invoke(nameof(EnableControl), stats.launchControlDelay);
    }

    void ApplyLaunchTilt()
    {
        float angle = Mathf.LerpAngle(
            transform.eulerAngles.z,
            launchTargetTilt,
            Time.deltaTime * launchTiltSpeed
        );

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void EnableControl()
    {
        controlEnabled = true;

        if (controller != null)
            controller.enabled = true;
    }
}
