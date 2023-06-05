using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Zone", menuName = "ScriptableObjects/Zone", order = 1)]
public class Zone : ScriptableObject
{
    [SerializeField] private string zoneName;
    [SerializeField] private Texture texture;
    [SerializeField] private int zoneExp;

    public string ZoneName { get => zoneName; set => zoneName = value; }
    public Texture Texture { get => texture; set => texture = value; }
    public int ZoneExp { get => zoneExp; set => zoneExp = value; }
}
