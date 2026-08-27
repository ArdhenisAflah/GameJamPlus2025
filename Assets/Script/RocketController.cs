using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    [Header("References")]
    public FuelGauge fuelGauge;
    public SpeedometerNeedle needle;

    [Header("Gravity / Drift")]
    public float gravityScale = 0.1f;

    [Header("Fuel UI")]
    public Slider fuelSlider;

    [Header("Tilt")]
    public float tiltUp = 25f;
    public float tiltDown = -30f;
    public float tiltSpeed = 5f;

    [Header("Mobile & Input Settings")]
    [Tooltip("If true, touching on top of UI elements (e.g., buttons) will not trigger rocket boost.")]
    [SerializeField] private bool ignoreTouchOverUI = true;
    [Tooltip("Enable keyboard Space/Up/W keys for boosting in Editor/PC.")]
    [SerializeField] private bool enableKeyboardBoost = true;

    private Rigidbody2D rb;
    private SlowManager slowManager;
    private RocketStats stats;
    private RocketAnimationController animCtrl;

    private float fuel;
    private bool grounded = false;
    private bool isHoldingInput = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        slowManager = GetComponent<SlowManager>();
        stats = GetComponent<RocketStats>();
        animCtrl = GetComponent<RocketAnimationController>();



        rb.gravityScale = gravityScale;

        fuel = stats.maxFuel;

        if (fuelSlider != null)
            fuelSlider.value = 1f;
    }

    private void OnDisable()
    {
        // Safety: pastikan animasi & efek boost berhenti ketika controller di-disable (contoh: saat Game Over)
        isHoldingInput = false;
        if (animCtrl != null)
        {
            animCtrl.PlayBoost(false, false);
        }
    }

    void Update()
    {
        if (stats == null || rb == null) return;

        float slow = (slowManager != null) ? slowManager.FinalMultiplier : 1f;

        // Mendeteksi apakah pemain sedang menekan layar (Mobile Touch / Mouse / Keyboard)
        isHoldingInput = CheckBoostInput();

        // BOOST LOGIC
        if (isHoldingInput && fuel > 0f)
        {
            Boost(slow);

            if (!grounded)
                Tilt(tiltUp);

            fuel -= stats.fuelBurnRate * Time.deltaTime;

            if (animCtrl != null)
                animCtrl.PlayBoost(true, true);
        }
        else
        {
            if (!grounded)
                Tilt(tiltDown);

            if (fuel < stats.maxFuel)
                fuel += stats.fuelRegenRate * Time.deltaTime;

            if (animCtrl != null)
                animCtrl.PlayBoost(false, false);
        }

        fuel = Mathf.Clamp(fuel, 0, stats.maxFuel);

        // Horizontal slow
        ApplyHorizontalSlow(slow);

        UpdateFuelUI();

        fuelGauge?.UpdateFuel(fuel, stats.maxFuel);
        needle?.UpdateNeedle(rb.velocity.x);
    }

    /// <summary>
    /// Mendeteksi input hold secara konsisten di Mobile (Touch) maupun Desktop/Editor (Mouse/Keyboard).
    /// </summary>
    private bool CheckBoostInput()
    {
        // 1. Cek Mobile Touch Screen (Mendukung multi-touch & touch phases)
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // Sentuhan valid jika sedang ditekan (Began), digeser (Moved), atau ditahan diam (Stationary)
                if (touch.phase == TouchPhase.Began ||
                    touch.phase == TouchPhase.Moved ||
                    touch.phase == TouchPhase.Stationary)
                {
                    // Abaikan jika jari menekan elemen UI (misal tombol pause/menu)
                    if (ignoreTouchOverUI && EventSystem.current != null)
                    {
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                            continue;
                    }

                    return true;
                }
            }
        }

        // 2. Cek Mouse Click (Untuk testing di Unity Editor & PC Build)
        if (Input.GetMouseButton(0))
        {
            if (ignoreTouchOverUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return false;
            }
            return true;
        }

        // 3. Cek Keyboard Shortcut (Space / W / Up Arrow untuk kemudahan testing)
        if (enableKeyboardBoost && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            return true;
        }

        return false;
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

    // ==========================
    // GROUND DETECTION
    // ==========================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            grounded = true;

            rb.velocity *= stats.groundSpeedLoss;

            // Stop animation saat nempel ground
            if (animCtrl != null)
                animCtrl.PlayBoost(false, false);

            // Optional: reset rotation
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
