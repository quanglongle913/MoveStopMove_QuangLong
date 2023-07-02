using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win : UICanvas
{
    [SerializeField] private TMPro.TextMeshProUGUI textCoin;
    [SerializeField] private TMPro.TextMeshProUGUI textStar;
    [SerializeField] private TMPro.TextMeshProUGUI text_ZoneType;
    [SerializeField] private TMPro.TextMeshProUGUI text_ZoneTypeNext;
    [SerializeField] private RawImage image_ZoneType;
    [SerializeField] private RawImage image_ZoneTypeNext;
    // Start is called before the first frame update
    void Start()
    {
        Player player = GameManager.Instance.Player();
        textCoin.text = "" + player.InGameGold;
        textStar.text = ""+player.KilledCount();

        GameManager.Instance.LevelManager().OnFinishGame();

        int zoneType = PlayerPrefs.GetInt(Constant.PLAYER_ZONE_TYPE, 0);
        Zone zone = GameManager.Instance.ZoneData().Zones[zoneType];

        text_ZoneType.text = zone.ZoneName;
        image_ZoneType.texture = zone.Texture;
        zoneType++;
        text_ZoneTypeNext.text = zone.ZoneName;
        image_ZoneTypeNext.texture = zone.Texture;

        GameManager.Instance.SoundManager().PlayEndWinSoundEffect();
    }

    public void CountineButton()
    {
        GameManager.Instance.LevelManager().OnRetry();
        int playerZoneExp = PlayerPrefs.GetInt(Constant.PLAYER_ZONE_EXP, 0);
        playerZoneExp += 10;
        PlayerPrefs.SetInt(Constant.PLAYER_ZONE_EXP, playerZoneExp);
        PlayerPrefs.Save();
        UIManager.Instance.OpenUI<Loading>();
        Close();
    }
}
