using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField] private NavMeshData navMeshData;
    [SerializeField] private Transform startPoint;
    [Header("BotAmount > BotInGame")]
    [SerializeField] private int botAmount;
    [SerializeField] private int botInGame;
    public int GetBotAmount()
    { 
        return botAmount;
    }
    public int GetBotInGame()
    {
        return botInGame;
    }
    public Transform GetStartPoint()
    {
        return startPoint;
    }
    public NavMeshData GetNavMeshData()
    {
        return navMeshData;
    }
}
