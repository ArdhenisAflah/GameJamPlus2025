using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEntry : MonoBehaviour
{
    public UpgradeType upgradeType;
    public TMP_Text levelText;
    public TMP_Text costText;
    public Button upgradeButton;

    [Header("Slider Reference")]
    public CroppedSlider levelSlider;

    public int maxLevel = 8;
    public int baseCost = 50;

    [System.Serializable]
    public struct SerializableNode
    {
        public int level;
        public GameObject[] unlockItem;
        public GameObject[] disableItem;
    }

    [SerializeField]
    public List<SerializableNode> PassingGradeUpgradeUnlockItem = new List<SerializableNode>();

    public static UpgradeEntry Instance;

    private void Start()
    {
        if (levelSlider != null)
        {
            levelSlider.SetMaxLevel(maxLevel);
            levelSlider.SetLevelInstant(GetCurrentLevel());
        }

        UpdateUI();
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeButton);
        }
    }

    private void OnEnable()
    {
        // Berlangganan event perubahan shell dari ShellManager
        ShellManager.OnShellChanged += HandleShellChanged;

        UpdateUI();
    }

    private void OnDisable()
    {
        // Berhenti berlangganan saat disabled untuk mencegah memory leak
        ShellManager.OnShellChanged -= HandleShellChanged;
    }

    private void HandleShellChanged(int currentShells)
    {
        // Refresh UI & status tombol saat jumlah shell berubah
        UpdateUI();
    }

    public void UpdateUI()
    {
        int currentLevel = GetCurrentLevel();
        int cost = GetCost();

        if (levelText != null)
        {
            levelText.text = $"{currentLevel}/{maxLevel}";
        }

        if (levelSlider != null)
        {
            levelSlider.SetLevel(currentLevel);
        }

        if (costText != null)
        {
            if (currentLevel < maxLevel)
            {
                costText.text = $"Cost: {cost}";
            }
            else
            {
                costText.text = "Cost: MAX";
            }
        }

        ReachingLevelUnlock();

        // Update status interactable tombol berdasarkan kecukupan shell & max level
        if (upgradeButton != null)
        {
            upgradeButton.gameObject.SetActive(true);

            bool canAfford = ShellManager.Instance != null && ShellManager.Instance.CheckAvaiable(cost);
            bool isNotMaxLevel = currentLevel < maxLevel;

            upgradeButton.interactable = isNotMaxLevel && canAfford;
        }
    }

    public void ReachingLevelUnlock()
    {
        int currentLevel = GetCurrentLevel();

        foreach (SerializableNode sn in PassingGradeUpgradeUnlockItem)
        {
            if (currentLevel >= sn.level)
            {
                if (sn.disableItem != null)
                {
                    foreach (GameObject itemDisable in sn.disableItem)
                    {
                        if (itemDisable != null)
                            itemDisable.SetActive(false);
                    }
                }

                if (sn.unlockItem != null)
                {
                    foreach (GameObject itemEnable in sn.unlockItem)
                    {
                        if (itemEnable != null)
                            itemEnable.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnUpgradeButton()
    {
        int cost = GetCost();

        if (ShellManager.Instance != null && ShellManager.Instance.SpendShell(cost))
        {
            ApplyUpgrade();
            UpdateUI(); // Refresh state tombol ini (level bertambah, cost naik, dan cek affordability)
        }
        else
        {
            Debug.Log("Not enough Shell!");
            UpdateUI();
        }
    }

    int GetCost()
    {
        return baseCost * (GetCurrentLevel());
    }

    int GetCurrentLevel()
    {
        if (UpgradeManager.Instance == null) return 1;

        switch (upgradeType)
        {
            case UpgradeType.Launch: return UpgradeManager.Instance.levelLaunch;
            case UpgradeType.Boost: return UpgradeManager.Instance.levelBoost;
            case UpgradeType.Fuel: return UpgradeManager.Instance.levelFuel;
            case UpgradeType.Wall: return UpgradeManager.Instance.levelWall;
        }

        return 0;
    }

    void ApplyUpgrade()
    {
        if (UpgradeManager.Instance == null) return;

        switch (upgradeType)
        {
            case UpgradeType.Launch:
                UpgradeManager.Instance.AddLaunch();
                break;

            case UpgradeType.Boost:
                UpgradeManager.Instance.AddBoost();
                break;

            case UpgradeType.Fuel:
                UpgradeManager.Instance.AddFuel();
                break;

            case UpgradeType.Wall:
                UpgradeManager.Instance.AddWall();
                break;
        }
    }
}