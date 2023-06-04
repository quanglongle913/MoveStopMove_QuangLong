using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private int numberOfBotsOnMap;
    [SerializeField] private int numberOfBotsInGameLvel;

    [SerializeField] private GameState gameState;
    [SerializeField] private UIManager uIManager;

    private List<Indicator> indicatorList;
    private List<CharacterInfo> characterInfoList;
    private List<BotAI> botAIListEnable;
    private List<BotAI> botAIListStack;

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

    private void Start()
    {
        botAIListStack = new List<BotAI>();
        BotAIListEnable = new List<BotAI>();
        IndicatorList = new List<Indicator>();
        CharacterInfoList = new List<CharacterInfo>();
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
        /*if (gameState == GameState.Loading)
        {
            Debug.Log("Loading.....");
        }*/
        if (IsInitBotAI && gameState == GameState.Loading)
        {
            
            StartCoroutine(SetGameStateMenu());
        }
        if (BotAIListEnable.Count == 0 && gameState == GameState.InGame && IsInit)
        {
            uIManager.setEndGame(); 
        }
    }
    IEnumerator SetGameStateMenu()
    {
        //Loading time 1.5s
        yield return new WaitForSeconds(1.5f);
        gameState = GameState.GameMenu;
        uIManager.GameMenu();
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
