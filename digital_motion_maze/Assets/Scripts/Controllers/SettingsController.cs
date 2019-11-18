using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public GameObject ShowBox;
    public GameObject ConfirmationBox;

    private void Start()
    {
        ConfirmationBox.SetActive(false);
        ShowBox.SetActive(false);
    }

    public void ClearPlayerConfig()
    {
        ConfirmationBox.SetActive(true);
    }

    public void ConfirmClearPlayerConfig()
    {
        ConfigManager.ClearPlayerConfig();
        ConfirmationBox.SetActive(false);
        ShowBox.SetActive(true);
    }

    public void CancelClearPlayerConfig()
    {
        ConfirmationBox.SetActive(false);
    }

    public void CloseShowBox() { ShowBox.SetActive(false); }
}
