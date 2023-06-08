using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
    [SerializeField] private TMPro.TextMeshProUGUI textWeaponType;
    [SerializeField] private TMPro.TextMeshProUGUI textWeaponStatus; 
    [SerializeField] private TMPro.TextMeshProUGUI textWeaponPriceBtnBuy;
    [SerializeField] private TMPro.TextMeshProUGUI textWeaponPriceBtnUnBuy;
    [SerializeField] private List<GameObject> listWeaponPreview;
   
    [SerializeField] private GameObject btn_Selelect;
    [SerializeField] private GameObject btn_Equipped;
    [SerializeField] private GameObject btn_Buy;
    [SerializeField] private GameObject btn_UnBuy;
    int itemSelelected=0;

    [Header("SkinShop: ")]
    [SerializeField] private SkinShop skinShop;
    [SerializeField] private GameObject popup_SkinShop;

    [SerializeField] private GameObject frame_TopHatSkinShop;
    [SerializeField] private GameObject frame_TopPantsSkinShop;
    [SerializeField] private GameObject frame_TopSheildSkinShop;
    [SerializeField] private GameObject frame_TopSetFullSkinShop;

    [SerializeField] private TMPro.TextMeshProUGUI textSkinBuffInfo;
    [SerializeField] private GameObject btn_SelelectSkinShop;
    [SerializeField] private GameObject btn_EquippedSkinShop;
    [SerializeField] private GameObject btn_BuySkinShop;
    [SerializeField] private GameObject btn_UnBuySkinShop;
    private AccessoriesData HatSkinShopData;

    [Header("InGame: ")]
    [SerializeField] private GameObject popup_Setting;
    [SerializeField] private TMPro.TextMeshProUGUI textAlive;
    [Header("EndGame: ")]
    [SerializeField] private GameObject popup_Countine;
    [SerializeField] private GameObject popup_TryAgain;
    [SerializeField] private TMPro.TextMeshProUGUI text_CountDown;

    GameManager _GameManager;
    Player player;
    private void Start()
    {
        _GameManager = GameManager.Instance;
        player = _GameManager.Player.GetComponent<Player>();
    }
    private void Update()
    {
        if (_GameManager.GameState == GameState.GameMenu)
        {
            if (PlayerPrefs.GetInt(Constant.PLAYER_COIN,-1)==-1)
            { 
                PlayerPrefs.SetInt(Constant.PLAYER_COIN,10000);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.GetInt(Constant.PLAYER_WEAPONS_HAVE, -1) == -1) 
            {
                PlayerPrefs.SetInt(Constant.PLAYER_WEAPONS_HAVE, 0);
                PlayerPrefs.SetInt(Constant.WEAPONS_USE, 0);
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
        else if (_GameManager.GameState == GameState.InGame)
        {
            int total = _GameManager.TotalBotAI + _GameManager.BotAIListEnable.Count;
            textAlive.text = ("Alive: " + total);
        }
        
    }
    public void setLoading()
    {
        _GameManager.OnInit();
    }
    public void setGameMenu()
    {
        GameMenu();
        _GameManager.GameState = GameState.GameMenu;
    }
    public void setInGame()
    {
        InGame();
        _GameManager.GameState = GameState.InGame;
    }
    public void setEndGame(bool isPlayerWon)
    {   
        EndGame(isPlayerWon);
        _GameManager.GameState = GameState.EndGame;
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
            popup_SkinShop.SetActive(false);
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

    //================GAME MENU WEAPONSHOP ===================
    public void Show_Popup_WeaponShop()
    {
        popup_GameMenuChild.SetActive(false);
        popup_WeaponShop.SetActive(true);
        itemSelelected = PlayerPrefs.GetInt(Constant.WEAPONS_USE, 0);
        upDateWeaponShopUI();
        listWeaponPreview[itemSelelected].SetActive(true);
        player.ShowWeaponIndex(itemSelelected);
        _GameManager.Player.gameObject.SetActive(false);
        BtnUpdate();
    }
    public void Hide_Popup_WeaponShop()
    {
        popup_GameMenuChild.SetActive(true);
        //gameManager.Player.GetComponent<Character>().ShowWeaponIndex(PlayerPrefs.GetInt(Constant.WEAPONS_USE, 0));
        _GameManager.Player.gameObject.SetActive(true);
        HideAllWeaponsInWeaponShopUI();
        popup_WeaponShop.SetActive(false);
        player.OnInit();
    }
    public void upDateWeaponShopUI()
    {
        HideAllWeaponsInWeaponShopUI();
        
    }
    public void HideAllWeaponsInWeaponShopUI()
    {
        for (int i=0; i< listWeaponPreview.Count;i++)
        {
            listWeaponPreview[i].SetActive(false);
        }
    }
    public void BtnUpdate()
    {
        player.ShowWeaponIndex(itemSelelected);
        player.SetWeaponSkinMat(player.ListWeaponsInHand[itemSelelected].gameObject.GetComponent<Renderer>(), _GameManager.WeaponData, itemSelelected);
        btn_Buy.SetActive(false);
        btn_Equipped.SetActive(false);
        btn_Selelect.SetActive(false);
        btn_UnBuy.SetActive(false);
        Weapon weapon = _GameManager.WeaponData.Weapon[itemSelelected];
        textWeaponType.text = "" + weapon.WeaponName;
        textWeaponPriceBtnBuy.text = "" + weapon.WeaponPrice;
        textWeaponPriceBtnUnBuy.text = "" + weapon.WeaponPrice;
        textWeaponBuffInfo.text = "+" + weapon.BuffData.BuffIndex+ " "+ weapon.BuffData.BuffType; //GameManager. WeaponData.Weapon[i].BuffType & BuffInfo

        if (itemSelelected == PlayerPrefs.GetInt(Constant.WEAPONS_USE, 0))
        {
            btn_Equipped.SetActive(true);
            textWeaponType.color = Color.white;
            textWeaponStatus.gameObject.SetActive(false);
        }
        else if (itemSelelected <= PlayerPrefs.GetInt(Constant.PLAYER_WEAPONS_HAVE, 0))
        {
            btn_Selelect.SetActive(true);
            textWeaponType.color = Color.white;
            textWeaponStatus.gameObject.SetActive(false);
        }
        else if (itemSelelected == PlayerPrefs.GetInt(Constant.PLAYER_WEAPONS_HAVE, 0) + 1)
        {
            btn_Buy.SetActive(true);
            textWeaponType.color = Color.black;
            textWeaponStatus.text = "(Lock)";
            textWeaponStatus.gameObject.SetActive(true);
        }
        else {
            btn_UnBuy.SetActive(true);
            textWeaponType.color = Color.black;
            if (itemSelelected > 0)
            {
                textWeaponStatus.text = "UnLock " + _GameManager.WeaponData.Weapon[itemSelelected - 1].WeaponName + " First";
            }
            
            textWeaponStatus.gameObject.SetActive(true);
        }
    }
    public void NextWeapon() 
    {
        if (itemSelelected < listWeaponPreview.Count-1)
        {
            itemSelelected++;
            Debug.Log(itemSelelected);
            HideAllWeaponsInWeaponShopUI();
            listWeaponPreview[itemSelelected].SetActive(true);
            BtnUpdate();

        }
   
    }
    public void PrevWeapon() 
    {
        Debug.Log(itemSelelected);
        if (itemSelelected > 0)
        {
            Debug.Log(itemSelelected);
            itemSelelected--;
            HideAllWeaponsInWeaponShopUI();
            listWeaponPreview[itemSelelected].SetActive(true);
            BtnUpdate();
        }
        
    }
    //btn Selelect and btn Equipped Onclick
    public void OnClickBtnSelelect_Equipped()
    {
        PlayerPrefs.SetInt(Constant.WEAPONS_USE, itemSelelected);
        PlayerPrefs.Save();
        player.WeaponIndex= itemSelelected;
        Hide_Popup_WeaponShop();
    }
    public void OnClickBtnBuy()
    {
        if (PlayerPrefs.GetInt(Constant.PLAYER_COIN) >= _GameManager.WeaponData.Weapon[itemSelelected].WeaponPrice)
        {
            int coin = PlayerPrefs.GetInt(Constant.PLAYER_COIN) - _GameManager.WeaponData.Weapon[itemSelelected].WeaponPrice;
            PlayerPrefs.SetInt(Constant.PLAYER_COIN, coin);
            PlayerPrefs.SetInt(Constant.PLAYER_WEAPONS_HAVE, itemSelelected);
            PlayerPrefs.SetInt(Constant.WEAPONS_USE, itemSelelected);
            PlayerPrefs.Save();
            player.WeaponIndex = itemSelelected;
            Hide_Popup_WeaponShop();
        }
        else
        {
            Debug.Log("You don't have enough coins to make this");
        }
    }
    //================GAME MENU SKIN SHOP ===================
    public void Show_Popup_SkinShop()
    {
        popup_GameMenuChild.SetActive(false);
        popup_SkinShop.SetActive(true);
        OnSlelectedHatSkinShop();
        _GameManager.GameState = GameState.SkinShop;
    }
    public void Hide_Popup_SkinShop()
    {
        popup_GameMenuChild.SetActive(true);
        
        popup_SkinShop.SetActive(false);
        _GameManager.GameState = GameState.GameMenu;
    }
    public void upDateSkinShopUI()
    {
        //HideAllWeaponsInWeaponShopUI();

    }
    public void OnSlelectedHatSkinShop()
    {
        frame_TopHatSkinShop.SetActive(false);
        frame_TopPantsSkinShop.SetActive(true);
        frame_TopSheildSkinShop.SetActive(true);
        frame_TopSetFullSkinShop.SetActive(true);
        InitSkinShop(_GameManager.HatsData);
        //player.UpdateUIAccessoris(_GameManager.HatsData);
        player.UpdateAccessoriesSkinShop();

    }
    public void OnSlelectedPaintSkinShop()
    {
        frame_TopHatSkinShop.SetActive(true);
        frame_TopPantsSkinShop.SetActive(false);
        frame_TopSheildSkinShop.SetActive(true);
        frame_TopSetFullSkinShop.SetActive(true);
        InitSkinShop(_GameManager.PantsData);
        //player.UpdateUIAccessoris(_GameManager.PantsData);
        player.UpdateAccessoriesSkinShop();
    }
    public void OnSlelectedSheildSkinShop()
    {
        frame_TopHatSkinShop.SetActive(true);
        frame_TopPantsSkinShop.SetActive(true);
        frame_TopSheildSkinShop.SetActive(false);
        frame_TopSetFullSkinShop.SetActive(true);
        InitSkinShop(_GameManager.ShieldData);
        //player.UpdateUIAccessoris(_GameManager.ShieldData);
        player.UpdateAccessoriesSkinShop();
    }
    public void OnSlelectedSetFullSkinShop()
    {
        frame_TopHatSkinShop.SetActive(true);
        frame_TopPantsSkinShop.SetActive(true);
        frame_TopSheildSkinShop.SetActive(true);
        frame_TopSetFullSkinShop.SetActive(false);
        InitSkinShop(_GameManager.SetfullData);
        //player.UpdateUIAccessoris(_GameManager.SetfullData);
        player.UpdateAccessoriesSkinShop();
    }
    private void InitSkinShop(AccessoriesData accessoriesData)
    {
        skinShop.ClearnItems();
        int index = player.GetAccessorisSelectedIndex(accessoriesData);
        accessoriesData.Accessories[index].Selected = true;
        skinShop.AccessoriesData = accessoriesData;
        skinShop.IsUpdate = true;
        StartCoroutine(setButtonCouroutime());
    }
    IEnumerator setButtonCouroutime()
    {
        yield return new WaitForSeconds(.5f);
        SetSkinShopButtonOnclick();
    }

    private void SetSkinShopButtonOnclick() 
    {
        for (int i = 0; i < skinShop.Items.Count; i++)
        {
            skinShop.Items[i].ImageItem.gameObject.GetComponent<Button>().AddEventListener(i, OnShopItemBtnClicked);
        }
       
    }
    private void OnShopItemBtnClicked(int itemIndex)
    {
        skinShop.ItemsOnClicked(itemIndex);
        if (skinShop.Items[0].SkinType == SkinType.Hat)
        {
            player.SetAllAccessoriesUnSelected(_GameManager.SetfullData);
            player.SetAllAccessoriesUnSelected(_GameManager.HatsData);
            _GameManager.HatsData.Accessories[itemIndex].Selected = true;
           
             //player.UpdateUIAccessoris(_GameManager.HatsData);
            player.UpdateAccessoriesSkinShop();
        }
        else if (skinShop.Items[0].SkinType == SkinType.Pant)
        {
            player.SetAllAccessoriesUnSelected(_GameManager.SetfullData);
            player.SetAllAccessoriesUnSelected(_GameManager.PantsData);
            _GameManager.PantsData.Accessories[itemIndex].Selected = true;
            player.SetPantsSkin(_GameManager.PantsData);
           
            //player.UpdateUIAccessoris(_GameManager.PantsData);
            player.UpdateAccessoriesSkinShop();
        }
        else if (skinShop.Items[0].SkinType == SkinType.Sheild)
        {
            player.SetAllAccessoriesUnSelected(_GameManager.SetfullData);
            player.SetAllAccessoriesUnSelected(_GameManager.ShieldData);
            _GameManager.ShieldData.Accessories[itemIndex].Selected = true;
           
            //player.UpdateUIAccessoris(_GameManager.ShieldData);
            player.UpdateAccessoriesSkinShop();
        }
        else if (skinShop.Items[0].SkinType == SkinType.SetFull)
        {
            player.SetAllAccessoriesUnSelected(_GameManager.HatsData);
            player.SetAllAccessoriesUnSelected(_GameManager.PantsData);
            player.SetAllAccessoriesUnSelected(_GameManager.ShieldData);
            player.SetAllAccessoriesUnSelected(_GameManager.SetfullData);
            _GameManager.SetfullData.Accessories[itemIndex].Selected = true;
      
            //player.UpdateUIAccessoris(_GameManager.SetfullData);
            player.UpdateAccessoriesSkinShop();
        }
        
    }
}
