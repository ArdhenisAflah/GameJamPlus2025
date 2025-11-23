using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    private string savePath;
    [SerializeField] private List<GameObject> prefabDatabase;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "save.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ====================
    // SAVE
    // ====================
    public void Save()
    {
        SaveData data = new SaveData
        {
            levelLaunch = UpgradeManager.Instance != null ? UpgradeManager.Instance.levelLaunch : 0,
            levelBoost = UpgradeManager.Instance != null ? UpgradeManager.Instance.levelBoost : 0,
            levelFuel = UpgradeManager.Instance != null ? UpgradeManager.Instance.levelFuel : 0,
            levelWall = UpgradeManager.Instance != null ? UpgradeManager.Instance.levelWall : 0,

            shell = ShellManager.Instance != null ? ShellManager.Instance.shell : 0
        };

        // Simpan ke file
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Shell: {data.shell}");
    }

    // ====================
    // LOAD
    // ====================
    public SaveData Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (ShellManager.Instance != null)
            {
                ShellManager.Instance.LoadShellFromSave(data.shell);
            }

            Debug.Log($"Loaded Global Save");
            return data;
        }

        Debug.LogWarning("No Global Save file found.");
        return null;
    }

    // ====================
    // RESTORE SAVE
    // ====================
    public SaveData RestoreSave()
    {
        SaveData data = Load();
        if (data == null) return null;

        // 🔹 Pulihkan stats shell
        if (ShellManager.Instance != null)
        {
            ShellManager.Instance.LoadShellFromSave(data.shell);
        }

        return data;
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Deleted Global Save.json");
        }
    }

    public void DeleteAllSaves()
    {
        string dir = Application.persistentDataPath;
        string[] files = Directory.GetFiles(dir, "save_*.json");

        foreach (var file in files)
        {
            File.Delete(file);
            Debug.Log($"Deleted: {Path.GetFileName(file)}");
        }

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Deleted main save.json");
        }
    }
}