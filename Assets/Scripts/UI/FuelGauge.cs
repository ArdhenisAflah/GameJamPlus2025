using UnityEngine;
using UnityEngine.UI;

public class FuelGauge : MonoBehaviour
{
    [Header("References")]
    public Image fuelFillImage;

    [Header("Fuel Settings")]
    public float maxFuel = 100f;
    public float currentFuel = 100f;

    void Start()
    {
        UpdateFuel(maxFuel, maxFuel);
    }

    public void UpdateFuel(float fuel, float newMaxFuel)
    {
        currentFuel = Mathf.Clamp(fuel, 0, newMaxFuel);

        float fillAmount = currentFuel / newMaxFuel;

        fuelFillImage.fillAmount = fillAmount;
    }
}
