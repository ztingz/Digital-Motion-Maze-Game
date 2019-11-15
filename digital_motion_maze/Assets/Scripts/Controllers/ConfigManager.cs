using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class ConfigManager
{
    private const string SETTINGS_PATH_TAIL = "/Configs/settings.json";
    private const string ROUND_PATH_FORMATE = "/Configs/round{0}.json";

    //从配置文件载入玩家配置
    public static PlayerConfig LoadSettingsFromJsonFile()
    {
        Resources.Load(SETTINGS_PATH_TAIL).ToString();
        string json = File.ReadAllText(
            UnityEngine.Application.dataPath + SETTINGS_PATH_TAIL,
            System.Text.Encoding.UTF8
        );
        return JsonUtility.FromJson<PlayerConfig>(json);
    }

    public static void SaveSettingsFromJsonFile(PlayerConfig playerConfig)
    {
        string json = JsonUtility.ToJson(playerConfig, true);//易读格式
        File.WriteAllText(
            UnityEngine.Application.dataPath + SETTINGS_PATH_TAIL,
            json, System.Text.Encoding.UTF8
        );
        Debug.Log(json);
    }

    //从配置文件载入关卡信息
    public static Round LoadRoundFromJsonFile(int num)
    {
        // Debug.Log(Resources.Load<TextAsset>(string.Format(ROUND_PATH_FORMATE, num)));
        string json = System.IO.File.ReadAllText(
            UnityEngine.Application.dataPath + string.Format(ROUND_PATH_FORMATE, num),
            System.Text.Encoding.UTF8
        );
        // StreamReader reader = new StreamReader(UnityEngine.Application.dataPath + string.Format(ROUND_PATH_FORMATE, num));
        // string json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<Round>(json);
    }
}
