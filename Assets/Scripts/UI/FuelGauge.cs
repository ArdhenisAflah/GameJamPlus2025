using UnityEngine;
using UnityEngine.UI;

public class FuelGauge : MonoBehaviour
{
    public Image fullGauge;

    public void UpdateFuel(float currentFuel, float maxFuel)
    {
        fullGauge.fillAmount = Mathf.Clamp01(currentFuel / maxFuel);
    }
}
