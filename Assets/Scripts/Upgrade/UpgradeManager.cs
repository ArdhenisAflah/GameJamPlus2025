using UnityEngine;
using TMPro;

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
