using UnityEngine;
using UnityEngine.SceneManagement; // kalau mau restart scene

public class PlayerGameOver : MonoBehaviour
{
    public Rigidbody2D rb;
    public string groundTag = "Ground";
    public float velocityLimit = 0.01f; // batas minimal dianggap 0
    public float checkDelay = 1f; // waktu diam sebelum game over

    private float idleTimer = 0f;
    private bool touchedGround = false;

    void Update()
    {
        // Cek velocity player
        if (rb.velocity.magnitude <= velocityLimit)
        {
            idleTimer += Time.deltaTime;

            // Jika pemain sudah menyentuh tanah DAN diam
            if (touchedGround && idleTimer >= checkDelay)
            {
                GameOver();
            }
        }
        else
        {
            idleTimer = 0f; // reset kalau player bergerak lagi
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(groundTag))
        {
            touchedGround = true;
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");

        // Contoh: restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Bisa diganti dengan UI GameOver juga
    }
}
