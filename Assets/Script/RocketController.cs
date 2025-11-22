using UnityEngine;

public class RocketController : MonoBehaviour
{
    [Header("Gravity / Drift")]
    public float gravityScale = 0.1f;      // semakin besar, semakin cepat turun

    [Header("Boost")]
    public float upwardBoost = 30f;
    public float forwardBoost = 6f;

    [Header("Tilt")]
    public float tiltUp = 25f;
    public float tiltDown = -30f;
    public float tiltSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;   // aktifkan gravitasi
    }

    void Update()
    {
        bool holding = Input.GetMouseButton(0);

        if (holding)
        {
            Boost();
            Tilt(tiltUp);
        }
        else
        {
            Tilt(tiltDown);
        }
    }

    void Boost()
    {
        // Tambah gaya ke atas & kanan
        rb.AddForce(Vector2.up * upwardBoost * Time.deltaTime, ForceMode2D.Force);
        rb.AddForce(Vector2.right * forwardBoost * Time.deltaTime, ForceMode2D.Force);
    }

    void Tilt(float target)
    {
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, target, Time.deltaTime * tiltSpeed);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
