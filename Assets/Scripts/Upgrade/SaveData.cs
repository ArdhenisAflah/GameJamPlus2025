using System;

[Serializable]
public class SaveData
{
    public int shell;

    public int levelLaunch = 1;
    public int levelBoost = 1;
    public int levelFuel = 1;
    public int levelWall = 1;

    public float upwardBoost = 30f;
    public float forwardBoost = 6f;

    public float maxFuel = 1f;
    public float fuelBurnRate = 0.4f;
    public float fuelRegenRate = 0f;

    public float groundSpeedLoss = 0.5f;

    public float slowResistance = 0f;

    public float launchUpwardForce = 10f;
    public float launchForwardForce = 4f;
    public float launchControlDelay = 1f;   
}