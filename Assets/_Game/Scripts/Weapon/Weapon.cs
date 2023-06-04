using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] string name;
    [SerializeField] int attackSpeed;
    [SerializeField] int range;
    [SerializeField] int moveSpeed;

    public GameObject WeaponPrefab { get => weaponPrefab; set => weaponPrefab = value; }
    public int AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Range { get => range; set => range = value; }
    public int MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
}