using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameState gameState;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private Player player;
    [SerializeField] private Transform playerStartPoint;
    [Header("BotAI: ")]
    [Tooltip("Number Bot in Map < Bot in Game")]
    [SerializeField] private int numberOfBotsOnMap;
    [Tooltip("Number Bot in Game > Bot in Map")]
    [SerializeField] private int numberOfBotsInGameLvel;
    [Header("GiftBox: ")]
    [Tooltip("Number GiftBox in Game")]
    [SerializeField] private int giftBoxNumber;
    [Header("Character Skins Data: ")]
    [SerializeField] private AccessoriesData hatsData;
    [SerializeField] private AccessoriesData pantsData;
    [SerializeField] private AccessoriesData setfullData;
    [SerializeField] private AccessoriesData shieldData;
    
    [Header("Weapon Manager: ")]
    [SerializeField] private ObjectPool[] poolObject;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject weaponManager;
    [Header("Data Manager: ")]
    [SerializeField] private SaveData saveData;
    [SerializeField] private ZoneData zoneData;

    private List<Indicator> indicatorList;
    private List<CharacterInfo> characterInfoList;
    private List<BotAI> botAIListEnable;
    private List<BotAI> botAIListStack;
    private List<GiftBox> listGiftBox;

    private bool isInit, isInitIndicator, isInitBotAI;

    private int totalBotAI_InGame;
    private int totalBotAI;
    public List<BotAI> BotAIListEnable { get => botAIListEnable; set => botAIListEnable = value; }
    public bool IsInit { get => isInit; set => isInit = value; }
    public GameState GameState { get => gameState; set => gameState = value; }

    public List<Indicator> IndicatorList { get => indicatorList; set => indicatorList = value; }
    public bool IsInitIndicator { get => isInitIndicator; set => isInitIndicator = value; }
    public bool IsInitBotAI { get => isInitBotAI; set => isInitBotAI = value; }
    public int TotalBotAI_InGame { get => totalBotAI_InGame; set => totalBotAI_InGame = value; }
    public int TotalBotAI { get => totalBotAI; set => totalBotAI = value; }
    public List<CharacterInfo> CharacterInfoList { get => characterInfoList; set => characterInfoList = value; }
    public List<BotAI> BotAIListStack { get => botAIListStack; set => botAIListStack = value; }

    public AccessoriesData PantsData { get => pantsData; set => pantsData = value; }
    public AccessoriesData SetfullData { get => setfullData; set => setfullData = value; }
    public AccessoriesData ShieldData { get => shieldData; set => shieldData = value; }
    public AccessoriesData HatsData { get => hatsData; set => hatsData = value; }

    public ObjectPool[] PoolObject { get => poolObject; set => poolObject = value; }
    public WeaponData WeaponData { get => weaponData; set => weaponData = value; }
    public SaveData SaveData { get => saveData; set => saveData = value; }
    public GameObject WeaponManager { get => weaponManager; set => weaponManager = value; }
    public UIManager UIManager { get => uIManager; set => uIManager = value; }
    public Player Player { get => player; set => player = value; }
    public int NumberOfBotsInGameLvel { get => numberOfBotsInGameLvel; set => numberOfBotsInGameLvel = value; }
    public ZoneData ZoneData { get => zoneData; set => zoneData = value; }
    public Transform PlayerStartPoint { get => playerStartPoint; set => playerStartPoint = value; }
    public List<GiftBox> ListGiftBox { get => listGiftBox; set => listGiftBox = value; }
    public int GiftBoxNumber { get => giftBoxNumber; set => giftBoxNumber = value; }

    private void Start()
    {
        botAIListStack = new List<BotAI>();
        BotAIListEnable = new List<BotAI>();
        indicatorList = new List<Indicator>();
        characterInfoList = new List<CharacterInfo>();
        listGiftBox = new List<GiftBox>();
        SaveData.ReadJsonFile(); // read Json Data Bot
        OnInit();
       
        //Debug.Log("" + gameState);
    }
    public void OnInit()
    {
        StartCoroutine(loading());
        //Debug.Log("" + gameState);
    }
    private void Update()
    {
        if (IsInitBotAI && gameState == GameState.Loading)
        {
            
            StartCoroutine(SetGameStateMenu());
        }
        if (BotAIListEnable.Count == 0 && gameState == GameState.InGame && IsInit && !Player.IsDeath)
        {
            //Player Won!.....
            Player.SetEndGame();
            uIManager.setEndGame(true);
            Player.FloatingJoystick.OnReset();
        }
    }
    IEnumerator SetGameStateMenu()
    {
        //Loading time 1.5s
        
        yield return new WaitForSeconds(0.5f);
        uIManager.setGameMenu();

    }
    IEnumerator loading()
    {
        if (botAIListEnable.Count > 0)
        {
            for (int i = 0; i < botAIListEnable.Count; i++)
            {
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
        yield return new WaitForSeconds(0.5f);
        botAIListStack.Clear();
        botAIListEnable.Clear();
        indicatorList.Clear();
        characterInfoList.Clear();
        IsInit = false;
        isInitIndicator = false;
        IsInitBotAI = false;
        totalBotAI = numberOfBotsInGameLvel;
        totalBotAI_InGame = numberOfBotsOnMap;
        gameState = GameState.Loading;
        uIManager.Loading();
    }
}
