using UnityEngine;
using TMPro;

public class ShellManager : MonoBehaviour
{
    public static ShellManager Instance;

    [Header("Shell Settings")]
    public int shell;

    [Header("UI Reference")]
    public TMP_Text shellText;

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
    }

    public bool SpendShell(int amount)
    {
        if (shell >= amount)
        {
            shell -= amount;
            UpdateShellUI();
            SaveSystem.Instance?.Save();
            return true;
        }
        return false;
    }

    void UpdateShellUI()
    {
        if (shellText != null)
        {
            shellText.text = "Shell: " + shell.ToString();
        }
    }

    public void LoadShellFromSave(int savedShell)
    {
        shell = Mathf.Max(0, savedShell);
        UpdateShellUI();
        Debug.Log($"Loaded Shell: {shell}");
    }
}
