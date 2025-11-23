using UnityEngine;

public class SaveLoader : MonoBehaviour
{
    [Header("Load on Start?")]
    public bool loadOnStart = true;

    void Start()
    {
        if (loadOnStart)
            LoadAllData();
    }

    public void LoadAllData()
    {
        // Pastikan SaveSystem sudah ada
        if (SaveSystem.Instance == null)
        {
            Debug.LogWarning("SaveSystem not found in scene!");
            return;
        }

        SaveData data = SaveSystem.Instance.RestoreSave();
        
        if (data == null)
        {
            Debug.Log("No save data found.");
            return;
        }
    }
}
