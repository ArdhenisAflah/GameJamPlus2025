using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeEntry : MonoBehaviour
{
    public UpgradeType upgradeType;
    public TMP_Text levelText;
    public Button upgradeButton;

    public int maxLevel = 8;
    public int baseCost = 50;

    private void Start()
    {
        UpdateUI();
        upgradeButton.onClick.AddListener(OnUpgradeButton);
    }

    void UpdateUI()
    {
        levelText.text = $"Level: {GetCurrentLevel()}/{maxLevel}";

        upgradeButton.interactable = GetCurrentLevel() < maxLevel;
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
            case UpgradeType.Boost:  return UpgradeManager.Instance.levelBoost;
            case UpgradeType.Fuel:   return UpgradeManager.Instance.levelFuel;
            case UpgradeType.Wall:   return UpgradeManager.Instance.levelWall;
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