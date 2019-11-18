using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour
{
    private static PlayerConfig _config;
    private static AudioSource BGM;
    public static bool BGMPlay
    {
        get { return _config.BgmAutoPlay; }
        set
        {
            _config.BgmAutoPlay = value;
            if (_config.BgmAutoPlay) BGM.Play();
            else BGM.Pause();
        }
    }
    protected void Awake()
    {
        _config = ConfigManager.LoadSettingsFromJsonFile();
        BGM = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (_config.BgmAutoPlay) BGM.Play();
        else BGM.Pause();
    }

    private void OnApplicationQuit()
    {
        ConfigManager.SaveSettingsFromJsonFile(_config);
    }

    public void ToExit() { Application.Quit(); }

    public void ToMenu() { SceneManager.LoadScene("Menu"); }
    public void ToSelect() { SceneManager.LoadScene("Select"); }
    public void ToSettings() { SceneManager.LoadScene("Settings"); }
    public void ToHelp() { SceneManager.LoadScene("Help"); }
    public void ToHome() { SceneManager.LoadScene("Home"); }
    public void ToAbout() { SceneManager.LoadScene("About"); }

    public static void SToSelect() { SceneManager.LoadScene("Select"); }
    public static void SToRound(int index)
    {
        RoundxController.SelectRoundIndex = index;
        SceneManager.LoadScene("Roundx");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Input.GetKeyDown:::KeyCode.Escape");
        }
    }
}
