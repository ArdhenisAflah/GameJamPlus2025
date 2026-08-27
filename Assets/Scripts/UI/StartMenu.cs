using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string nextSceneName = "Opening Cutscene";
    [SerializeField] private float inputDelay = 0.5f;

    private bool canStart = false;
    private bool isLoading = false;

    private void Start()
    {
        // Delay untuk mencegah skip tidak sengaja saat scene baru terbuka
        Invoke(nameof(EnableStart), inputDelay);

        // Load Semua Save
        SaveSystem.Instance?.RestoreSave();
        // Putar BGM
        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlayBGM("bgm1");
        }
    }

    private void EnableStart()
    {
        canStart = true;
    }

    private void Update()
    {
        if (!canStart || isLoading) return;

        if (IsTouchOrPressed())
        {
            LoadCorrectScene();
        }
    }

    private bool IsTouchOrPressed()
    {
        // 1. Cek Mobile Touch (hanya saat awal sentuh / Began)
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                return true;
            }
        }

        // 2. Cek Mouse Click (juga mendeteksi single tap pada mobile/editor)
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        // 3. Cek Keyboard / Controller (untuk testing di PC/Editor)
        if (Input.anyKeyDown)
        {
            return true;
        }

        return false;
    }

    private void LoadCorrectScene()
    {
        isLoading = true; // Mencegah double load scene saat ditekan berkali-kali
        SceneManager.LoadScene(nextSceneName);
    }
}
