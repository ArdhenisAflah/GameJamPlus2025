using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    private bool canStart = false;

    private void Start()
    {
        // Small delay prevents instant skipping when scene loads
        Invoke(nameof(EnableStart), 0.5f);
    }

    void EnableStart()
    {
        canStart = true;
    }

    private void Update()
    {
        if (!canStart) return;

        // Keyboard / Controller / Mouse / Touch
        if (Input.anyKeyDown ||
            Input.touchCount > 0 ||
            Input.GetMouseButtonDown(0))
        {
            LoadGameplay();
        }
    }

    void LoadGameplay()
    {
        SceneManager.LoadScene("Hans"); // Change to your Scene name
    }
}