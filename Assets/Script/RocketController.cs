using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    [Header("Gravity / Drift")]
    public float gravityScale = 0.1f;

    [Header("Fuel UI")]
    public Slider fuelSlider;

    [Header("Tilt")]
    public float tiltUp = 25f;
    public float tiltDown = -30f;
    public float tiltSpeed = 5f;

    private Rigidbody2D rb;
    private SlowManager slowManager;
    private RocketStats stats;

    private float fuel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        slowManager = GetComponent<SlowManager>();
        stats = GetComponent<RocketStats>();

        rb.gravityScale = gravityScale;

        fuel = stats.maxFuel;

        if (fuelSlider != null)
            fuelSlider.value = 1f;
    }

    void Update()
    {
        float slow = (slowManager != null) ? slowManager.FinalMultiplier : 1f;
        bool holding = Input.GetMouseButton(0);

        if (holding && fuel > 0f)
        {
            Boost(slow);
            Tilt(tiltUp);
            fuel -= stats.fuelBurnRate * Time.deltaTime;
        }
        else
        {
            Tilt(tiltDown);

            if (fuel < stats.maxFuel)
                fuel += stats.fuelRegenRate * Time.deltaTime;
        }

        fuel = Mathf.Clamp(fuel, 0, stats.maxFuel);

        ApplyHorizontalSlow(slow);

        UpdateFuelUI();
    }

    // ==========================
    // BOOST FORCE
    // ==========================
    void Boost(float slow)
    {
        rb.AddForce(Vector2.up * stats.upwardBoost * slow * Time.deltaTime);
        rb.AddForce(Vector2.right * stats.forwardBoost * slow * Time.deltaTime);
    }

    // ==========================
    // HORIZONTAL SLOW FIX
    // ==========================
    void ApplyHorizontalSlow(float slow)
    {
        float hx = rb.velocity.x;
        float targetHx = hx * slow;

        // smooth supaya tidak patah
        float finalHx = Mathf.Lerp(hx, targetHx, 6f * Time.deltaTime);

        rb.velocity = new Vector2(finalHx, rb.velocity.y);
    }

    // ==========================
    // ROTATION TILT
    // ==========================
    void Tilt(float target)
    {
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, target, Time.deltaTime * tiltSpeed);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // ==========================
    // FUEL UI
    // ==========================
    void UpdateFuelUI()
    {
        if (fuelSlider != null)
            fuelSlider.value = fuel / stats.maxFuel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            rb.velocity *= stats.groundSpeedLoss;
    }
}
