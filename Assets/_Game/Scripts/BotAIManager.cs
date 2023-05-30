using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

public class BotAIManager : PooledObject
{
    [SerializeField] GameObject poolMaster;
    [SerializeField] private ObjectPool poolObject;
    [SerializeField] int totalBotAI;
    [SerializeField] float offset;
    [SerializeField] private float size_x;
    [SerializeField] private float size_z;

    public List<BotAI> botAIList;  

    public static BotAIManager instance;
    private void Awake()
    {
        if(instance==null) 
        { 
            instance = this; 
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        botAIList=new List<BotAI>();
        StartCoroutine(coroutineGenerateBotAI(0.5f));
    }

    // Update is called once per frame
    void Update()
    {

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
    private IEnumerator coroutineGenerateBotAI(float time)
    { 
        yield return new WaitForSeconds(time);
        GenerateBotAI(totalBotAI, GeneratePoolObjectPosition(poolMaster.transform.position, totalBotAI));
    }
    private void GenerateBotAI(int totalBotAI, List<Vector3> listPoolObjectPosition)
    {
        for (int i=0;i<totalBotAI;i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, listPoolObjectPosition.Count);
            Vector3 a_vector3 = listPoolObjectPosition[randomIndex];
            PooledObject botAIObject = Spawner(poolObject, poolMaster);
            botAIObject.transform.position = a_vector3;
            listPoolObjectPosition.Remove(a_vector3);
            botAIList.Add(botAIObject.GetComponent<BotAI>());
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        int Row = Mathf.CeilToInt(Mathf.Sqrt(totalBotAI));
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
