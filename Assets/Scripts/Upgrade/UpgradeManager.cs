using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Upgrade Settings")]
    public int levelLaunch = 1;
    public int levelBoost = 1;
    public int levelFuel = 1;
    public int levelWall = 1;

    [Header("UI Reference")]
    public TMP_Text shellText;

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

    void Start()
    {
        UpdateUpgradeUI();
    }

    public void AddLaunch()
    {
        levelLaunch += 1;
        UpdateUpgradeUI();
        SaveSystem.Instance?.Save();
    }

    public void AddBoost()
    {
        levelBoost += 1;
        UpdateUpgradeUI();
        SaveSystem.Instance?.Save();
    }

    public void AddFuel()
    {
        levelFuel += 1;
        UpdateUpgradeUI();
        SaveSystem.Instance?.Save();
    }

    public void AddWall()
    {
        levelWall += 1;
        UpdateUpgradeUI();
        SaveSystem.Instance?.Save();
    }

    void UpdateUpgradeUI()
    {
        if (shellText != null)
        {
            // shellText.text = "Shell: " + shell.ToString();
        }
    }

    public void LoadUpgradeFromSave(int savedShell)
    {
        // shell = Mathf.Max(0, savedShell);
        UpdateUpgradeUI();
        // Debug.Log($"Loaded Upgrades: {shell}");
    }
}
