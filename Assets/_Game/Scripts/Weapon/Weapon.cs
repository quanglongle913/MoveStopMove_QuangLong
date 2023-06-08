using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    //[SerializeField] GameObject weaponPrefabs;
    [SerializeField] string weaponName;
    [SerializeField] WeaponType weaponType;
    [SerializeField] int weaponPrice;
    [SerializeField] private bool equipped = false;
    [SerializeField] private bool buyed = false;
    [SerializeField] private Material mat;
    [SerializeField] private BuffData buffData;


    public string WeaponName { get => weaponName; set => weaponName = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public Material Mat { get => mat; set => mat = value; }
    public BuffData BuffData { get => buffData; set => buffData = value; }
    public int WeaponPrice { get => weaponPrice; set => weaponPrice = value; }
    public bool Equipped { get => equipped; set => equipped = value; }
    public bool Buyed { get => buyed; set => buyed = value; }
}
