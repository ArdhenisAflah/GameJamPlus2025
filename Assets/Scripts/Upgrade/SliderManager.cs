using UnityEngine;
using UnityEngine.UI;

public class CroppedSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image filler;
    [SerializeField] private Image background;
    [SerializeField] private Image frame;
    
    [Header("Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float currentValue = 1f;
    [SerializeField] private float minValue = 0f;
    [SerializeField] private float maxValue = 1f;
    
    [Header("Animation")]
    [SerializeField] private bool useAnimation = true;
    [SerializeField] private float animationDuration = 0.3f;
    
    [Header("Level System")]
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int maxLevel = 8;
    
    private void Start()
    {
        UpdateSlider(currentValue);
    }
    
    private void OnValidate()
    {
        if (filler != null)
        {
            UpdateSlider(currentValue);
        }
    }
    
    public void SetValue(float value)
    {
        float targetValue = Mathf.Clamp01(value);
        
        if (useAnimation && Application.isPlaying)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateCoroutine(targetValue, animationDuration));
        }
        else
        {
            currentValue = targetValue;
            UpdateSlider(currentValue);
        }
    }
    
    public void SetValueInstant(float value)
    {
        StopAllCoroutines();
        currentValue = Mathf.Clamp01(value);
        UpdateSlider(currentValue);
    }
    
    public void SetLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 0, maxLevel);
        float normalizedValue = (float)currentLevel / (float)maxLevel;
        SetValue(normalizedValue);
    }
    
    public void SetLevelInstant(int level)
    {
        currentLevel = Mathf.Clamp(level, 0, maxLevel);
        float normalizedValue = (float)currentLevel / (float)maxLevel;
        SetValueInstant(normalizedValue);
    }
    
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    
    public void SetMaxLevel(int max)
    {
        maxLevel = max;
    }
    
    public void SetValueWithRange(float value, float min, float max)
    {
        minValue = min;
        maxValue = max;
        float normalizedValue = Mathf.InverseLerp(min, max, value);
        SetValue(normalizedValue);
    }
    
    public float GetValue()
    {
        return currentValue;
    }
    
    public float GetValueInRange()
    {
        return Mathf.Lerp(minValue, maxValue, currentValue);
    }
    
    private void UpdateSlider(float normalizedValue)
    {
        if (filler == null) return;
        
        filler.fillAmount = normalizedValue;
        filler.fillMethod = Image.FillMethod.Horizontal;
        filler.fillOrigin = (int)Image.OriginHorizontal.Left;
        filler.type = Image.Type.Filled;
    }
    
    public void AddValue(float amount)
    {
        SetValue(currentValue + amount);
    }
    
    public void SubtractValue(float amount)
    {
        SetValue(currentValue - amount);
    }
    
    public void AddValueInRange(float amount)
    {
        float currentInRange = GetValueInRange();
        SetValueWithRange(currentInRange + amount, minValue, maxValue);
    }
    
    public void SubtractValueInRange(float amount)
    {
        float currentInRange = GetValueInRange();
        SetValueWithRange(currentInRange - amount, minValue, maxValue);
    }
    
    public void AnimateToValue(float targetValue, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateCoroutine(targetValue, duration));
    }
    
    public void SetAnimationEnabled(bool enabled)
    {
        useAnimation = enabled;
    }
    
    public void SetAnimationDuration(float duration)
    {
        animationDuration = duration;
    }
    
    private System.Collections.IEnumerator AnimateCoroutine(float targetValue, float duration)
    {
        float startValue = currentValue;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            currentValue = Mathf.Lerp(startValue, targetValue, t);
            UpdateSlider(currentValue);
            yield return null;
        }
        
        currentValue = targetValue;
        UpdateSlider(currentValue);
    }
}