using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

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


        // Nyalakan yang baru
        Debug.LogWarning(currentLevel);

        for (int i = 0; i < visualUpgrades.Count; i++)
        {
            // Nyalakan yang baru
            if (currentLevel >= visualUpgrades[i].requiredLevel)
            {

                foreach (var obj in visualUpgrades[i].objectsToActivate)
                {
                    if (obj != null)
                    {

                        obj.SetActive(true);
                    }
                }

                // Matikan yang lama
                foreach (var obj in visualUpgrades[i].objectsToDisable)
                {
                    if (obj != null) obj.SetActive(false);
                }
                return;
            }
        }
    }

    IEnumerator GetDataAfterX()
    {
        yield return new WaitForSeconds(0.5f);
        RefreshVisuals();

    }

    private void OnEnable()
    {
        StartCoroutine(GetDataAfterX());
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
