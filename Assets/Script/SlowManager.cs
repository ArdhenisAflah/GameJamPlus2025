using UnityEngine;
using System.Collections.Generic;

public class SlowManager : MonoBehaviour
{
    [Header("Multiplier Limits")]
    public float minMultiplier = 0.1f;
    public float maxMultiplier = 1f;

    [Header("Recovery Settings")]
    public float recoveryRate = 1f;

    [HideInInspector]
    public float FinalMultiplier = 1f;

    private RocketStats stats;
    private List<SlowSource> activeSlows = new List<SlowSource>();

    void Start()
    {
        stats = GetComponent<RocketStats>();
        if (stats == null)
            Debug.LogWarning("SlowManager: RocketStats tidak ditemukan!");
    }

    void Update()
    {
        float slowResistance = (stats != null) ? stats.slowResistance : 0f;

        float slowTotal = 0f;

        for (int i = activeSlows.Count - 1; i >= 0; i--)
        {
            var s = activeSlows[i];

            if (s.insideZone && s.stacking)
                s.Stack(Time.deltaTime);

            if (!s.insideZone)
                s.currentSlow = Mathf.MoveTowards(s.currentSlow, 0f, recoveryRate * Time.deltaTime);

            if (s.currentSlow <= 0f)
            {
                activeSlows.RemoveAt(i);
                continue;
            }

            float reducedSlow = s.currentSlow * (1f - slowResistance);
            slowTotal += reducedSlow;
        }

        slowTotal = Mathf.Clamp(slowTotal, 0f, 0.95f);

        FinalMultiplier = Mathf.Clamp(1f - slowTotal, minMultiplier, maxMultiplier);
    }

    public SlowSource AddSlow(float enterAmount, float stackRate)
    {
        SlowSource s = new SlowSource(enterAmount, stackRate);
        activeSlows.Add(s);
        return s;
    }
}

public class SlowSource
{
    public float currentSlow;
    public float stackRate;
    public bool insideZone = true;

    public bool stacking => stackRate > 0f;

    public SlowSource(float enter, float stack)
    {
        currentSlow = enter;
        stackRate = stack;
    }

    public void Stack(float dt)
    {
        currentSlow += stackRate * dt;
    }
}
