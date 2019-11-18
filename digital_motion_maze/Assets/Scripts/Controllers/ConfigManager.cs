using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class ConfigManager
{
    private const string CONFIG_DIR_PATH = "/Configs/";
    private const string SETTINGS_PATH_TAIL = "/Configs/settings.json";
    private const string ROUNDS_PATH = "/Configs/rounds.json";
    private const string ROUND_PATH_FORMATE = "/Configs/round{0}.json";
    private const string RECORDS_PATH = "/Configs/records.json";

    //从配置文件载入关卡记录信息
    public static List<Record> LoadRecords()
    {
        string json = File.ReadAllText(
            UnityEngine.Application.dataPath + RECORDS_PATH,
            System.Text.Encoding.UTF8
        );
        return JsonConvert.DeserializeObject<List<Record>>(json);
    }

    public static Record LoadRecord(int index)
    {
        string json = File.ReadAllText(
            UnityEngine.Application.dataPath + RECORDS_PATH,
            System.Text.Encoding.UTF8
        );
        return JsonConvert.DeserializeObject<List<Record>>(json)[index];
    }

    public static void SaveRecord(Record record)
    {
        List<Record> records = LoadRecords();

        if (records != null)
        {
            if (records.Count > record.RoundIndex)
            {
                if (record.StarNumber > records[record.RoundIndex].StarNumber)
                    records[record.RoundIndex] = record;
                else if (record.StarNumber == records[record.RoundIndex].StarNumber)
                    if (record.Step > records[record.RoundIndex].Step)
                        records[record.RoundIndex] = record;
                    else if (record.Time > records[record.RoundIndex].Time)
                        records[record.RoundIndex] = record;
            }
            else records.Add(record);
        }
        else
        {
            records = new List<Record>();
            records.Add(record);
        }


        string json = JsonConvert.SerializeObject(records);
        File.WriteAllText(
            UnityEngine.Application.dataPath + RECORDS_PATH,
            json, System.Text.Encoding.UTF8
        );
    }

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

    //从配置文件载入关卡数量信息
    public static int LoadRoundNumber()
    {
        return Directory.GetFileSystemEntries(Application.dataPath + CONFIG_DIR_PATH, "round*.json").Length;
    }

    //从配置文件载入某一关卡的信息
    public static Round LoadRoundFromJsonFile(int num)
    {
        string json = File.ReadAllText(
            UnityEngine.Application.dataPath + string.Format(ROUND_PATH_FORMATE, num),
            System.Text.Encoding.UTF8
        );
        return JsonConvert.DeserializeObject<Round>(json);
    }
}
