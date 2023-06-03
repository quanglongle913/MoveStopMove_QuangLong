using System.Collections;
using System.Collections.Generic;
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

    GameManager gameManager;
    
    private void Start()
    {
       gameManager = GameManager.Instance;
    }
    public void setLoading()
    {
        Loading();
        gameManager.GameState = GameState.Loading;
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

}
