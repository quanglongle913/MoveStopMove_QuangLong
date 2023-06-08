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
            botAIInfo.BotAI_name = "" + (BotAINameType)i;
            botAIInfo.Weapon = numberOfWeapon;
            botAIInfo.CharacterSkin = _characterSkin;
            _botAIInfo.Add(botAIInfo);
        }
        botAIData.BotAIInfo = _botAIInfo;
    }
    //Tạo data và kiểu BotAIInfo.weapon là weapon INDEX mà BOT dùng
    //CharacterSkin[i].index là số skin loại 'i' có trong game
    public void GenerateDataSetIndex()
    {
        List<BotAIInfo> _botAIInfo = new List<BotAIInfo>();
        for (int i = 0; i < BotAIGenerate; i++)
        {
            BotAIInfo botAIInfo = new BotAIInfo();
            botAIInfo.BotAI_name = "" + (BotAINameType)i;
            int randomWeapon = UnityEngine.Random.Range(0, numberOfWeapon);
            botAIInfo.Weapon = randomWeapon;
            List<CharacterSkin> characterSkin = new List<CharacterSkin>();

            for (int j = 0; j < _characterSkin.Count; j++)
            {
                int randomSkin = UnityEngine.Random.Range(0, _characterSkin[j].Index);
                CharacterSkin temp = new CharacterSkin();
                temp.Index = randomSkin;
                temp.Skin_name = _characterSkin[j].Skin_name;
                characterSkin.Add(temp);

            }


            botAIInfo.CharacterSkin = characterSkin;
            _botAIInfo.Add(botAIInfo);
        }
        botAIData.BotAIInfo = _botAIInfo;
        
    }
}


[System.Serializable]
public class BotAIData
{
    public List<BotAIInfo> BotAIInfo = new List<BotAIInfo>();
}
[System.Serializable]
public class BotAIInfo
{
    public string BotAI_name;
    public int Weapon;//là số weapon index trong game
    public List<CharacterSkin> CharacterSkin = new List<CharacterSkin>();
}

[System.Serializable]
public class CharacterSkin
{
    [SerializeField] private string skin_name;
    [SerializeField] private int index; //số skin index  trong game

    public string Skin_name { get => skin_name; set => skin_name = value; }
    public int Index { get => index; set => index = value; }
    //public List<BotAIAccessories> _botAIAccessories = new List<BotAIAccessories>();
}
/*[System.Serializable]
public class BotAIAccessories{
    public string _name;
    public int index;
}*/
