using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "ScriptableObjects/BuffData", order = 1)]
public class BuffData : ScriptableObject
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private int buffIndex;
    public BuffType BuffType { get => buffType; set => buffType = value; }
    public int BuffIndex { get => buffIndex; set => buffIndex = value; }
}
