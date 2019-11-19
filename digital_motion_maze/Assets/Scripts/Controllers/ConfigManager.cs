using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

public static class ConfigManager
{
    private const string CONFIG_DIR_PATH = "/Configs/";
    private const string SETTINGS_NAME = "settings.json";
    private const string ROUND_NAME_FORMATE = "round{0}.json";
    private const string RECORDS_NAME = "records.json";

    //将StreamingAssets中的预载配置文件拷贝到PersistentData中, 方便读写
    private static string CopyFileFromStreamingAssetsToPersistentData(string name)
    {
        WWW www = new WWW(Application.streamingAssetsPath + CONFIG_DIR_PATH + name);

        string dir_path = Application.persistentDataPath + CONFIG_DIR_PATH;
        // Debug.Log(dir_path + name);
        if (!Directory.Exists(dir_path))
            Directory.CreateDirectory(dir_path);

        string file_path = Application.persistentDataPath + CONFIG_DIR_PATH + name;
        // if (!File.Exists(file_path))
        FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine(www.text);
        sw.Close();
        return file_path;
    }

    //玩家配置文件
    public static PlayerConfig LoadSettingsFromJsonFile()
    {
        string path = Application.persistentDataPath + CONFIG_DIR_PATH + SETTINGS_NAME;
        if (!File.Exists(path))
            path = CopyFileFromStreamingAssetsToPersistentData(SETTINGS_NAME);
        string json = File.ReadAllText(path, Encoding.UTF8);
        return JsonConvert.DeserializeObject<PlayerConfig>(json);
    }
    public static void SaveSettingsFromJsonFile(PlayerConfig playerConfig)
    {
        string json = JsonConvert.SerializeObject(playerConfig);
        File.WriteAllText(
            Application.persistentDataPath + CONFIG_DIR_PATH + SETTINGS_NAME,
            json, Encoding.UTF8
        );
    }

    //关卡记录信息
    public static List<Record> LoadRecords()
    {
        string path = Application.persistentDataPath + CONFIG_DIR_PATH + RECORDS_NAME;
        if (!File.Exists(path))
            path = CopyFileFromStreamingAssetsToPersistentData(RECORDS_NAME);
        string json = File.ReadAllText(path, Encoding.UTF8);
        return JsonConvert.DeserializeObject<List<Record>>(json);
    }
    public static Record LoadRecord(int index)
    {
        string path = Application.persistentDataPath + CONFIG_DIR_PATH + RECORDS_NAME;
        if (!File.Exists(path))
            path = CopyFileFromStreamingAssetsToPersistentData(RECORDS_NAME);
        string json = File.ReadAllText(path, Encoding.UTF8);
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
            Application.persistentDataPath + CONFIG_DIR_PATH + RECORDS_NAME,
            json, Encoding.UTF8
        );
    }


    //关卡数量信息
    public static int LoadRoundNumber()
    {
        string path = Application.persistentDataPath + CONFIG_DIR_PATH + SETTINGS_NAME;
        if (!File.Exists(path))
            path = CopyFileFromStreamingAssetsToPersistentData(SETTINGS_NAME);
        string json = File.ReadAllText(path, Encoding.UTF8);
        return JsonConvert.DeserializeObject<PlayerConfig>(json).RoundNumber;
        // return Directory.GetFileSystemEntries(Application.persistentDataPath + CONFIG_DIR_PATH, "round*.json").Length;
    }

    //关卡配置文件
    public static Round LoadRoundFromJsonFile(int num)
    {
        string path = Application.persistentDataPath + CONFIG_DIR_PATH + string.Format(ROUND_NAME_FORMATE, num);
        if (!File.Exists(path))
            path = CopyFileFromStreamingAssetsToPersistentData(string.Format(ROUND_NAME_FORMATE, num));
        string json = File.ReadAllText(path, Encoding.UTF8);
        return JsonConvert.DeserializeObject<Round>(json);
    }

    public static void ClearPlayerConfig()
    {
        string path = Application.persistentDataPath + CONFIG_DIR_PATH + RECORDS_NAME;
        if (!File.Exists(path))
            path = CopyFileFromStreamingAssetsToPersistentData(RECORDS_NAME);
        File.WriteAllText(path, string.Empty, Encoding.UTF8);
    }
}
