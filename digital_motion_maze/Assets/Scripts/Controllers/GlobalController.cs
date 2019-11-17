using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    protected void Awake() { DontDestroyOnLoad(this.gameObject); }

    public void ToExit() { Application.Quit(); }

    public void ToMenu() { SceneManager.LoadScene("Menu"); }
    public void ToSelect() { SceneManager.LoadScene("Select"); }
    public void ToSettings() { SceneManager.LoadScene("Settings"); }
    public void ToHelp() { SceneManager.LoadScene("Help"); }
    public void ToHome() { SceneManager.LoadScene("Home"); }

    public void ToAbout() { SceneManager.LoadScene("About"); }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Input.GetKeyDown:::KeyCode.Escape");
        }
    }
}
