using UnityEngine;

public class RocketAutoLaunch : MonoBehaviour
{
    private Rigidbody2D rb;
    private RocketController controller;
    private RocketStats stats;

    private bool launched = false;
    private bool controlEnabled = false; // Status baru

    // --- Tambahkan variabel untuk TILT saat LAUNCH ---
    [Header("Launch Tilt Settings")]
    [Tooltip("Target Angle rotasi saat peluncuran (e.g., sama dengan tiltUp di controller)")]
    public float launchTargetTilt = 25f; 
    [Tooltip("Kecepatan lerp rotasi saat peluncuran")]
    public float launchTiltSpeed = 10f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<RocketController>();
        stats = GetComponent<RocketStats>();

        // Nonaktifkan controller dan pastikan tidak membekukan rotasi untuk lerp rotasi
        if (controller != null)
            controller.enabled = false;
        
        // Penting: Rotasi Z harus tidak dibekukan agar LerpAngle bekerja
        // Meskipun tidak menggunakan AddTorque, kita perlu rotasi Z tidak dibekukan 
        // selama fase ini jika Rigidbody perlu berinteraksi dengan fisika secara umum.
        // Namun, jika Anda menggunakan transform.rotation, Rigidbody2D tidak perlu peduli.
        
        LaunchRocket();
    }

    void FixedUpdate()
    {
        // Jalankan rotasi terkontrol hanya jika sudah diluncurkan dan kontrol belum diaktifkan
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

        // Terapkan Gaya Dorong (Force)
        rb.AddForce(
            new Vector2(stats.launchForwardForce, stats.launchUpwardForce), 
            ForceMode2D.Impulse
        );

        // HILANGKAN AddTorque di sini. Kita ganti dengan Lerp Rotasi di FixedUpdate.
        // rb.AddTorque(launchTiltTorque, ForceMode2D.Impulse); // Dihapus!

        // Setelah penundaan, aktifkan kontrol.
        Invoke(nameof(EnableControl), stats.launchControlDelay);
    }

    void ApplyLaunchTilt()
    {
        // Gunakan LerpAngle, seperti di RocketController, untuk rotasi yang mulus
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

        // Setelah kontrol diaktifkan, RocketController akan mengambil alih rotasi.
        if (controller != null)
            controller.enabled = true;
    }
}