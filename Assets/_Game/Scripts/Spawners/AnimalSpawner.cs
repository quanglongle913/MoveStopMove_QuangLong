using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class AnimalSpawner : PooledObject
{
    [SerializeField] private ObjectPool poolObject;
    [SerializeField] private GameObject poolMaster;
    GameManager _GameManager;
    private List<AnimalAI> animalAIs;
    bool isInit=false;
    private void Start()
    {
        _GameManager = GameManager.Instance;
        animalAIs= new List<AnimalAI>();
    }
    private void Update()
    {
        if (_GameManager.GameState == GameState.Loading && !isInit)
        {

            GenerateBotAI(30, GeneratePoolObjectPosition(poolMaster.transform.position, 30));
            isInit = true;
        }
        else if (_GameManager.GameState == GameState.InGame)
        {
            if (animalAIs.Count > 0)
            {
                for (int i = 0; i < animalAIs.Count; i++)
                {
                    animalAIs[i].gameObject.SetActive(true);
                }
                
            }
        }
    }
    protected List<Vector3> GeneratePoolObjectPosition(Vector3 a_root, int numCount)
    {
        List<Vector3> listPoolObjectPosition = new List<Vector3>();
        int Row = Mathf.CeilToInt(Mathf.Sqrt(numCount));
        int Column = Row;
        int offset = 20;
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
            PooledObject animalAIObj = Spawner(poolObject, poolMaster, false);
            animalAIObj.transform.position = listPoolObjectPosition[randomIndex];
            listPoolObjectPosition.Remove(listPoolObjectPosition[randomIndex]);
            AnimalAI animalAI = animalAIObj.GetComponent<AnimalAI>();
            animalAIs.Add(animalAI);
        }
    }
}
