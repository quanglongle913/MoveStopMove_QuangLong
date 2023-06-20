using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class AnimalManager : PooledObject
{
    [SerializeField] private ObjectPool poolObject;
    [SerializeField] private GameObject poolMaster;
    [SerializeField] int animalNum;
    [SerializeField] float offset=20;
    [SerializeField] private float size_x;
    [SerializeField] private float size_z;

    GameManager _GameManager;
    bool isInit=false;
   
    private void Start()
    {
        _GameManager = GameManager.Instance;
        _GameManager.AnimalAIListEnable = new List<AnimalAI>();
        _GameManager.AnimalAIListStack = new List<AnimalAI>();
    }
    private void Update()
    {
        if (_GameManager.GameState == GameState.Loading && !isInit)
        {
            GenerateBotAI(animalNum, GeneratePoolObjectPosition(poolMaster.transform.position, 10));
            isInit = true;
        }
        else if (_GameManager.GameState == GameState.InGame)
        {
            //Debug.Log(_GameManager.GameMode);
            if (_GameManager.Player.KilledCount < 100 && _GameManager.GameMode == GameMode.Survival)
            {
                //Debug.Log(_GameManager.AnimalAIListEnable.Count);
                //Debug.Log(_GameManager.AnimalAIListStack.Count);
                if (_GameManager.AnimalAIListEnable.Count < animalNum && _GameManager.AnimalAIListStack.Count > 0)
                {
                    int randomIndex = Random.Range(0, _GameManager.AnimalAIListStack.Count);
                    _GameManager.AnimalAIListStack[randomIndex].gameObject.SetActive(true);
                    _GameManager.AnimalAIListEnable.Add(_GameManager.AnimalAIListStack[randomIndex]);
                    _GameManager.AnimalAIListStack.Remove(_GameManager.AnimalAIListStack[randomIndex]);
                    //Debug.Log(_GameManager.AnimalAIListEnable.Count);
                    
                    GenerateBotAI(1, GeneratePoolObjectPosition(poolMaster.transform.position, 10));
                    //Debug.Log(_GameManager.AnimalAIListStack.Count);
                }
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
            PooledObject animalAIObj = Spawner(poolObject, poolMaster, false);
            animalAIObj.transform.position = listPoolObjectPosition[randomIndex];
            listPoolObjectPosition.Remove(listPoolObjectPosition[randomIndex]);
            AnimalAI animalAI = animalAIObj.GetComponent<AnimalAI>();
            _GameManager.AnimalAIListStack.Add(animalAI);
        }
    }
    void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        int Row = Mathf.CeilToInt(Mathf.Sqrt(10));
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
