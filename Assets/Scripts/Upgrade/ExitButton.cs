using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button exitButton;

    private void Start()
    {
        exitButton.onClick.AddListener(OnExitButton);
    }

    public void OnExitButton()
    {
        UpgradeManager.Instance.ExitUpgradePanel();
    }
}
