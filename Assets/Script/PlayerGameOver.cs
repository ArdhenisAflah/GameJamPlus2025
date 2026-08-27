using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGameOver : MonoBehaviour
{
    [Header("Settings")]
    public Rigidbody2D rb;
    public string groundTag = "Ground";
    public float velocityLimit = 0.01f; // batas minimal dianggap 0
    public float checkDelay = 1f;       // waktu diam sebelum game over

    [Header("UI Panels")]
    public GameObject GameOverPanel;
    public Transform panelover;
    public GameObject ShellsScore;
    public GameObject Upgrades;

    private float idleTimer = 0f;
    private bool touchedGround = false;
    private bool isGameOver = false;
    private bool hasFinishedRun = false;

    private RocketController controller;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (GameOverPanel != null && GameOverPanel.transform.childCount > 2)
            panelover = GameOverPanel.transform.GetChild(2);

        controller = GetComponent<RocketController>();
    }

    private void Update()
    {
        // 1. Jika sudah masuk ke menu Upgrade / Run selesai, hentikan semua proses Update
        if (hasFinishedRun) return;

        // 2. Kondisi saat Game Over aktif (Menunggu input sentuhan layar dari pemain)
        if (isGameOver)
        {
            if (IsTapToContinue())
            {
                LoadUpgrade();
            }
            return;
        }

        // 3. Cek velocity player saat gameplay berlangsung
        if (rb != null && rb.velocity.magnitude <= velocityLimit)
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

    /// <summary>
    /// Mendeteksi tap/sentuhan pertama di mobile ataupun klik di editor/PC.
    /// </summary>
    private bool IsTapToContinue()
    {
        // Mobile Touch: Hanya respon pada sentuhan awal (Began)
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                return true;
            }
        }

        // Mouse Click Fallback (Editor & PC)
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        // Keyboard Fallback (Editor & PC)
        if (Input.anyKeyDown)
        {
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(groundTag))
        {
            touchedGround = true;
        }
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
        isGameOver = true;

        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);

        if (panelover != null)
        {
            var scoreText = panelover.gameObject.GetComponent<TextMeshProUGUI>();
            if (scoreText != null)
            {
                scoreText.text = "Score: " + ScorSystem.score.ToString();
            }
        }
    }

    private void LoadUpgrade()
    {
        // Tandai bahwa run sudah selesai agar Update tidak lagi memicu GameOver()
        isGameOver = false;
        hasFinishedRun = true;

        if (ScorSystem.score >= 1000)
        {
            SceneManager.LoadScene("EndCutscene");
            return;
        }

        // Matikan kontrol roket
        if (controller != null)
        {
            controller.enabled = false;
        }

        Debug.Log("Matikan Game Over Panel");
        if (GameOverPanel != null)
            GameOverPanel.SetActive(false);

        if (ShellsScore != null)
            ShellsScore.SetActive(true);

        if (Upgrades != null)
            Upgrades.SetActive(true);

        if (ScorSystem.score > 0)
        {
            ShellManager.Instance?.AddShell(ScorSystem.score);
            Debug.Log("Converted Score to Shell: +" + ScorSystem.score);
            ScorSystem.score = 0;
        }

        SaveSystem.Instance?.Save();
    }
}
