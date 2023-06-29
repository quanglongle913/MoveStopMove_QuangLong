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
    Player player;
    private void Start()
    {
        player = GameManager.Instance.Player();
    }
    void Update()
    {
        textKillCount.text = "" + player.KilledCount();
        float _value = (float)player.InGamneExp / (float)(player.GetLevel() * 50);
        sliderExpBar.value = _value;
        textPlayerLevel.text = "" + player.GetLevel();
        textPlayerExp.text = "" + player.InGamneExp + "/" + player.GetLevel() * 50;
        ShowHealthBarPlayer(player);
    }
    private void ShowHealthBarPlayer(Player player)
    {
        sliderHealthBar.value = (float)player.Hp() / (float)player.MaxHp();
    }
    public void SettingButton()
    {
        UIManager.Instance.OpenUI<SettingInGame>();
        Close();
    }
}
