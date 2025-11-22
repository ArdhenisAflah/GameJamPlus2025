using UnityEngine;

public class RocketAutoLaunch : MonoBehaviour
{
    private Rigidbody2D rb;
    private RocketController controller;
    private RocketStats stats;

    private bool launched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<RocketController>();
        stats = GetComponent<RocketStats>();

        if (controller != null)
            controller.enabled = false;

        LaunchRocket();
    }

    void LaunchRocket()
    {
        if (launched) return;
        launched = true;

        rb.velocity = Vector2.zero;

        // Ambil nilai dari RocketStats
        rb.AddForce(
            new Vector2(stats.launchForwardForce, stats.launchUpwardForce), 
            ForceMode2D.Impulse
        );

        Invoke(nameof(EnableControl), stats.launchControlDelay);
    }

    void EnableControl()
    {
        if (controller != null)
            controller.enabled = true;
    }
}
