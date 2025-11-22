using UnityEngine;

public class RocketStats : MonoBehaviour
{
    [Header("Boost Power")]
    public float upwardBoost = 30f;
    public float forwardBoost = 6f;

    [Header("Fuel System")]
    public float maxFuel = 1f;
    public float fuelBurnRate = 0.4f;
    public float fuelRegenRate = 0f;

    [Header("Ground Hit")]
    public float groundSpeedLoss = 0.5f;

    [Header("Slow Resistance")]
    public float slowResistance = 0f;

    [Header("Launch Settings")]
    public float launchUpwardForce = 10f;
    public float launchForwardForce = 4f;
    public float launchControlDelay = 1f;   
}
