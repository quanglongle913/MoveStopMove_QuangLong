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
    [SerializeField] int node = 45;
    [SerializeField] float offset=18;
    [SerializeField] private float size_x=1;
    [SerializeField] private float size_z=1;
    [SerializeField] private Player player;

    //-----------------
    private Level currentLevel;
    private List<GameObject> objCharacters = new List<GameObject>();
    private List<BotAI> bots = new List<BotAI>();
    private List<BotAI> botsInGame = new List<BotAI>();
    private List<GiftBox> giftBoxs = new List<GiftBox>();
    private int levelIndex;
    private int botAmount;
    private int botInGame;
    private int botInStack;

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
        objCharacters.Add(GameManager.Instance.Player().gameObject);
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
                            

                            bots[i].SetExp(player.InGamneExp);
                            bots[i].SetLevel(player.GetLevel());
                            bots[i].ChangeState(new IdleState());
                            bots[i].gameObject.SetActive(true);
                            botsInGame.Add(bots[i]);
                            objCharacters.Add(bots[i].gameObject);
                            botInStack--;
                        }
                    }
                }
                else if (botsInGame.Count == 0)
                {
                    //Player Win
                    UIManager.Instance.OpenUI<Win>();
                    UIManager.Instance.CloseUI<InGame>();
                }
                if (giftBoxs.Count < 10)
                {
                    GenerateGiftBox(1, GeneratePoolObjectPosition(transform.position, node));
                }
            }
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

        GenerateBotAI(botAmount, GeneratePoolObjectPosition(transform.position, node));
        player.transform.position = levelPrefabs[levelIndex].GetStartPoint().position;
        PlayerInit();

    }
    //public Level GetLevel()
    //{
    //    return currentLevel;
    //}
    public void OnDestroy()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
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
                bot.DetectionCharacter(objCharacters);
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
            bool check=false;
            int randomIndex = Random.Range(0, listPoolObjectPosition.Count);
            for (int j = 0; j < 100; j++)
            {
                if (IsDesAllCharacter(listPoolObjectPosition[randomIndex]))
                {
                    randomIndex = Random.Range(0, listPoolObjectPosition.Count);
                }
                else
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                AddBot(GameManager.Instance.GetBotAIInfo(i), listPoolObjectPosition[randomIndex]);
            }

        }
    }
    private void AddBot(BotAIInfo botAIInfo, Vector3 vector3)
    {
        BotAI bot = SimplePool.Spawn<BotAI>(PoolType.Bot, vector3, Quaternion.identity);
        bot.OnInit();
        bot.UpdateInfo(botAIInfo, GameManager.Instance.GetAccessoriesDatas());

        Indicator indicator = SimplePool.Spawn<Indicator>(PoolType.Indicator);
        indicator.SetCharacter(Constant.Cache.GetCharacter(bot.gameObject));
        indicator.gameObject.SetActive(false);
        bot.Indicator = indicator;

        CharacterInfo characterInfo = SimplePool.Spawn<CharacterInfo>(PoolType.CharacterInfo);
        characterInfo.SetCharacter(Constant.Cache.GetCharacter(bot.gameObject));
        characterInfo.gameObject.SetActive(false);
        bot.CharacterInfo = characterInfo;

        bot.gameObject.SetActive(false);
        bots.Add(bot);
    }
    private bool IsDesAllCharacter(Vector3 vector3)
    { 
        bool isDesAllCharacter = false;
        
        for (int i = 0; i < objCharacters.Count; i++)
        {
            if (Constant.IsDes(objCharacters[i].transform.position, vector3, GameManager.Instance.Player().InGameAttackRange)&& objCharacters[i].activeSelf)
            {
                isDesAllCharacter = true;
                break;
            }
        }
        return isDesAllCharacter;
    }
    private bool IsDesAllGiftBox(Vector3 vector3)
    {
        bool isDesAll = false;
        for (int i = 0; i < giftBoxs.Count; i++)
        {
            if (Constant.IsDes(giftBoxs[i].transform.position, vector3, 3f))
            {
                isDesAll = true;
                break;
            }
        }
        return isDesAll;
    }
    private void GenerateGiftBox(int index, List<Vector3> listPoolObjectPosition)
    {
        for (int i = 0; i < index; i++)
        {
            bool check = false;
            int randomIndex = Random.Range(0, listPoolObjectPosition.Count);
            for (int j = 0; j < 50 ; j++)
            {
                if (IsDesAllGiftBox(listPoolObjectPosition[randomIndex]))
                {
                    randomIndex = Random.Range(0, listPoolObjectPosition.Count);
                }
                else
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                GiftBox giftBox = SimplePool.Spawn<GiftBox>(PoolType.GiftBox, listPoolObjectPosition[randomIndex], Quaternion.identity);
                giftBoxs.Add(giftBox);
            }
        }
    }
    private void PlayerInit()
    {
        CharacterInfo characterInfo = SimplePool.Spawn<CharacterInfo>(PoolType.CharacterInfo);
        characterInfo.SetCharacter(Constant.Cache.GetCharacter(player.gameObject));
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
        OnRetry();
        GameManager.Instance.ChangeMode(GameMode.Normal);
        GameManager.Instance.ChangeState(GameState.InGame);
    }

    public void OnFinishGame()
    {
        GameManager.Instance.ChangeState(GameState.EndGame);
        ResetListBotAI(bots);
        ResetListBotAI(botsInGame);
        //Save Gold
        int coins = PlayerPrefs.GetInt(Constant.PLAYER_COIN, 0);
        coins += (int)player.InGameGold;
        PlayerPrefs.SetInt(Constant.PLAYER_COIN, coins);
        PlayerPrefs.Save();
        
        int rank = 1 + botInStack + botsInGame.Count;
        player.SetRank(rank);
        int bestRank = PlayerPrefs.GetInt(Constant.BEST_RANK, 99);
        if (rank < bestRank)
        {
            PlayerPrefs.SetInt(Constant.BEST_RANK, rank);
            PlayerPrefs.Save();
        }
        OnReset();
    }

    public void OnReset()
    {
        SimplePool.CollectAll();
        bots.Clear();
        botsInGame.Clear();

        player.SetTransformPosition(levelPrefabs[levelIndex].GetStartPoint());
        player.CharacterInfo.Hide();
        player.OnInit();
    }

    internal void OnRetry()
    {
        OnReset();
        //OnResetSurvival();
        LoadLevel(levelIndex);
        OnInit();
        GameManager.Instance.ChangeState(GameState.GameMenu);
        UIManager.Instance.OpenUI<GameMenu>();
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
    }
    public void ResetListBotAI(List<BotAI> lists)
    {
        for (int i = 0; i < lists.Count; i++)
        {
            lists[i].ChangeState(null);
            lists[i].Indicator.Hide();
            lists[i].CharacterInfo.Hide();
            lists[i].MoveStop();
        }
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
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        int Row = Mathf.CeilToInt(Mathf.Sqrt(node));
        int Column = Row;
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                int index = Row * j + i;
                Vector3 objectPosition = new Vector3((j - (Row / 2)) + offset * j + transform.position.x, 0.05f + transform.position.y, ((Column / 2) - i) - offset * i + transform.position.z);
                drawRectangle(objectPosition);
            }
        }

    }
    private void drawRectangle(Vector3 point)
    {
        //Top Left
        Vector3 topL = new Vector3(point.x - size_x, point.y, point.z + size_z);
        //Top Right
        Vector3 topR = new Vector3(point.x + size_x, point.y, point.z + size_z);
        //Bot Right
        Vector3 botR = new Vector3(point.x + size_x, point.y, point.z - size_z);
        //Bot Left
        Vector3 botL = new Vector3(point.x - size_x, point.y, point.z - size_z);

        Gizmos.DrawLine(topL, topR);
        Gizmos.DrawLine(topR, botR);
        Gizmos.DrawLine(botR, botL);
        Gizmos.DrawLine(botL, topL);
    }
}