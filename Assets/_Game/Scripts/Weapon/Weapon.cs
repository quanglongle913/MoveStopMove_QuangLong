using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    //[SerializeField] GameObject weaponPrefabs;
    [SerializeField] string weaponName;
    [SerializeField] WeaponType weaponType;
    //[SerializeField] Texture texture;
    [SerializeField] private Material mat;
    [SerializeField] private BuffData buffData;


    public string WeaponName { get => weaponName; set => weaponName = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public Material Mat { get => mat; set => mat = value; }
    public BuffData BuffData { get => buffData; set => buffData = value; }
}
