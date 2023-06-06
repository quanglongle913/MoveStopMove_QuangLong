using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Layout: ")]
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject inGame;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject endGame;
    [Header("GameMenu: ")]
    [SerializeField] private GameObject popup_GameMenuChild;
    [SerializeField] private TMPro.TextMeshProUGUI textCoin;
    [SerializeField] private TMPro.TextMeshProUGUI textName;
    [SerializeField] private TMPro.TextMeshProUGUI textZone;
    [SerializeField] private TMPro.TextMeshProUGUI textZoneBTN;
    [SerializeField] private TMPro.TextMeshProUGUI textBestBTN;
    [SerializeField] private TMPro.TextMeshProUGUI textZombieDay;
    [SerializeField] private TMPro.TextMeshProUGUI textZoneExp;
    [Header("WeaponShop: ")]
    [SerializeField] private GameObject popup_WeaponShop;
    [SerializeField] private TMPro.TextMeshProUGUI textWeaponBuffInfo; //GameManager. WeaponData.Weapon[i].BuffType & BuffInfo
    [SerializeField] private List<GameObject> listWeaponPreview;

    [SerializeField] private GameObject btn_Selelect;
    [SerializeField] private GameObject btn_Equipped;
    [SerializeField] private GameObject btn_Buy;
    [SerializeField] private GameObject btn_UnBuy;

    [Header("SkinShop: ")]
    [SerializeField] private GameObject popup_SkinShop;
    [Header("InGame: ")]
    [SerializeField] private GameObject popup_Setting;
    [SerializeField] private TMPro.TextMeshProUGUI textAlive;
    [Header("EndGame: ")]
    [SerializeField] private GameObject popup_Countine;
    [SerializeField] private GameObject popup_TryAgain;
    [SerializeField] private TMPro.TextMeshProUGUI text_CountDown;

    GameManager gameManager;

    private void Start()
    {
       gameManager = GameManager.Instance;
    }
    private void Update()
    {
        if (gameManager.GameState == GameState.GameMenu)
        {
            //Debug.Log(PlayerPrefs.GetInt(Constant.PLAYER_COIN, -1));
            if (PlayerPrefs.GetInt(Constant.PLAYER_COIN,-1)==-1)
            { 
                PlayerPrefs.SetInt(Constant.PLAYER_COIN,10000);
                PlayerPrefs.Save();
            }
            var score = PlayerPrefs.GetInt(Constant.PLAYER_COIN, 0);
            string.Format("Score: {0:#,#}", score);
            score.ToString("#,#");
            textCoin.text = (score.ToString("#,#"));
            textName.text = PlayerPrefs.GetString(Constant.PLAYER_NAME, "You");
            textZone.text = ("" + PlayerPrefs.GetInt(Constant.PLAYER_ZONE, 1));
            textZoneBTN.text = ("ZONE: " + PlayerPrefs.GetInt(Constant.PLAYER_ZONE, 1));
            textBestBTN.text = ("BEST: #" + PlayerPrefs.GetInt(Constant.PLAYER_BEST, 99));
            textZombieDay.text = ("" + PlayerPrefs.GetInt(Constant.PLAYER_ZOMBIEDAY, 0));
            int maxExpZone = PlayerPrefs.GetInt(Constant.PLAYER_ZONE, 1) * 200;
            textZoneExp.text = (PlayerPrefs.GetInt(Constant.PLAYER_EXP, 0)+"/"+ maxExpZone);

        }
        else if (gameManager.GameState == GameState.InGame)
        {
            int total = gameManager.TotalBotAI + gameManager.BotAIListEnable.Count;
            textAlive.text = ("Alive: " + total);
        }
        
    }
    public void setLoading()
    {
        gameManager.OnInit();
    }
    public void setGameMenu()
    {
        GameMenu();
        gameManager.GameState = GameState.GameMenu;
    }
    public void setInGame()
    {
        InGame();
        gameManager.GameState = GameState.InGame;
    }
    public void setEndGame(bool isPlayerWon)
    {   
        EndGame(isPlayerWon);
        gameManager.GameState = GameState.EndGame;
    }
    public void Loading()
    {
        if (!loading.activeSelf)
        {
            loading.SetActive(true);
        }
        if (gameMenu.activeSelf)
        {
            gameMenu.SetActive(false);
        }
        if (inGame.activeSelf)
        {
            inGame.SetActive(false);
        }
        if (endGame.activeSelf)
        {
            endGame.SetActive(false);
        }
    }
    public void GameMenu()
    {
        if (loading.activeSelf)
        {
            loading.SetActive(false);
        }
        if (!gameMenu.activeSelf)
        {
            gameMenu.SetActive(true);
            popup_GameMenuChild.SetActive(true);
            popup_WeaponShop.SetActive(false);
        }
        if (inGame.activeSelf)
        {
            inGame.SetActive(false);
        }
        if (endGame.activeSelf)
        {
            endGame.SetActive(false);
        }
        Hide_Popup_Setting();
    }
    public void InGame()
    {
        if (loading.activeSelf)
        {
            loading.SetActive(false);
        }
        if (gameMenu.activeSelf)
        {
            gameMenu.SetActive(false);
        }
        if (!inGame.gameObject.activeSelf)
        {
            inGame.SetActive(true);
        }
        if (endGame.activeSelf)
        {
            endGame.SetActive(false);
        }
    }
    public void EndGame(bool isPlayerWon)
    {
        if (loading.activeSelf)
        {
            loading.SetActive(false);
        }
        if (gameMenu.activeSelf)
        {
            gameMenu.SetActive(false);
        }
        if (inGame.activeSelf)
        {
            inGame.SetActive(false);
        }
        if (!endGame.activeSelf)
        {
            endGame.SetActive(true);
            if (isPlayerWon)
            {
                Show_Popup_PlayerWon();
            }
            else
            {
                Show_Popup_Tryagain();
            }
                
        }
    }
    //================IN GAME===================
    public void Show_Popup_Setting()
    {
        popup_Setting.SetActive(true);
    }
    public void Hide_Popup_Setting()
    {
        if (popup_Setting.activeSelf)
        {
            popup_Setting.SetActive(false);
        }
    }
    public void Show_Popup_Countine()
    {
        popup_TryAgain.SetActive(false);
        popup_Countine.SetActive(true);    
    }
    public void Hide_Popup_Countine()
    {
        popup_Countine.SetActive(false);
        setLoading();
    }
    public void Hide_Popup_Tryagain()
    {
        if (popup_TryAgain.activeSelf)
        {
            popup_TryAgain.SetActive(false);
        }
    }
    public void Show_Popup_PlayerWon()
    {
        //TODO
       
    }
    public void Hide_Popup_PlayerWon()
    {
        //TODO
    
    }
    public void Show_Popup_Tryagain()
    {
        popup_Countine.SetActive(false);
        popup_TryAgain.SetActive(true);
        StartCoroutine(Waiter(text_CountDown));
    }

    public void EndGame_RevieNow()
    {
        //UNDONE Test TODO Loading and auto go play
        setLoading();
    }
    IEnumerator Waiter(TMPro.TextMeshProUGUI text_CountDown)
    {
        text_CountDown.text = "5";
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "4";
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "3";
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "2";
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "1";
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "0";
        Show_Popup_Countine();
    }

    //================GAME MENU===================
    public void Show_Popup_WeaponShop()
    {
        //TODO
        popup_GameMenuChild.SetActive(false);
        popup_WeaponShop.SetActive(true);
    }
    public void Hide_Popup_WeaponShop()
    {
        //TODO
        popup_GameMenuChild.SetActive(true);
        popup_WeaponShop.SetActive(false);
    }
}
