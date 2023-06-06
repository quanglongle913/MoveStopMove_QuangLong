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
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void Update()
    {
        if (gameManager.GameState == GameState.Loading && !gameManager.IsInitBotAI)
        {
  
            GenerateBotAI(gameManager.TotalBotAI, GeneratePoolObjectPosition(poolMaster.transform.position, gameManager.TotalBotAI));
            gameManager.IsInitBotAI = true;
        }
        else if (gameManager.GameState == GameState.InGame && gameManager.BotAIListEnable.Count < gameManager.TotalBotAI_InGame && gameManager.IsInitBotAI)
        {
            if (gameManager.BotAIListStack.Count > 0)
            {
                int randomIndex = Random.Range(0, gameManager.BotAIListStack.Count);
                gameManager.BotAIListStack[randomIndex].gameObject.SetActive(true);
                gameManager.BotAIListEnable.Add(gameManager.BotAIListStack[randomIndex]);
                gameManager.BotAIListStack.Remove(gameManager.BotAIListStack[randomIndex]);
                gameManager.TotalBotAI--;
            }
            else
            { 
                //WIN Action GameState = Endgame
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
            //Debug.Log("BotAI:" + listPoolObjectPosition.Count);
            int randomIndex = Random.Range(0, listPoolObjectPosition.Count);
            PooledObject botAIObject = Spawner(poolObject, poolMaster,false);
            botAIObject.transform.position = listPoolObjectPosition[randomIndex];
            listPoolObjectPosition.Remove(listPoolObjectPosition[randomIndex]);
            int randomColor = Random.Range(0, 5);
            ColorType _colorType = (ColorType)randomColor;
            BotAI botAI = botAIObject.GetComponent<BotAI>();
            botAI.ColorType = _colorType;
            botAI.InGamneExp = 100;
            botAI.ChangeColor(botAI.gameObject, _colorType);
            //Bot weapon = random (.) weapon List Have
            int weaponRandom = Random.Range(0, gameManager.SaveData.BotAIData.botAIInfo[i].weapon); //weapon power (bot have)
            //Debug.Log("BotAI:" + weapon);
            botAI.WeaponIndex = weaponRandom;
            //set paint= _characterSkin[1] skin with inde =random 0->index
            int paintRandom = Random.Range(0, gameManager.SaveData.BotAIData.botAIInfo[i]._characterSkin[1].index);
            botAI.setAccessorisSkinMat(botAI.PaintSkin, gameManager.PantsData, paintRandom); 

            botAI.CharacterName = gameManager.SaveData.BotAIData.botAIInfo[i].botAI_name;
            gameManager.BotAIListStack.Add(botAI);
            //Debug.Log("BotAI:" +weapon);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        int Row = Mathf.CeilToInt(Mathf.Sqrt(gameManager.TotalBotAI));
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
