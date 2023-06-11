using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BotAISpawner : PooledObject
{
    [SerializeField] GameObject poolMaster;
    [SerializeField] private ObjectPool poolObject;
    [SerializeField] float offset;
    [SerializeField] private float size_x;
    [SerializeField] private float size_z;
    [SerializeField] private GameManager _GameManager;

    private void Update()
    {
        if (_GameManager.GameState == GameState.Loading && !_GameManager.IsInitBotAI)
        {
  
            GenerateBotAI(_GameManager.TotalBotAI, GeneratePoolObjectPosition(poolMaster.transform.position, _GameManager.TotalBotAI));
            _GameManager.IsInitBotAI = true;
        }
        else if (_GameManager.GameState == GameState.InGame && _GameManager.BotAIListEnable.Count < _GameManager.TotalBotAI_InGame && _GameManager.IsInitBotAI)
        {
            if (_GameManager.BotAIListStack.Count > 0)
            {
                int randomIndex = Random.Range(0, _GameManager.BotAIListStack.Count);
                _GameManager.BotAIListStack[randomIndex].InGamneExp = _GameManager.LevelExpAverage;
                Debug.Log(_GameManager.BotAIListStack[randomIndex].InGamneExp);
                _GameManager.BotAIListStack[randomIndex].gameObject.SetActive(true);
                _GameManager.BotAIListStack[randomIndex].UpdateCharacterLvl();
                _GameManager.BotAIListEnable.Add(_GameManager.BotAIListStack[randomIndex]);
                _GameManager.BotAIListStack.Remove(_GameManager.BotAIListStack[randomIndex]);
                _GameManager.TotalBotAI--;
            }
        }
    }
    protected List<Vector3> GeneratePoolObjectPosition(Vector3 a_root, int numCount)
    {
        List<Vector3> listPoolObjectPosition = new List<Vector3>();
        int Row = Mathf.CeilToInt(Mathf.Sqrt(numCount));
        int Column = Row;
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                int index = Row * j + i;
                Vector3 objectPosition = new Vector3((j - (Row / 2)) + offset * j + a_root.x, 0.05f + a_root.y, ((Column / 2) - i) - offset * i + a_root.z);
                listPoolObjectPosition.Add(objectPosition);
            }
        }
        return listPoolObjectPosition;
    }
    
    private void GenerateBotAI(int totalBotAI, List<Vector3> listPoolObjectPosition)
    {
        for (int i = 0; i < totalBotAI; i++)
        {
            int randomIndex = Random.Range(0, listPoolObjectPosition.Count);
            PooledObject botAIObject = Spawner(poolObject, poolMaster,false);
            botAIObject.transform.position = listPoolObjectPosition[randomIndex];
            listPoolObjectPosition.Remove(listPoolObjectPosition[randomIndex]);

            BotAI botAI = botAIObject.GetComponent<BotAI>();
            botAI.UpdateBotAIInfo(i,_GameManager);
            _GameManager.BotAIListStack.Add(botAI);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        int Row = Mathf.CeilToInt(Mathf.Sqrt(_GameManager.TotalBotAI));
        int Column = Row;
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                int index = Row * j + i;
                Vector3 objectPosition = new Vector3((j - (Row / 2)) + offset * j + poolMaster.transform.position.x, 0.05f + poolMaster.transform.position.y, ((Column / 2) - i) - offset * i + poolMaster.transform.position.z);
                drawRectangle(objectPosition);
            }
        }

    }
    private void drawRectangle(Vector3 point)
    {
        //Top Left
        Vector3 topL = new Vector3(point.x - size_x, point.y, point.z + size_z);
        //Top Right
        Vector3 topR = new Vector3(point.x + size_x, point.y, point.z + size_z);
        //Bot Right
        Vector3 botR = new Vector3(point.x + size_x, point.y, point.z - size_z);
        //Bot Left
        Vector3 botL = new Vector3(point.x - size_x, point.y, point.z - size_z);

        Gizmos.DrawLine(topL, topR);
        Gizmos.DrawLine(topR, botR);
        Gizmos.DrawLine(botR, botL);
        Gizmos.DrawLine(botL, topL);
    }
}
