using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DataManager;

public class DataManager : MonoBehaviour
{
    string saveFile;
    private PlayerData playerData= new PlayerData();
    public PlayerData _PlayerData { get => playerData; set => playerData = value; }
    public string SaveFile { get => saveFile; set => saveFile = value; }
    public void ReadData()
    {
        saveFile = Constant.GetStreamingAssetsPath(Constant.JSON_DATA_FILENAME_PLAYER);
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Work with JSON
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);
            //  into a pattern matching the GameData class.
            _PlayerData = JsonUtility.FromJson<PlayerData>(fileContents);
        }
    }
    public void SaveData()
    {
        saveFile = Constant.GetStreamingAssetsPath(Constant.JSON_DATA_FILENAME_PLAYER);
        string potion = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFile, potion);
    }

    public void GenerateData(WeaponData weaponData, List<AccessoriesData> accessoriesDatas)
    {
        PlayerData playerData = new PlayerData();
        playerData.Player_Name = "You";
        playerData.weapons = new List<Weapon1>();
        playerData.ListAccessoriesData = new List<AccessoriesData1>();

        for (int i = 0; i < weaponData.Weapon.Count; i++)
        {
            Weapon1 weapon = new Weapon1();
            weapon.WeaponType = weaponData.Weapon[i].WeaponType;
            weapon.WeaponName = weaponData.Weapon[i].WeaponName;
            weapon.WeaponPrice = weaponData.Weapon[i].WeaponPrice;
            weapon.Buyed = weaponData.Weapon[i].Buyed;
            weapon.Equipped = weaponData.Weapon[i].Equipped;

            playerData.weapons.Add(weapon);
        }
        
        for (int i = 0; i < accessoriesDatas.Count; i++)
        {
            AccessoriesData1 accessoriesData1 = new AccessoriesData1();
            accessoriesData1.Accessories = new List<Accessories1>();
            accessoriesData1.SkinType = accessoriesDatas[i].SkinType;
            for (int j = 0; j < accessoriesDatas[i].Accessories.Count; j++)
            {
                Accessories1 accessories = new Accessories1();
                accessories.AccessoriesName = accessoriesDatas[i].Accessories[j].AccessoriesName;
                accessories.Selected = accessoriesDatas[i].Accessories[j].Selected;
                accessories.Buyed = accessoriesDatas[i].Accessories[j].Buyed;
                accessories.Equipped = accessoriesDatas[i].Accessories[j].Equipped;
                accessoriesData1.Accessories.Add(accessories);
            }
            playerData.ListAccessoriesData.Add(accessoriesData1);
        }
        _PlayerData = playerData;
        SaveData();
    }
    [System.Serializable]
    public class PlayerData
    {
        public string Player_Name;
        public List<Weapon1> weapons = new List<Weapon1>();
        public PlayerSkinShopState playerSkinShopState;
        public List<AccessoriesData1> ListAccessoriesData= new List<AccessoriesData1>();
    }
    [System.Serializable]
    public class Weapon1
    {
        //[SerializeField] GameObject weaponPrefabs;
        [SerializeField] string weaponName;
        [SerializeField] WeaponType weaponType;
        [SerializeField] int weaponPrice;
        [SerializeField] private bool equipped = false;
        [SerializeField] private bool buyed = false;


        public string WeaponName { get => weaponName; set => weaponName = value; }
        public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
        public int WeaponPrice { get => weaponPrice; set => weaponPrice = value; }
        public bool Equipped { get => equipped; set => equipped = value; }
        public bool Buyed { get => buyed; set => buyed = value; }
    }
    [System.Serializable]
    public class AccessoriesData1
    {
        [SerializeField] private SkinType skinType;
        [SerializeField] private List<Accessories1> accessories;

        public SkinType SkinType { get => skinType; set => skinType = value; }
        public List<Accessories1> Accessories { get => accessories; set => accessories = value; }
    }
    [System.Serializable]
    public class Accessories1
    {
        [SerializeField] private string accessoriesName;
        [SerializeField] private bool selected = false;
        [SerializeField] private bool buyed = false;
        [SerializeField] private bool equipped = false;

        public string AccessoriesName { get => accessoriesName; set => accessoriesName = value; }
        public bool Selected { get => selected; set => selected = value; }
        public bool Buyed { get => buyed; set => buyed = value; }
        public bool Equipped { get => equipped; set => equipped = value; }
    }
}
