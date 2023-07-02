using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Survival : MonoBehaviour
{
    [SerializeField] private NavMeshData navMeshData;
    [SerializeField] private Transform startPoint;
    [SerializeField] private int botAmount;
    [SerializeField] private List<Transform> startPoints;
    public int GetBotAmount()
    {
        return botAmount;
    }
    public Transform GetStartPoint()
    {
        return startPoint;
    }
    public NavMeshData GetNavMeshData()
    {
        return navMeshData;
    }
    public List<Transform> GetStartPoints()
    {
        return startPoints;
    }
}
