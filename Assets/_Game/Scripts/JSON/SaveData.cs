using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;


public class SaveData : MonoBehaviour
{

    [Header("Data Readed")]
    [SerializeField] private BotAIData botAIData = new BotAIData();
    [Header("Data input for write")]
    [SerializeField] private int BotAIGenerate;
    [SerializeField] private int numberOfWeapon;
    [SerializeField] private List<CharacterSkin> _characterSkin = new List<CharacterSkin>();
    string saveFile;

    public BotAIData BotAIData { get => botAIData; set => botAIData = value; }

    public void ReadJsonFile()
    {
        saveFile = Application.dataPath + "/StreamingAssets/BotAIData.json";
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Work with JSON
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            //  into a pattern matching the GameData class.
            botAIData = JsonUtility.FromJson<BotAIData>(fileContents);

            //Debug.Log("BotAIData._botAIInfo:" + botAIData.botAIInfo.Count);
            //Debug.Log("BotAIData._botAIInfo 1:" + botAIData.botAIInfo[1].botAI_name);
            //SaveData.BotAIData.botAIInfo[i]._characterSkin[1].index
        }
    }
    public void SaveIntoJson()
    {
        saveFile = Application.dataPath + "/StreamingAssets/BotAIData.json";
        GenerateData();
        string potion = JsonUtility.ToJson(botAIData);
        //System.IO.File.WriteAllText(Application.streamingAssetsPath + "/BotAIData.json", potion);
        File.WriteAllText(saveFile, potion);
    }
    public void SaveIntoJson2()
    {
        saveFile = Application.dataPath + "/StreamingAssets/BotAIData.json";
        GenerateDataSetIndex();
        string potion = JsonUtility.ToJson(botAIData);
        //System.IO.File.WriteAllText(Application.streamingAssetsPath + "/BotAIData.json", potion);
        File.WriteAllText(saveFile, potion);
    }
    public void GenerateData()
    {
        //Tạo data và kiểu BotAIInfo.weapon là số weapon có trong game
        //CharacterSkin[i].index là số skin loại 'i' có trong game
        List<BotAIInfo> _botAIInfo = new List<BotAIInfo>();
        for (int i = 0; i < BotAIGenerate; i++)
        {
            BotAIInfo botAIInfo = new BotAIInfo();
            botAIInfo.botAI_name = "" + (BotAINameType)i;
            botAIInfo.weapon = numberOfWeapon;
            botAIInfo._characterSkin = _characterSkin;
            _botAIInfo.Add(botAIInfo);
        }
        botAIData.botAIInfo = _botAIInfo;
    }
    //Tạo data và kiểu BotAIInfo.weapon là weapon INDEX mà BOT dùng
    //CharacterSkin[i].index là số skin loại 'i' có trong game
    public void GenerateDataSetIndex()
    {
        List<BotAIInfo> _botAIInfo = new List<BotAIInfo>();
        for (int i = 0; i < BotAIGenerate; i++)
        {
            BotAIInfo botAIInfo = new BotAIInfo();
            botAIInfo.botAI_name = "" + (BotAINameType)i;
            int randomWeapon = UnityEngine.Random.Range(0, numberOfWeapon);
            botAIInfo.weapon = randomWeapon;
            List<CharacterSkin> characterSkin = _characterSkin;
            for (int j = 0; j < characterSkin.Count; j++)
            {
                int randomSkin = UnityEngine.Random.Range(0, characterSkin[j].index);
                characterSkin[j].index = randomSkin;
            }

            botAIInfo._characterSkin = characterSkin;
            _botAIInfo.Add(botAIInfo);
        }
        botAIData.botAIInfo = _botAIInfo;
        
    }
}


[System.Serializable]
public class BotAIData
{
    public List<BotAIInfo> botAIInfo = new List<BotAIInfo>();
}
[System.Serializable]
public class BotAIInfo
{
    public string botAI_name;
    public int weapon;//là số weapon có trong game
    public List<CharacterSkin> _characterSkin = new List<CharacterSkin>();
}

[System.Serializable]
public class CharacterSkin
{
    public string skin_name;
    public int index; //số skin loại 'i' có trong game
    //public List<BotAIAccessories> _botAIAccessories = new List<BotAIAccessories>();
}
/*[System.Serializable]
public class BotAIAccessories{
    public string _name;
    public int index;
}*/
