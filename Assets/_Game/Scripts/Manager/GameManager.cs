using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private int totalBotAI_InGame;
    [SerializeField] private int totalBotAI;

    [SerializeField] private GameState gameState;
    [SerializeField] private UIManager uIManager;

    private List<Indicator> indicatorList;
    private List<CharacterInfo> characterInfoList;
    private List<BotAI> botAIListEnable;
    private List<BotAI> botAIListStack;

    private bool isInit, isInitIndicator, isInitBotAI;


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
        IsInit = false;
        isInitIndicator=false;
        IsInitBotAI=false;
        botAIListStack =new List<BotAI>();
        BotAIListEnable = new List<BotAI>();
        IndicatorList= new List<Indicator>();
        CharacterInfoList = new List<CharacterInfo>();
        gameState = GameState.Loading;
        uIManager.Loading();
        Debug.Log("" + gameState);
    }
    private void Update()
    {
        if (IsInitBotAI && gameState == GameState.Loading)
        {
            StartCoroutine(SetGameStateMenu());
        }
    }
    IEnumerator SetGameStateMenu()
    {
        //Loading time 1.5s
        yield return new WaitForSeconds(1.5f);
        gameState = GameState.GameMenu;
        uIManager.GameMenu();
    }
}
