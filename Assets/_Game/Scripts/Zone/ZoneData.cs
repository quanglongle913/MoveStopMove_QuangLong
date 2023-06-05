using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZoneData", menuName = "ScriptableObjects/ZoneData", order = 1)]
public class ZoneData : ScriptableObject
{
    [SerializeField] Zone[] zones;

    public Zone[] Zones { get => zones; set => zones = value; }
}
