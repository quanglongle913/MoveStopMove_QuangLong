using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LevelUpType { AttackSpeed, AttackRange, MoveSpeed, Weapon, Hp}
[CreateAssetMenu(fileName = "LevelUpData", menuName = "ScriptableObjects/LevelUpData", order = 1)]
public class LevelUpData : ScriptableObject
{
    [SerializeField] Sprite levelUpImage;
    [SerializeField] LevelUpType levelUpType;
    [SerializeField] string levelUpInfo;
    public Sprite LevelUpImage { get => levelUpImage; set => levelUpImage = value; }
    public LevelUpType LevelUpType { get => levelUpType; set => levelUpType = value; }
    public string LevelUpInfo { get => levelUpInfo; set => levelUpInfo = value; }
    public bool isLevelUpType(LevelUpType levelUpType)
    {
        return (this.levelUpType == levelUpType);
    }
}