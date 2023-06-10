using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBoxSpawner : PooledObject
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

            //GenerateGiftBox(_GameManager.TotalBotAI, GeneratePoolObjectPosition(poolMaster.transform.position, _GameManager.TotalBotAI));
        }
        else if (_GameManager.GameState == GameState.InGame && _GameManager.ListGiftBox.Count < _GameManager.GiftBoxNumber && _GameManager.IsInitBotAI)
        {
            GenerateGiftBox(1, GeneratePoolObjectPosition(poolMaster.transform.position, _GameManager.GiftBoxNumber*10));
        }
    }
    // Start is called before the first frame update
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

    private void GenerateGiftBox(int totalGiftBox, List<Vector3> listPoolObjectPosition)
    {
        for (int i = 0; i < totalGiftBox; i++)
        {
            int randomIndex = Random.Range(0, listPoolObjectPosition.Count);
            PooledObject botAIObject = Spawner(poolObject, poolMaster, true);
            botAIObject.transform.position = listPoolObjectPosition[randomIndex];
            listPoolObjectPosition.Remove(listPoolObjectPosition[randomIndex]);

            GiftBox giftBox = botAIObject.GetComponent<GiftBox>();

            _GameManager.ListGiftBox.Add(giftBox);
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
