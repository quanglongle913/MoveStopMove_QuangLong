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
        text_GoldEarned.text = "" + PlayerPrefs.GetInt(Constant.PLAYER_COIN, 0);
        int zoneType = PlayerPrefs.GetInt(Constant.PLAYER_ZONE_TYPE, 0);
        text_ZoneType.text = GameManager.Instance.ZoneData().Zones[zoneType].ZoneName;
        image_ZoneType.texture = GameManager.Instance.ZoneData().Zones[zoneType].Texture;
        zoneType++;
        text_ZoneTypeNext.text = GameManager.Instance.ZoneData().Zones[zoneType].ZoneName;
        image_ZoneTypeNext.texture = GameManager.Instance.ZoneData().Zones[zoneType].Texture;

        text_PlayerRank.text = "#" + GameManager.Instance.Player().Rank;
        GameManager.Instance.SoundManager().PlayLoseSoundEffect();
    }

    public void CountineButton()
    {
        GameManager.Instance.LevelManager().OnRetry();
        //UIManager.Instance.OpenUI<Loading>();
        Close();
    }
}
