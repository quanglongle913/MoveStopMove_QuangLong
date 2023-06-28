using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Player player;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private SoundManager soundManager;
    [Header("Data Manager: ")]
    [SerializeField] private BotData botData;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private ZoneData zoneData;
    [Header("Character Skins Data: ")]
    [SerializeField] private List<AccessoriesData> accessoriesDatas;

    private GameState gameState;
    private GameMode gameMode;
    private void Start()
    {
        botData.GenerateBotAIData();
        ChangeState(GameState.Loading);
        //botData.GenerateBotAIData();
        if (PlayerPrefs.GetInt(Constant.PLAYER_DATA_STATE, 0) == 0)
        {
            UpdateData();
            PlayerPrefs.SetInt(Constant.PLAYER_DATA_STATE, 1);
            PlayerPrefs.Save();
        }
        else
        {
            //UNDONE
            //Set ScriptableObject data ......
            dataManager.ReadData();
            for (int i = 0; i < dataManager._PlayerData.weapons.Count; i++)
            {
                weaponData.Weapon[i].WeaponType = dataManager._PlayerData.weapons[i].WeaponType;
                weaponData.Weapon[i].WeaponName = dataManager._PlayerData.weapons[i].WeaponName;
                weaponData.Weapon[i].WeaponPrice = dataManager._PlayerData.weapons[i].WeaponPrice;
                weaponData.Weapon[i].Buyed = dataManager._PlayerData.weapons[i].Buyed;
                weaponData.Weapon[i].Equipped = dataManager._PlayerData.weapons[i].Equipped;
            }
            for (int i = 0; i < dataManager._PlayerData.ListAccessoriesData.Count; i++)
            {
                //Debug.Log(DataManager._PlayerData.ListAccessoriesData[i].Accessories.Count);
                for (int j = 0; j < dataManager._PlayerData.ListAccessoriesData[i].Accessories.Count; j++)
                {
                    accessoriesDatas[i].Accessories[j].Buyed = dataManager._PlayerData.ListAccessoriesData[i].Accessories[j].Buyed;
                    accessoriesDatas[i].Accessories[j].Equipped = dataManager._PlayerData.ListAccessoriesData[i].Accessories[j].Equipped;
                    accessoriesDatas[i].Accessories[j].Selected = dataManager._PlayerData.ListAccessoriesData[i].Accessories[j].Selected;
                }
            }
        }
    }
    public WeaponData GetWeaponData()
    {
        return weaponData;
    }
    public BotAIInfo GetBotAIInfo(int index)
    {
        return botData.BotAIData.BotAIInfo[index];
    }
    public List<AccessoriesData> GetAccessoriesDatas()
    {
        return accessoriesDatas;
    }
    public void UpdateData()
    {
        Debug.Log("UpdateData");
        this.dataManager.GenerateData(this.weaponData, this.accessoriesDatas);
    }
    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }
    public void ChangeMode(GameMode gameMode)
    {
        this.gameMode = gameMode;
    }

    public bool IsState(GameState gameState)
    {
        return this.gameState == gameState;
    }
    public bool IsMode(GameMode gameMode)
    {
        return this.gameMode == gameMode;
    }
    public void HidePlayer()
    {
        player.gameObject.SetActive(false);
    }
    public void ShowPlayer()
    {
        player.gameObject.SetActive(true);
    }
    public Transform PlayerTF()
    {
        return player.transform;
    }
    public Camera GetCamera()
    {
        return mainCam;
    }
    public Player Player()
    {
        return player;
    }
    public int GetBotCount()
    {
        return levelManager.GetBotCount();
    }
    public void RemoveBotAIs(BotAI botAI)
    {
        levelManager.GetBotAIs().Remove(botAI);
    }
    public void RemoveAnimals(Animal animal)
    {
        levelManager.GetAnimalsInGame().Remove(animal);
    }
    public LevelManager LevelManager()
    {
        return levelManager;
    }
    public SoundManager SoundManager()
    {
        return soundManager;
    }
    public ZoneData ZoneData()
    {
        return zoneData;
    }
}
