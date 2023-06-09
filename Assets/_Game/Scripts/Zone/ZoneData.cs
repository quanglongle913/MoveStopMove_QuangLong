using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZoneData", menuName = "ScriptableObjects/ZoneData", order = 1)]
public class ZoneData : ScriptableObject
{
    [SerializeField] Zone[] zones;
    [SerializeField] private ZoneType playerZoneType;
    [SerializeField] private int playerZoneExp;

    public Zone[] Zones { get => zones; set => zones = value; }
    public int PlayerZoneExp { get => playerZoneExp; set => playerZoneExp = value; }
    public ZoneType PlayerZoneType { get => playerZoneType; set => playerZoneType = value; }
}
