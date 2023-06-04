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
        int total = gameManager.TotalBotAI + gameManager.BotAIListEnable.Count;
        textAlive.text = ("Alive: "+ total);
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
    public void setEndGame()
    {   
        EndGame();
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
    public void EndGame()
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
            Show_Popup_Tryagain();
        }
    }

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
    public void Show_Popup_Tryagain()
    {
        popup_Countine.SetActive(false);
        popup_TryAgain.SetActive(true);
        StartCoroutine(Waiter(text_CountDown));

    }
    public void Hide_Popup_Tryagain()
    {
        if (popup_TryAgain.activeSelf)
        {
            popup_TryAgain.SetActive(false);
        }
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
}
