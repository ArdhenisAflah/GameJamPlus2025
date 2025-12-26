using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
        public GameObject unlockItem;
        public GameObject disableItem;
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
        upgradeButton.onClick.AddListener(OnUpgradeButton);
    }

    void UpdateUI()
    {
        int currentLevel = GetCurrentLevel();
        levelText.text = $"{currentLevel}/{maxLevel}";
        int cost = GetCost();

        if (levelSlider != null)
        {
            levelSlider.SetLevel(currentLevel);
        }

        if (costText != null)
        {
            if (currentLevel < maxLevel)
            {
                costText.text = $"Cost: {cost.ToString()}"; 
            }
            else
            {
                costText.text = "Cost: MAX"; 
            }
        }

        ReachingLevelUnlock();
        
        if (ShellManager.Instance.CheckAvaiable(cost))
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = currentLevel < maxLevel;
        }
        else
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = false;
        }
    }

    public void ReachingLevelUnlock()
    {
        foreach (SerializableNode sn in PassingGradeUpgradeUnlockItem)
        {
            if (GetCurrentLevel() == sn.level)
            {
                if (sn.disableItem != null)
                    sn.disableItem.SetActive(false);
                
                if (sn.unlockItem != null)
                    sn.unlockItem.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        int cost = GetCost();
        int currentLevel = GetCurrentLevel();

        if (levelSlider != null)
        {
            levelSlider.SetLevelInstant(currentLevel);
        }

        if (costText != null)
        {
            if (currentLevel < maxLevel)
            {
                costText.text = cost.ToString();
            }
            else
            {
                costText.text = "MAX";
            }
        }

        if (ShellManager.Instance.CheckAvaiable(cost))
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = currentLevel < maxLevel;
        }
        else
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = false;
        }
    }

    void OnUpgradeButton()
    {
        int cost = GetCost();

        if (ShellManager.Instance.SpendShell(cost))
        {
            ApplyUpgrade();
            UpdateUI();
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