using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : UICanvas
{
    [Header("UI Top: ")]
    [SerializeField] private TMPro.TextMeshProUGUI textCoin;
    [SerializeField] private TMPro.TextMeshProUGUI textName;
    [SerializeField] private TMPro.TextMeshProUGUI textZone;
    [SerializeField] private Slider sliderZoneExp;
    [SerializeField] private TMPro.TextMeshProUGUI textZoneExp;
    [Header("UI Bottom: ")]
    [SerializeField] private TMPro.TextMeshProUGUI textZoneBTN;
    [SerializeField] private TMPro.TextMeshProUGUI textBestBTN;
    [SerializeField] private TMPro.TextMeshProUGUI textZombieDay;

    private void Start()
    {
        GameManager.Instance.ChangeState(GameState.GameMenu);
        UpdateData();
    }
    public void UpdateData()
    {
        if (PlayerPrefs.GetInt(Constant.PLAYER_COIN, -1) == -1)
        {
            PlayerPrefs.SetInt(Constant.PLAYER_COIN, 10000);
            PlayerPrefs.Save();
        }
        int coins = PlayerPrefs.GetInt(Constant.PLAYER_COIN, 0);
        string.Format(Constant.STRING_SCORE+": {0:#,#}", coins);
        coins.ToString("#,#");
        textCoin.text = (coins.ToString("#,#"));
        textName.text = PlayerPrefs.GetString(Constant.PLAYER_NAME, "You");
        
        textZombieDay.text = ("" + PlayerPrefs.GetInt(Constant.PLAYER_ZOMBIEDAY, 0));

        int ZoneNumber = PlayerPrefs.GetInt(Constant.PLAYER_ZONE_TYPE, 0);
        int maxExpZone = GameManager.Instance.ZoneData().Zones[(int)GameManager.Instance.ZoneData().PlayerZoneType].ZoneExp;
        int playerZoneExp = PlayerPrefs.GetInt(Constant.PLAYER_ZONE_EXP, 0);
        if (playerZoneExp >= maxExpZone)
        {
            playerZoneExp = 0;
            
            if (ZoneNumber < GameManager.Instance.ZoneData().Zones.Length)
            {
                ZoneNumber++;
            }
            PlayerPrefs.SetInt(Constant.PLAYER_ZONE_TYPE, ZoneNumber);
            PlayerPrefs.SetInt(Constant.PLAYER_ZONE_EXP, playerZoneExp);
            PlayerPrefs.Save();
        }
        
        textZone.text = "" + (ZoneNumber +1);
        textZoneExp.text = playerZoneExp + "/" + maxExpZone;
        sliderZoneExp.value = (float)playerZoneExp / (float)maxExpZone;

        textZoneBTN.text = (Constant.STRING_ZONE+": " + (ZoneNumber + 1));
        textBestBTN.text = (Constant.STRING_BEST+": #" + PlayerPrefs.GetInt(Constant.BEST_RANK, 99));
    }
    public void PlayButton()
    {
        GameManager.Instance.SurvivalManager().OnDestroy();
        GameManager.Instance.LevelManager().OnStartGame();
        UIManager.Instance.OpenUI<InGame>();
        Close();
    }
    public void SurvivalButton()
    {
        GameManager.Instance.LevelManager().OnDestroy();
        GameManager.Instance.SurvivalManager().OnStartGame();
        UIManager.Instance.OpenUI<InGameSurvival>();
        Close();
    }
    public void WeaponsButton()
    {
        UIManager.Instance.OpenUI<WeaponShopUI>();
        GameManager.Instance.HidePlayer();
        Close();
       
    }
    public void SkinShopButton()
    {
        UIManager.Instance.OpenUI<SkinShopUI>();
        Close();
    }
    public void SettingButton()
    {
        UIManager.Instance.OpenUI<Setting>();
        //Close();
    }
}
