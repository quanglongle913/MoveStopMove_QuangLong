using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    [Header("Normal:")]
    [SerializeField] private List<Level> levelPrefabs;
    
    [SerializeField] float offset;
    [SerializeField] private float size_x;
    [SerializeField] private float size_z;
    [SerializeField] private Player player;
    [Header("Survival:")]
    [SerializeField] private List<Level> survivalPrefabs;
    //-----------------
    private Level currentLevel;
    //Normal
    private List<BotAI> bots = new List<BotAI>();
    private List<BotAI> botsInGame = new List<BotAI>();
    private List<GiftBox> giftBoxs = new List<GiftBox>();
    private int levelIndex;
    private int botAmount;
    private int botInGame;
    private int botInStack;
    //----------------
    //Survival
    private int survivalIndex;
    //---------------
    private float screenBoundOffset = 0.9f;
    private Vector3 screenCentre;
    private Vector3 screenBounds;

    private void Awake()
    {
        levelIndex = PlayerPrefs.GetInt(Constant.LEVEL, 0);
    }

    private void Start()
    {
        //OnStart Game
        LoadLevel(levelIndex);
        UIManager.Instance.OpenUI<Loading>();
        OnInit();
        screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenBounds = screenCentre * screenBoundOffset;

    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsMode(GameMode.Normal))
        {
            if (GameManager.Instance.IsState(GameState.InGame))
            {
                BotUpdate(bots);
                PlayerUpdate();
                if (botInStack > 0)
                {
                    for (int i = 0; i < bots.Count; i++)
                    {
                        if (!bots[i].gameObject.activeSelf && !bots[i].IsDeath && botsInGame.Count < botInGame)
                        {
                            bots[i].gameObject.SetActive(true);
                            bots[i].ChangeState(new PatrolState());
                            botsInGame.Add(bots[i]);
                            botInStack--;
                        }
                    }
                }
                else if (botsInGame.Count == 0)
                {
                    UIManager.Instance.OpenUI<Win>();
                    UIManager.Instance.CloseUI<InGame>();
                }
                if (giftBoxs.Count < 1)
                {
                    GenerateGiftBox(1, GeneratePoolObjectPosition(transform.position, botAmount));
                }
            }
        } else if (GameManager.Instance.IsMode(GameMode.Survival))
        { 
            
        }
        
    }

    

    public void OnInit()
    {
        player.OnInit();
        botAmount = levelPrefabs[levelIndex].GetBotAmount();
        botInGame = levelPrefabs[levelIndex].GetBotInGame();
        botInStack = botAmount;
        //update navmesh data
        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(currentLevel.GetNavMeshData());

        GenerateBotAI(botAmount, GeneratePoolObjectPosition(transform.position, botAmount));
        player.transform.position = levelPrefabs[levelIndex].GetStartPoint().position;
        PlayerInit();

    }
    public void LoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        if (level < levelPrefabs.Count)
        {
            currentLevel = Instantiate(levelPrefabs[level]);
            //currentLevel.OnInit();
        }
        else
        {
            //TODO: level vuot qua limit
        }
    }
    private void PlayerUpdate()
    {
        if (!player.IsDeath)
        {
            if (player.InCamera(GameManager.Instance.GetCamera()))
            {
                player.CharacterInfo.UpdateData();
            }
            else
            {
                player.CharacterInfo.Hide();
            }
        }
        else
        {
            player.CharacterInfo.Hide();
        }
    }
    private void BotUpdate(List<BotAI> botsAI) 
    {
        for (int i = 0; i < botsAI.Count; i++)
        {
            BotAI bot = botsAI[i];
            if (!bot.IsDeath && bot.gameObject.activeSelf)
            {
                if (bot.InCamera(GameManager.Instance.GetCamera()))
                {
                    bot.Indicator.Hide();
                    bot.CharacterInfo.UpdateData();
                }
                else
                {
                    bot.CharacterInfo.Hide();
                    bot.Indicator.UpdateData(OffScreenIndicatorCore.GetScreenPosition(GameManager.Instance.GetCamera(), bot.gameObject, screenCentre, screenBounds));

                }
            }
            else
            {
                bot.Indicator.Hide();
                bot.CharacterInfo.Hide();
            }

        }
    }
    private void GenerateBotAI(int index, List<Vector3> listPoolObjectPosition)
    {
        for (int i = 0; i < index; i++)
        {
            int randomIndex = Random.Range(0, listPoolObjectPosition.Count);
            BotAI bot = SimplePool.Spawn<BotAI>(PoolType.Bot, listPoolObjectPosition[randomIndex], Quaternion.identity);
            bot.OnInit();
            bot.UpdateInfo(GameManager.Instance.GetBotAIInfo(i), GameManager.Instance.GetAccessoriesDatas());

            Indicator indicator = SimplePool.Spawn<Indicator>(PoolType.Indicator);
            indicator.SetCharacter(bot.GetComponent<Character>());
            indicator.gameObject.SetActive(false);
            bot.Indicator = indicator;

            CharacterInfo characterInfo = SimplePool.Spawn<CharacterInfo>(PoolType.CharacterInfo);
            characterInfo.SetCharacter(bot.GetComponent<Character>());
            characterInfo.gameObject.SetActive(false);
            bot.CharacterInfo = characterInfo;
            bot.gameObject.SetActive(false);
            bots.Add(bot);
        }
    }
    private void GenerateGiftBox(int index, List<Vector3> listPoolObjectPosition)
    {
        for (int i = 0; i < index; i++)
        {
            int randomIndex = Random.Range(0, listPoolObjectPosition.Count);
            GiftBox giftBox = SimplePool.Spawn<GiftBox>(PoolType.GiftBox, listPoolObjectPosition[randomIndex], Quaternion.identity);
            giftBoxs.Add(giftBox);
        }
    }
    private void PlayerInit()
    {
        CharacterInfo characterInfo = SimplePool.Spawn<CharacterInfo>(PoolType.CharacterInfo);
        characterInfo.SetCharacter(player.GetComponent<Character>());
        characterInfo.gameObject.SetActive(false);
        player.CharacterInfo = characterInfo;
    }
    protected List<Vector3> GeneratePoolObjectPosition(Vector3 a_root, int numCount)
    {
        List<Vector3> listPoolObjectPosition = new List<Vector3>();
        int Row = Mathf.CeilToInt(Mathf.Sqrt(numCount));
        int Column = Row;
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                int index = Row * j + i;
                Vector3 objectPosition = new Vector3((j - (Row / 2)) + offset * j + a_root.x, 0.05f + a_root.y, ((Column / 2) - i) - offset * i + a_root.z);
                listPoolObjectPosition.Add(objectPosition);
            }
        }
        return listPoolObjectPosition;
    }
    public void OnStartGame()
    {
        GameManager.Instance.ChangeMode(GameMode.Normal);
        GameManager.Instance.ChangeState(GameState.InGame);
        /*for (int i = 0; i < bots.Count; i++)
        {
            bots[i].ChangeState(new PatrolState());
        }*/
    }

    public void OnFinishGame()
    {
        GameManager.Instance.ChangeState(GameState.EndGame);
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].ChangeState(null);
            bots[i].Indicator.Hide();
            bots[i].CharacterInfo.Hide();
            bots[i].MoveStop();
        }
        //Save Gold
        int coins = PlayerPrefs.GetInt(Constant.PLAYER_COIN, 0);
        coins += (int)player.InGameGold;
        PlayerPrefs.SetInt(Constant.PLAYER_COIN, coins);
        PlayerPrefs.Save();
        player.SetTransformPosition(levelPrefabs[levelIndex].GetStartPoint());
        player.CharacterInfo.Hide();
        player.OnInit();
        int rank = 1 + botInStack + botsInGame.Count;
        player.Rank = rank;
        int bestRank = PlayerPrefs.GetInt(Constant.BEST_RANK, 99);
        if (rank < bestRank)
        {
            PlayerPrefs.SetInt(Constant.BEST_RANK, rank);
            PlayerPrefs.Save();
        }
    }

    public void OnReset()
    {
        SimplePool.CollectAll();
        bots.Clear();
        botsInGame.Clear();
    }

    internal void OnRetry()
    {
        OnReset();
        LoadLevel(levelIndex);
        OnInit();
        GameManager.Instance.ChangeState(GameState.GameMenu);
        UIManager.Instance.OpenUI<GameMenu>();
        //UIManager.Instance.GetUI<GameMenu>().UpdateData();
    }

    internal void OnNextLevel()
    {
        levelIndex++;
        PlayerPrefs.SetInt(Constant.LEVEL, levelIndex);
        PlayerPrefs.Save();
        OnReset();
        LoadLevel(levelIndex);
        OnInit();
        UIManager.Instance.OpenUI<GameMenu>();
        //UIManager.Instance.GetUI<GameMenu>().UpdateData();
    }
    public int GetBotCount()
    { 
        return botsInGame.Count;
    }
    public List<GiftBox> GiftBoxs()
    {
        return giftBoxs;
    }
    public List<BotAI> GetBotAIs()
    {
        return botsInGame;
    }
    //================================================Survival Mode=============================================

    public void OnStartSurvivalGame()
    {
        GameManager.Instance.ChangeMode(GameMode.Survival);
        GameManager.Instance.ChangeState(GameState.InGame);
        OnInitSurvival();
        /*for (int i = 0; i < bots.Count; i++)
        {
            bots[i].ChangeState(new PatrolState());
        }*/
    }
    public void OnInitSurvival()
    {

        /*  botAmount = levelPrefabs[levelIndex].GetBotAmount();
          botInGame = levelPrefabs[levelIndex].GetBotInGame();
          botInStack = botAmount;*/
        //update navmesh data
        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(currentLevel.GetNavMeshData());

        //GenerateBotAI(botAmount, GeneratePoolObjectPosition(transform.position, botAmount));
        player.OnInitSurvival();
        player.transform.position = survivalPrefabs[survivalIndex].GetStartPoint().position;

    }
    public void LoadSurvival(int survival)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        if (survival < survivalPrefabs.Count)
        {
            currentLevel = Instantiate(survivalPrefabs[survival]);
            //currentLevel.OnInit();
        }
        else
        {
            //TODO: level vuot qua limit
        }
    }
    public void OnResetSurvival()
    {
        SimplePool.CollectAll();
        //bots.Clear();
        //botsInGame.Clear();
    }

    internal void OnRetrySurvival()
    {
        OnResetSurvival();
        LoadSurvival(survivalIndex);
        OnInitSurvival();
        GameManager.Instance.ChangeState(GameState.GameMenu);
        UIManager.Instance.OpenUI<GameMenu>();
    }

    internal void OnNextLevelSurvival()
    {
        survivalIndex++;
        PlayerPrefs.SetInt(Constant.SURVIVAL, survivalIndex);
        PlayerPrefs.Save();
        OnResetSurvival();
        LoadSurvival(survivalIndex);
        OnInitSurvival();
        UIManager.Instance.OpenUI<GameMenu>();
    }
}