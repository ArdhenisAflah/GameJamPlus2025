using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    private bool canStart = false;

    private void Start()
    {
        // Delay untuk mencegah skip cepat
        Invoke(nameof(EnableStart), 0.5f);

        // Load Semua Save
        SaveSystem.Instance.RestoreSave();

        Debug.Log("StartMenu Loaded | hasSeenOpeningCutscene = " +
                  SaveSystem.Instance.hasSeenOpeningCutscene);

        // Putar BGM
        GameAudioManager.Instance.PlayBGM("bgm1");
    }

    void EnableStart()
    {
        canStart = true;
    }

    private void Update()
    {
        if (!canStart) return;

        if (Input.anyKeyDown ||
            Input.touchCount > 0 ||
            Input.GetMouseButtonDown(0))
        {
            LoadCorrectScene();
        }
    }

    void LoadCorrectScene()
    {
            SceneManager.LoadScene("Opening Cutscene");
    }
}
