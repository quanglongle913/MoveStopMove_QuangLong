using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private UIManager uIManager;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private VfxManager vfxManager;
    [SerializeField] private Player player;
    [SerializeField] private Transform playerStartPoint;
    [SerializeField] Camera mainCam;
    [SerializeField] Camera radarCam;
    [Header("BotAI: ")]
    [Tooltip("Number Bot in Map < Bot in Game")]
    [SerializeField] private int numberOfBotsOnMap;
    [Tooltip("Number Bot in Game > Bot in Map")]
    [SerializeField] private int numberOfBotsInGameLevel;
    [Header("GiftBox: ")]
    [Tooltip("Number GiftBox in Game")]
    [SerializeField] private int giftBoxNumber;
    [Header("Character Skins Data: ")]
    [SerializeField] private List<AccessoriesData> accessoriesDatas;
    
    [Header("Weapon Manager: ")]
    [SerializeField] private ObjectPool[] poolObject;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private WeaponSpawner weaponSpawner;
    [Header("Data Manager: ")]
    [SerializeField] private SaveData saveData;
    [SerializeField] private TextAsset BotAIData;
    [SerializeField] private TextAsset PlayerData;
    [SerializeField] private ZoneData zoneData;
    //[SerializeField] private List<GameObject> obstacles;

    private GameState gameState;
    private GameMode gameMode;

    private List<Indicator> indicatorList;
    private List<CharacterInfo> characterInfoList;
    private List<BotAI> botAIListEnable;
    private List<BotAI> botAIListStack;
    private List<GiftBox> listGiftBox;

    private List<AnimalAI> animalAIListEnable;
    private List<AnimalAI> animalAIListStack;

    private bool isInit, isInitIndicator, isInitBotAI;

    private int totalBotAI_InGame;
    private int totalBotAI;
    public int LevelExpAverage;
    public List<GameObject> newBloodsVfx;
    public List<GameObject> loodsVfx;
    //public List<GameObject> Obstacles { get => obstacles; set => obstacles = value; }
    public List<BotAI> BotAIListEnable { get => botAIListEnable; set => botAIListEnable = value; }
    public bool IsInit { get => isInit; set => isInit = value; }
    public GameState GameState { get => gameState; set => gameState = value; }

    public bool IsState(GameState gameState) => this.gameState == gameState;

    public List<Indicator> IndicatorList { get => indicatorList; set => indicatorList = value; }
    public bool IsInitIndicator { get => isInitIndicator; set => isInitIndicator = value; }
    public bool IsInitBotAI { get => isInitBotAI; set => isInitBotAI = value; }
    public int TotalBotAI_InGame { get => totalBotAI_InGame; set => totalBotAI_InGame = value; }
    public int TotalBotAI { get => totalBotAI; set => totalBotAI = value; }
    public List<CharacterInfo> CharacterInfoList { get => characterInfoList; set => characterInfoList = value; }
    public List<BotAI> BotAIListStack { get => botAIListStack; set => botAIListStack = value; }

    public ObjectPool[] PoolObject { get => poolObject; set => poolObject = value; }
    public WeaponData WeaponData { get => weaponData; set => weaponData = value; }
    public SaveData SaveData { get => saveData; set => saveData = value; }
    public GameObject WeaponHolder { get => weaponHolder; set => weaponHolder = value; }
    public UIManager UIManager { get => uIManager; set => uIManager = value; }
    public Player Player { get => player; set => player = value; }
    public int NumberOfBotsInGameLevel { get => numberOfBotsInGameLevel; set => numberOfBotsInGameLevel = value; }
    public ZoneData ZoneData { get => zoneData; set => zoneData = value; }
    public Transform PlayerStartPoint { get => playerStartPoint; set => playerStartPoint = value; }
    public List<GiftBox> ListGiftBox { get => listGiftBox; set => listGiftBox = value; }
    public int GiftBoxNumber { get => giftBoxNumber; set => giftBoxNumber = value; }
    public WeaponSpawner WeaponSpawner { get => weaponSpawner; set => weaponSpawner = value; }
    public SoundManager SoundManager { get => soundManager; set => soundManager = value; }
    public VfxManager VfxManager { get => vfxManager; set => vfxManager = value; }
    public Camera MainCam { get => mainCam; set => mainCam = value; }
    public Camera RadarCam { get => radarCam; set => radarCam = value; }
    public GameMode GameMode { get => gameMode; set => gameMode = value; }
    public List<AnimalAI> AnimalAIListEnable { get => animalAIListEnable; set => animalAIListEnable = value; }
    public List<AnimalAI> AnimalAIListStack { get => animalAIListStack; set => animalAIListStack = value; }
    public DataManager DataManager { get => dataManager; set => dataManager = value; }
    public List<AccessoriesData> AccessoriesDatas { get => accessoriesDatas; set => accessoriesDatas = value; }
   
    private void Start()
    {
        botAIListStack = new List<BotAI>();
        BotAIListEnable = new List<BotAI>();
        indicatorList = new List<Indicator>();
        characterInfoList = new List<CharacterInfo>();
        listGiftBox = new List<GiftBox>();
        newBloodsVfx = new List<GameObject>();
        SaveData.GenerateBotAIData(); // read Json Data Bot
        //Debug.Log(PlayerPrefs.GetInt(Constant.PLAYER_DATA_STATE, 0));
        if (PlayerPrefs.GetInt(Constant.PLAYER_DATA_STATE,0)==0)
        {
            UpdateData();
            PlayerPrefs.SetInt(Constant.PLAYER_DATA_STATE, 1);
            PlayerPrefs.Save();
        }
        else
        {
            //UNDONE
            //Set ScriptableObject data ......
            DataManager.ReadData();
            for (int i = 0; i < DataManager._PlayerData.weapons.Count; i++)
            {
                WeaponData.Weapon[i].WeaponType = DataManager._PlayerData.weapons[i].WeaponType;
                WeaponData.Weapon[i].WeaponName = DataManager._PlayerData.weapons[i].WeaponName;
                WeaponData.Weapon[i].WeaponPrice = DataManager._PlayerData.weapons[i].WeaponPrice;
                WeaponData.Weapon[i].Buyed = DataManager._PlayerData.weapons[i].Buyed;
                WeaponData.Weapon[i].Equipped = DataManager._PlayerData.weapons[i].Equipped;
            }
            for (int i = 0; i < DataManager._PlayerData.ListAccessoriesData.Count; i++)
            {
                //Debug.Log(DataManager._PlayerData.ListAccessoriesData[i].Accessories.Count);
                for (int j = 0; j < DataManager._PlayerData.ListAccessoriesData[i].Accessories.Count; j++)
                {
                    AccessoriesDatas[i].Accessories[j].Buyed = DataManager._PlayerData.ListAccessoriesData[i].Accessories[j].Buyed;
                    AccessoriesDatas[i].Accessories[j].Equipped = DataManager._PlayerData.ListAccessoriesData[i].Accessories[j].Equipped;
                    AccessoriesDatas[i].Accessories[j].Selected = DataManager._PlayerData.ListAccessoriesData[i].Accessories[j].Selected;
                }
            }
        }
        
        OnInit();
        //TEST
        //obstacles= GetObstacles();
        //Debug.Log("" + gameState);
    }
    public void OnInit()
    {
        StartCoroutine(Loading());
        //Debug.Log("" + gameState);
    }
    private void Update()
    {
        if (IsInitBotAI && gameState == GameState.Loading)
        {
            StartCoroutine(SetGameStateMenu());
        }
        if (gameState == GameState.InGame && IsInit && !Player.IsDeath)
        {
            if (gameMode == GameMode.Normal) 
            {

                if (BotAIListEnable.Count == 0 && BotAIListStack.Count == 0)
                {
                    //Player Won!.....
                    SoundManager.PlayEndWinSoundEffect();
                    Player.SetEndGame();
                    uIManager.setEndGame(true);
                    Player.FloatingJoystick.OnReset();
                }
                else
                {
                    LevelExpAverage = Player.InGamneExp;
                }
            }
            else if (gameMode == GameMode.Survival)
            {
                if (characterInfoList.Count > 0)
                {
                    for (int i = 0; i < characterInfoList.Count; i++)
                    {
                        if (characterInfoList[i].gameObject.activeSelf)
                        {
                            characterInfoList[i].GetComponent<PooledObject>().Release();
                        }

                    }
                }
                if (indicatorList.Count > 0)
                {
                    for (int i = 0; i < indicatorList.Count; i++)
                    {
                        if (indicatorList[i].gameObject.activeSelf)
                        {
                            indicatorList[i].GetComponent<PooledObject>().Release();
                        }
                       
                    }
                }
                if (this.Player.KilledCount == 1000)
                {
                    //Player Won!.....
                    SoundManager.PlayEndWinSoundEffect();
                    Player.SetEndGame();
                    uIManager.setEndGame(true);
                    Player.FloatingJoystick.OnReset();
                }
            }
        }
    }
    private List<GameObject> GetObstacles()
    {
        List<GameObject> newList = new List<GameObject>();
        var gameObjects = (TransparentObstacle[])GameObject.FindObjectsOfType(typeof(TransparentObstacle));
        for (int i = 0; i < gameObjects.Length; i++)
        {
            newList.Add(gameObjects[i].gameObject);
        }
        return newList;
    }
    IEnumerator SetGameStateMenu()
    {
        yield return new WaitForSeconds(0.1f);
        if (IsInitBotAI)
        {
            uIManager.setGameMenu();
        }
        else
        {
            StartCoroutine(SetGameStateMenu());
        }
        

    }
    IEnumerator Loading()
    {
        if (botAIListEnable.Count > 0)
        {
            for (int i = 0; i < botAIListEnable.Count; i++)
            {
                botAIListEnable[i].Agent.ResetPath();
                botAIListEnable[i].ChangeState(new IdleState());
                botAIListEnable[i].GetComponent<PooledObject>().Release();
            }
        }
        if (characterInfoList.Count > 0)
        {
            for (int i = 0; i < characterInfoList.Count; i++)
            {
                characterInfoList[i].GetComponent<PooledObject>().Release();
            }
        }
        if (indicatorList.Count > 0)
        {
            for (int i = 0; i < indicatorList.Count; i++)
            {
                indicatorList[i].GetComponent<PooledObject>().Release();
            }
        }
        //Clear Animal
        if (animalAIListEnable!=null && animalAIListEnable.Count > 0)
        {
            for (int i = 0; i < animalAIListEnable.Count; i++)
            {
                animalAIListEnable[i].Agent.ResetPath();
                animalAIListEnable[i].ChangeState(new IdleStateAnimal());
                animalAIListEnable[i].GetComponent<PooledObject>().Release();
            }
        }
        yield return new WaitForSeconds(0.5f);
        botAIListStack.Clear();
        botAIListEnable.Clear();
        indicatorList.Clear();
        characterInfoList.Clear();
        if (animalAIListEnable != null )
        {
            animalAIListEnable.Clear();
        }
        
        IsInit = false;
        isInitIndicator = false;
        IsInitBotAI = false;
        totalBotAI = numberOfBotsInGameLevel;
        totalBotAI_InGame = numberOfBotsOnMap;
        gameState = GameState.Loading;
        gameMode = GameMode.Normal;
        uIManager.Loading();
    }
    public void UpdateData()
    {
        Debug.Log("UpdateData");
        this.DataManager.GenerateData(this.WeaponData, this.AccessoriesDatas);
    }
}
