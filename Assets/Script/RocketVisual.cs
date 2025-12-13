using UnityEngine;
using System.Collections.Generic;

public class RocketVisual : MonoBehaviour
{

    [System.Serializable]
    public struct VisualGradeNode
    {
        public int requiredLevel;
        public GameObject[] objectsToActivate;
        public GameObject[] objectsToDisable;
    }

    [Header("Settings")]
    public UpgradeType typeToWatch = UpgradeType.Boost;

    [SerializeField]
    public List<VisualGradeNode> visualUpgrades = new List<VisualGradeNode>();


    public void RefreshVisuals()
    {

        int currentLevel = GetCurrentLevelFromManager();


        for (int i = visualUpgrades.Count - 1; i >= 0; i--)
        {

            if (currentLevel >= visualUpgrades[i].requiredLevel)
            {
                // Nyalakan yang baru
                foreach (var obj in visualUpgrades[i].objectsToActivate)
                {
                    if (obj != null) obj.SetActive(true);
                }

                // Matikan yang lama
                foreach (var obj in visualUpgrades[i].objectsToDisable)
                {
                    if (obj != null) obj.SetActive(false);
                }

                
            }
        }
    }

    private void OnEnable()
    {
        RefreshVisuals();
    }

    int GetCurrentLevelFromManager()
    {
        if (UpgradeManager.Instance == null) return 1;

        switch (typeToWatch)
        {
            case UpgradeType.Launch: return UpgradeManager.Instance.levelLaunch;
            case UpgradeType.Boost: return UpgradeManager.Instance.levelBoost;
            case UpgradeType.Fuel: return UpgradeManager.Instance.levelFuel;
            case UpgradeType.Wall: return UpgradeManager.Instance.levelWall;
            default: return 1;
        }
    }
}