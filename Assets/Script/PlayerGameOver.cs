using TMPro;
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

    public GameObject GameOverPanel;
    public Transform panelover;

    public GameObject ShellsScore;
    public GameObject Upgrades;

    private bool isGameOver = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        panelover = GameOverPanel.transform.GetChild(2);
    }
    void Update()
    {
        // Keyboard / Controller / Mouse / Touch
        if ((Input.anyKeyDown && isGameOver == true) ||
            (Input.touchCount > 0 && isGameOver == true) ||
            (Input.GetMouseButtonDown(0) && isGameOver == true))
        {
            LoadUpgrade();
        }
        // Cek velocity player
        if (rb.velocity.magnitude <= velocityLimit && !isGameOver)
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
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameOverPanel.SetActive(true);
        // Bisa diganti dengan UI GameOver juga
        panelover.gameObject.GetComponent<TextMeshProUGUI>().text = "Score: " + ScorSystem.score.ToString();

        //set bool gameover
        isGameOver = true;

    }

    void LoadUpgrade()
    {
        if (ScorSystem.score >= 3000)
        {
            SceneManager.LoadScene("EndCutscene");
        }
        else
        {
            GameOverPanel.SetActive(false);
            ShellsScore.SetActive(true);
            Upgrades.SetActive(true);
        }
    }
}
