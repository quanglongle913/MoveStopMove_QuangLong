using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponData", menuName = "ScriptableObjects/ColorData", order = 1)]
public class WeaponData : ScriptableObject
{
    [SerializeField] GameObject weaponPrefab;
}
