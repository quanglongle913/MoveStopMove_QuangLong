using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameState gameState;

    [SerializeField] private UIManager uIManager;

    private List<Indicator> indicatorList;
    private List<BotAI> botAIList;
    private bool isInit,isInitIndicator,isInitBotAI;


    public List<BotAI> BotAIList { get => botAIList; set => botAIList = value; }
    public bool IsInit { get => isInit; set => isInit = value; }
    public GameState GameState { get => gameState; set => gameState = value; }
    public List<Indicator> IndicatorList { get => indicatorList; set => indicatorList = value; }
    public bool IsInitIndicator { get => isInitIndicator; set => isInitIndicator = value; }
    public bool IsInitBotAI { get => isInitBotAI; set => isInitBotAI = value; }

    private void Start()
    {
        IsInit = false;
        isInitIndicator=false;
        IsInitBotAI=false;
        BotAIList = new List<BotAI>();
        IndicatorList= new List<Indicator>();
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
