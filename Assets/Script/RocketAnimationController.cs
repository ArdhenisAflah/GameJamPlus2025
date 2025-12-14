using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAnimationController : MonoBehaviour
{
    [Header("Driver Animator (Parent)")]
    public Animator driverAnimator;

    [Header("Rocket Skins (Child)")]
    public GameObject[] skins;


    private Animator activeSkinAnimator;
    private bool lastBoostStatus = false;

    public ParticleSystem particleRocket;




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

    int currentLevel;

    int currentSkin;


    // ================ INIT SKIN ================
    void InitSkin()
    {

        if (skins[currentSkin].TryGetComponent<Animator>(out var anim))
        {
            activeSkinAnimator = anim;
        }


        Debug.LogWarning("AKU PAKE " + currentSkin);
    }

    // ================ BOOST ANIMATION ================
    public void PlayBoost(bool isBoosting, bool isStopEffect = true)
    {
        // Hindari spam (Animator ignore transition kalau dipanggil setiap frame)
        if (isBoosting == lastBoostStatus)
            return;

        lastBoostStatus = isBoosting;

        // Parent
        if (driverAnimator != null)
        {
            driverAnimator.SetBool("IsBoosting", isBoosting);

            if (isStopEffect == true)
            {
                //play particle system here.
                particleRocket.Play();
            }
            else
            {
                particleRocket.Stop();
            }
        }


        // Skin active
        if (activeSkinAnimator != null)
            activeSkinAnimator.SetBool("IsBoosting", isBoosting);
    }

    private void OnEnable()
    {
        UpgradeManager.OnLoaded += HandleLoaded;
    }

    private void OnDisable()
    {
        UpgradeManager.OnLoaded -= HandleLoaded;
    }

    private void OnDestroy()
    {
        UpgradeManager.OnLoaded -= HandleLoaded; // double safety
    }

    private void HandleLoaded()
    {
        RefreshVisuals();
        InitSkin();
    }
    void HandleApplied()
    {
        RefreshVisuals();
        InitSkin();
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


    public void RefreshVisuals()
    {
        currentLevel = GetCurrentLevelFromManager();


        // Nyalakan yang baru
        Debug.LogWarning(currentLevel);

        for (int i = 0; i < visualUpgrades.Count; i++)
        {
            // Nyalakan yang baru
            if (currentLevel >= visualUpgrades[i].requiredLevel)
            {
                currentSkin = i;
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
}
