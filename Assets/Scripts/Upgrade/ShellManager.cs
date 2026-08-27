using System;
using TMPro;
using UnityEngine;

public class ShellManager : MonoBehaviour
{
    public static ShellManager Instance;

    [Header("Shell Settings")]
    public int shell;
    public TMP_Text shellText;

    // Event terpanggil setiap kali jumlah shell berubah (Add, Spend, Load)
    public static event Action<int> OnShellChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateShellUI();
    }

    public void AddShell(int amount)
    {
        shell += amount;
        UpdateShellUI();
        SaveSystem.Instance?.Save();
        OnShellChanged?.Invoke(shell);
    }

    public bool SpendShell(int amount)
    {
        if (shell >= amount)
        {
            shell -= amount;
            UpdateShellUI();
            SaveSystem.Instance?.Save();
            OnShellChanged?.Invoke(shell);
            return true;
        }
        return false;
    }

    public bool CheckAvaiable(int amount)
    {
        return shell >= amount;
    }

    void UpdateShellUI()
    {
        if (shellText != null)
        {
            shellText.text = $"{shell}";
        }
    }

    public void LoadShellFromSave(int savedShell)
    {
        shell = Mathf.Max(0, savedShell);
        UpdateShellUI();
        OnShellChanged?.Invoke(shell);
        Debug.Log($"Loaded Shell: {shell}");
    }
}
