using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    private string savePath;

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
            levelLaunch = UpgradeManager.Instance.levelLaunch,
            levelBoost  = UpgradeManager.Instance.levelBoost,
            levelFuel   = UpgradeManager.Instance.levelFuel,
            levelWall   = UpgradeManager.Instance.levelWall,
            shell       = ShellManager.Instance.shell
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Saved game.");
    }

    // ====================
    // LOAD
    // ====================
    public SaveData Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No Global Save file found.");
            return null;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        Debug.Log("Loaded save.json");
        return data;
    }

    // ====================
    // RESTORE SAVE
    // ====================
    public SaveData RestoreSave()
    {
        SaveData data = Load();
        if (data == null) return null;

        // --- Restore Shell ---
        ShellManager.Instance?.LoadShellFromSave(data.shell);

        // --- Restore Upgrade Levels ---
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.levelLaunch = data.levelLaunch;
            UpgradeManager.Instance.levelBoost  = data.levelBoost;
            UpgradeManager.Instance.levelFuel   = data.levelFuel;
            UpgradeManager.Instance.levelWall   = data.levelWall;
        }

        // --- APPLY STATS DIRECTLY TO ROCKET ---
        ApplyStatsToRocket();

        Debug.Log("All upgrades restored and stats applied to Rocket!");

        return data;
    }

    private void ApplyStatsToRocket()
    {
        RocketStats stats = FindObjectOfType<RocketStats>();
        if (stats == null)
        {
            Debug.LogWarning("RocketStats not found in scene!");
            return;
        }

        // ========= APPLY MULTIPLIERS =========
        int Llaunch = UpgradeManager.Instance.levelLaunch - 1;
        int Lboost  = UpgradeManager.Instance.levelBoost - 1;
        int Lfuel   = UpgradeManager.Instance.levelFuel - 1;
        int Lwall   = UpgradeManager.Instance.levelWall - 1;

        // Base values
        float baseLaunchUp     = 6f;
        float baseLaunchForward= 6f;

        float baseBoostUp      = 300f;
        float baseBoostForward = 500f;

        float baseFuel         = 1f;

        float baseSlowResist   = 0f;

        // Per level adds
        float launchUpPerLevel       = 0f;
        float launchForwardPerLevel  = 6f;

        float boostUpPerLevel        = 0f;
        float boostForwardPerLevel   = 50f;

        float fuelPerLevel           = 0.5f;

        float slowResistPerLevel     = 0.1f;

        // ========= APPLY VALUES =========
        stats.launchUpwardForce  = baseLaunchUp     + (Llaunch * launchUpPerLevel);
        stats.launchForwardForce = baseLaunchForward+ (Llaunch * launchForwardPerLevel);

        stats.upwardBoost        = baseBoostUp      + (Lboost * boostUpPerLevel);
        stats.forwardBoost       = baseBoostForward + (Lboost * boostForwardPerLevel);

        stats.maxFuel            = baseFuel         + (Lfuel * fuelPerLevel);

        stats.slowResistance     = baseSlowResist   + (Lwall * slowResistPerLevel);
        stats.slowResistance     = Mathf.Clamp(stats.slowResistance, 0f, 1f);

        Debug.Log("🔥 Stats applied directly from SaveSystem");
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Deleted Global Save.json");
        }
    }
}
