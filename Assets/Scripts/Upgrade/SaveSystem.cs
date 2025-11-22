using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    private string savePath;
    [SerializeField] private List<GameObject> prefabDatabase;

    private string lastManaBlockID;
    public void SetLastManaBlock(string id)
    {
        lastManaBlockID = id;
    }
    public string GetLastManaBlockID() => lastManaBlockID;

    // 🔹 Simpan mana blocks yang sudah pernah diaktifkan
    private HashSet<string> triggeredManaBlocks = new HashSet<string>();

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

    private string GetSceneSavePath(string sceneName)
    {
        return Path.Combine(Application.persistentDataPath, $"save_{sceneName}.json");
    }

    // ============================================================
    // 🟢 SAVE
    // ============================================================
    public void Save(Vector3 playerPos, int mana)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        SaveData data = new SaveData
        {
            sceneName = sceneName,
            playerX = playerPos.x,
            playerY = playerPos.y,
            mana = mana,
            triggeredManaBlocks = new List<string>(triggeredManaBlocks),

            // 🔹 tambahan untuk resource & shell
            shell = ShellManager.Instance != null ? ShellManager.Instance.shell : 0
        };

        // Simpan ke file
        string scenePath = GetSceneSavePath(sceneName);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        File.WriteAllText(scenePath, json);

        Debug.Log($"💾 Saved scene '{sceneName}' | Mana: {data.mana}, MaxMana: {data.maxMana}, SelectLimit: {data.selectionLimit}, shell: {data.shell}");
    }

    // ============================================================
    // 🟡 LOAD
    // ============================================================
    public SaveData Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data.triggeredManaBlocks != null)
                triggeredManaBlocks = new HashSet<string>(data.triggeredManaBlocks);

            // 🔹 Apply langsung ke ResourceManager dan ShellManager
            if (ShellManager.Instance != null)
            {
                ShellManager.Instance.LoadShellFromSave(data.shell);
            }

            Debug.Log($"✅ Loaded global save {triggeredManaBlocks.Count} ManaBlocks triggered)");
            return data;
        }

        Debug.LogWarning("⚠️ No global save file found.");
        return null;
    }

    // ============================================================
    // 🔵 RESTORE SAVE
    // ============================================================
    public SaveData RestoreSave()
    {
        SaveData data = Load();
        if (data == null) return null;

        string currentScene = SceneManager.GetActiveScene().name;
        if (data.sceneName != currentScene)
        {
            Debug.Log($"⚠️ Save ditemukan untuk scene '{data.sceneName}', bukan '{currentScene}'. Abaikan restore.");
            return null;
        }

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
            Debug.Log("🗑️ Deleted global save.json");
        }
    }

    public void DeleteAllSaves()
    {
        string dir = Application.persistentDataPath;
        string[] files = Directory.GetFiles(dir, "save_*.json");

        foreach (var file in files)
        {
            File.Delete(file);
            Debug.Log($"🗑️ Deleted: {Path.GetFileName(file)}");
        }

        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("🗑️ Deleted main save.json");
        }

        triggeredManaBlocks.Clear();
    }
}