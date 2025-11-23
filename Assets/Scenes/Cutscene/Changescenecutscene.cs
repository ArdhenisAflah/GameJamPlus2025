using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadSceneByAsset : MonoBehaviour
{
    public PlayableDirector timeline;

    // Scene asset yang bisa di-drag di inspector
    #if UNITY_EDITOR
    public SceneAsset sceneAsset;
    #endif

    // Path scene untuk runtime
    [SerializeField] private string scenePath;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        if (sceneAsset != null)
        {
            // Ambil path scene secara otomatis
            scenePath = AssetDatabase.GetAssetPath(sceneAsset);
        }
        #endif
    }

    private void Start()
    {
        if (timeline == null)
            timeline = GetComponent<PlayableDirector>();

        timeline.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector pd)
    {
        if (!string.IsNullOrEmpty(scenePath))
        {
            SceneManager.LoadScene(scenePath);
        }
        else
        {
            Debug.LogError("Scene belum diassign!");
        }
    }
}
