using UnityEngine;

// REQUIREMENT: This script must be attached to the MAIN BODY (Chassis) of the wheelchair
public class WheelChairController : MonoBehaviour
{
    [Header("Physics Components")]
    [Tooltip("The Rigidbody of the back wheel itself.")]
    public Rigidbody2D wheelRigidbody;

    [Tooltip("The Rigidbody of the main chair/character.")]
    public Rigidbody2D headRigidbody;

    [Header("Movement Settings")]
    [Tooltip("Maximum speed in units per second.")]
    public float moveSpeed = 5f; // Changed to a reasonable speed value

    [Header("Balance Settings")]
    [Tooltip("Force applied to lean back (wheelie) or forward.")]
    public float leanForce = 500f;


    [Header("Kekuatan Leher")]
    [Tooltip("Seberapa kuat kepala mencoba tegak. Mulai dari 500-2000.")]
    public float kekakuanOtot = 5000f;

    [Tooltip("Peredam agar kepala tidak membal-membal (jitter). Mulai dari 0.5 - 10.")]
    public float peredam = 5f;


    [Header("Target Rotasi")]
    [Tooltip("0 artinya tegak lurus ke atas.")]
    public float targetSudut = 0f;

    // Internal variables to store input
    private float _horizontalInput; // For moving forward/back
    private float _verticalInput;   // For leaning

    private void Update()
    {
        // 1. INPUT (Always in Update)
        _horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrows
        _verticalInput = Input.GetAxisRaw("Vertical");     // W/S or Up/Down arrows
    }

    private void FixedUpdate()
    {
        // 2. PHYSICS (Always in FixedUpdate)
        MoveWheelDirectly();
        StabilkanKepala();
    }

    private void MoveWheelDirectly()
    {
        // CHANGE: Direct Velocity Control
        // We get the current velocity so we don't mess up Gravity (Y axis)
        Vector2 targetVelocity = wheelRigidbody.velocity;

        // We ONLY overwrite the X axis (Left/Right)
        // This gives you instant control. No sliding, no acceleration time.
        targetVelocity.x = _horizontalInput * moveSpeed;

        // Apply the new velocity vector back to the Rigidbody
        wheelRigidbody.velocity = targetVelocity;
    }

    void StabilkanKepala()
    {
        if (headRigidbody == null) return;

        // 1. Hitung selisih sudut saat ini dengan target (0 derajat)
        // Mathf.DeltaAngle menangani masalah putaran 360 -> 0 derajat dengan aman
        float selisihSudut = Mathf.DeltaAngle(transform.eulerAngles.z, targetSudut);

        // 2. Hitung kekuatan putar (Torque) yang dibutuhkan
        // Rumus: (Error * Kekuatan) - (Kecepatan * Peredam)
        // Ini disebut PD Controller (Proportional-Derivative) sederhana
        float torque = (selisihSudut * kekakuanOtot) - (headRigidbody.angularVelocity * peredam);

        // 3. Terapkan putaran ke Rigidbody
        // Gunakan ForceMode2D.Force untuk simulasi otot yang halus
        headRigidbody.AddTorque(torque * Time.fixedDeltaTime);
    }
}