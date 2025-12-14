using UnityEngine;
using TMPro;
using System;

public class UpgradeManager : MonoBehaviour
{

    public static UpgradeManager Instance;

    [Header("UI Panels")]
    public GameObject upgradePanel;

    [Header("Upgrade Settings")]
    public int levelBoost = 1;
    public int levelLaunch = 1;
    public int levelFuel = 1;
    public int levelWall = 1;


    public static event Action OnLoaded;

    // ================== APPLY FROM SAVE ==================
    public void ApplyFromSave(SaveData data)
    {
        if (data == null)
        {
            Debug.LogWarning("ApplyFromSave called with NULL data, using defaults");
            OnLoaded?.Invoke();
            return;
        }

        levelBoost = data.levelBoost;
        levelLaunch = data.levelLaunch;
        levelFuel = data.levelFuel;
        levelWall = data.levelWall;
        OnLoaded?.Invoke();
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddLaunch()
    {
        levelLaunch += 1;
        SaveSystem.Instance?.Save();
    }

    public void AddBoost()
    {
        levelBoost += 1;
        SaveSystem.Instance?.Save();
    }

    public void AddFuel()
    {
        levelFuel += 1;
        SaveSystem.Instance?.Save();
    }

    public void AddWall()
    {
        levelWall += 1;
        SaveSystem.Instance?.Save();
    }

    public void ExitUpgradePanel()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        SaveSystem.Instance?.Save();
    }

    public void LoadUpgradeFromSave(int savedShell)
    {
        // shell = Mathf.Max(0, savedShell);
        // Debug.Log($"Loaded Upgrades: {shell}");
    }
}
