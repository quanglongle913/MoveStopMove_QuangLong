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
    [SerializeField] int attackSpeed;
    [SerializeField] int range;
    [SerializeField] int moveSpeed;

    public int AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Range { get => range; set => range = value; }
    public int MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public string WeaponName { get => weaponName; set => weaponName = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public Material Mat { get => mat; set => mat = value; }
    //public Texture Texture { get => texture; set => texture = value; }
    //public GameObject WeaponPrefabs { get => weaponPrefabs; set => weaponPrefabs = value; }
}
