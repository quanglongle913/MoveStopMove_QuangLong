using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameState gameState;

    private UIManager uIManager;
    private BotAIManager botAIManager;
    private IndicatorManager indicatorManager;
    //public static GameManager Instance;
    public GameState GameState { get => gameState; set => gameState = value; }
    //public bool IsInit { get => isInit; set => isInit = value; }

    private void Start()
    {
        botAIManager = BotAIManager.Instance;
        indicatorManager = IndicatorManager.Instance;
        uIManager = UIManager.Instance;
        //gameState = (GameState)PlayerPrefs.GetInt(Constant.GAME_STATE, 0);
        //gameState = GameState.Loading;
        gameState = GameState.Loading;
        uIManager.Loading();
        Debug.Log("" + gameState);
    }
    private void Update()
    {
        if (botAIManager.IsInit && gameState == GameState.Loading)
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
