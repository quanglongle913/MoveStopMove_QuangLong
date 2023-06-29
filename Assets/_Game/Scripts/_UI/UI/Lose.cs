using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lose : UICanvas
{

    [SerializeField] private TMPro.TextMeshProUGUI text_ZoneType;
    [SerializeField] private TMPro.TextMeshProUGUI text_ZoneTypeNext;
    [SerializeField] private RawImage image_ZoneType;
    [SerializeField] private RawImage image_ZoneTypeNext;

    [SerializeField] private TMPro.TextMeshProUGUI text_PlayerRank;
    [SerializeField] private TMPro.TextMeshProUGUI text_KillerName;
    [SerializeField] private TMPro.TextMeshProUGUI text_GoldEarned;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.LevelManager().OnFinishGame();

        int zoneType = PlayerPrefs.GetInt(Constant.PLAYER_ZONE_TYPE, 0);
        Zone zone = GameManager.Instance.ZoneData().Zones[zoneType];

        text_GoldEarned.text = "" + PlayerPrefs.GetInt(Constant.PLAYER_COIN, 0);
        
        text_ZoneType.text = zone.ZoneName;
        image_ZoneType.texture = zone.Texture;
        zoneType++;
        text_ZoneTypeNext.text = zone.ZoneName;
        image_ZoneTypeNext.texture = zone.Texture;

        Player player = GameManager.Instance.Player();
        text_KillerName.text = player.KilledByName();
        text_KillerName.color = player.GetColor(player.KillerColorType());
        text_PlayerRank.text = "#" + player.Rank();
        GameManager.Instance.SoundManager().PlayLoseSoundEffect();
    }

    public void CountineButton()
    {
        GameManager.Instance.LevelManager().OnRetry();
        Close();
    }
}
