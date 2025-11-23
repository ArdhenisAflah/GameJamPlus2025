using UnityEngine;

public class RocketAnimationController : MonoBehaviour
{
    [Header("Driver Animator (Parent)")]
    public Animator driverAnimator;

    [Header("Rocket Skins (Child)")]
    public GameObject[] skins;
    public int selectedSkin = 0;

    private Animator activeSkinAnimator;
    private bool lastBoostStatus = false;

    public ParticleSystem particleRocket;

    void Start()
    {
        InitSkin();
    }

    // ================ INIT SKIN ================
    void InitSkin()
    {
        for (int i = 0; i < skins.Length; i++)
            skins[i].SetActive(false);

        skins[selectedSkin].SetActive(true);

        activeSkinAnimator = skins[selectedSkin].GetComponent<Animator>();
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
}
