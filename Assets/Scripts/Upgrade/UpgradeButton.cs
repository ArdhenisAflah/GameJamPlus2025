using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class UpgradeEntry : MonoBehaviour
{
    public UpgradeType upgradeType;
    public TMP_Text levelText;
    public Button upgradeButton;

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
        UpdateUI();

        upgradeButton.onClick.AddListener(OnUpgradeButton);
    }

    void UpdateUI()
    {
        levelText.text = $"Level: {GetCurrentLevel()}/{maxLevel}";
        int cost = GetCost();

        //always check for level reaching
        ReachingLevelUnlock();
        if (ShellManager.Instance.CheckAvaiable(cost))
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = false;
        }
        // upgradeButton.interactable = GetCurrentLevel() < maxLevel;
    }


    public void ReachingLevelUnlock()
    {
        foreach (SerializableNode sn in PassingGradeUpgradeUnlockItem)
        {
            //check apakah level saat ini tidak cocok untuk membuka item yang diperlukan....
            if (GetCurrentLevel() == sn.level)
            {
                sn.disableItem.SetActive(false);
                //unlock that element.
                sn.unlockItem.SetActive(true);
            }
        }
    }



    private void OnEnable()
    {
        int cost = GetCost();

        if (ShellManager.Instance.CheckAvaiable(cost))
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = true;
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