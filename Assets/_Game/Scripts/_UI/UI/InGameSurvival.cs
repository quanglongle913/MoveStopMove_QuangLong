using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSurvival : UICanvas
{
    [SerializeField] private TMPro.TextMeshProUGUI textKillCount;
    [SerializeField] private TMPro.TextMeshProUGUI textPlayerLevel;
    [SerializeField] private TMPro.TextMeshProUGUI textPlayerExp;
    [SerializeField] private Slider sliderExpBar;
    [SerializeField] private Slider sliderHealthBar;
    public void SettingButton()
    {
        UIManager.Instance.OpenUI<SettingInGame>();
        Close();
    }
    void Update()
    {
        textKillCount.text = "" + GameManager.Instance.Player().KilledCount;
        float _value = (float)GameManager.Instance.Player().InGamneExp / (float)(GameManager.Instance.Player().GetLevel() * 50);
        sliderExpBar.value = _value;
        textPlayerLevel.text = "" + GameManager.Instance.Player().GetLevel();
        textPlayerExp.text = "" + GameManager.Instance.Player().InGamneExp + "/" + GameManager.Instance.Player().GetLevel() * 50;
        ShowHealthBarPlayer(GameManager.Instance.Player());
    }
    private void ShowHealthBarPlayer(Player player)
    {
        sliderHealthBar.value = (float)player.Hp() / (float)player.MaxHp();
    }
}
