using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : UICanvas
{
    [SerializeField] List<LevelUpData> levelUpDatas;
    [SerializeField] List<Image> buttonImages;
    [SerializeField] List<TextMeshProUGUI> textLevelUpInfos;
    private List<int> randoms = new List<int>();
    private void Start()
    {
        int random1 = Random.Range(0, 2);
        int random2 = Random.Range(2, 4);
        randoms.Add(random1);
        randoms.Add(random2);
        randoms.Add(4);
        for (int i = 0; i < buttonImages.Count; i++)
        {
            buttonImages[i].sprite = levelUpDatas[randoms[i]].LevelUpImage;
            textLevelUpInfos[i].text = levelUpDatas[randoms[i]].LevelUpInfo;
        }
        Time.timeScale = 0.0f;
    }
    public void LevelUpButton(int index)
    {
        Time.timeScale = 1.0f;
        UIManager.Instance.OpenUI<InGame>();
        Close();
        Player player = GameManager.Instance.Player();
        if (levelUpDatas[randoms[index]].isLevelUpType(LevelUpType.AttackSpeed))
        {
            //player Attack Speed +10
            player.InGameAttackSpeed += (player.InGameAttackSpeed * 0.1f);
        }
        if (levelUpDatas[randoms[index]].isLevelUpType(LevelUpType.AttackRange))
        {
            //player AttackRange +10
            player.InGameAttackRange += (player.InGameAttackRange * 0.1f);
        }
        if (levelUpDatas[randoms[index]].isLevelUpType(LevelUpType.MoveSpeed))
        {
            //player MoveSpeed +10
            player.InGameMoveSpeed += (player.InGameMoveSpeed * 0.1f);
        }
        if (levelUpDatas[randoms[index]].isLevelUpType(LevelUpType.Weapon))
        {
            //player Bullets+1
            player.Bullets++;
        }
        if (levelUpDatas[randoms[index]].isLevelUpType(LevelUpType.Hp))
        {
            //player Hp+50%
            float hp =player.Hp() + player.MaxHp() * 0.5f;
            player.SetHp(hp);
        }
    }

}
