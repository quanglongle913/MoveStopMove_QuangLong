using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IndicatorSpawner : PooledObject
{
    [Range(0.5f, 0.9f)]
    [Tooltip("Distance offset of the indicators from the centre of the screen")]
    [SerializeField] private float screenBoundOffset = 0.9f;

    private Vector3 screenCentre;
    private Vector3 screenBounds;

    [SerializeField] Camera mainCam;
    [SerializeField] Camera radarCam;
    [SerializeField] GameObject player;
    [SerializeField] GameObject poolMaster;
    [SerializeField] int total;
    [SerializeField] private float rangeDetection;
    [Header("------------Indicator--------------- ")]
    [SerializeField] private ObjectPool poolObject;

    [Header("----------CharacterInfo------------- ")]
    [SerializeField] private ObjectPool poolObjectCharacterInfo;
    [SerializeField] private float axisY;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        //
        screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenBounds = screenCentre * screenBoundOffset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!gameManager.IsInitIndicator && gameManager.TotalBotAI_InGame > 0)
        {
            total = gameManager.TotalBotAI_InGame;
            GenerateDetection(total);
            GenerateCharacterInfo(total + 1);
            gameManager.IsInitIndicator = true;
            gameManager.IsInit = true; //hoan tat IsInit
        }
        if (gameManager.IsInitIndicator && gameManager.GameState == GameState.InGame)
        {
            if (gameManager.BotAIListEnable.Count > 0)
            {
                Vector3 newPos = new Vector3(0, 0, -rangeDetection);
                if (radarCam.transform.localPosition != newPos)
                {
                    radarCam.transform.localPosition = newPos;
                }
                if (gameManager != null && gameManager.IndicatorList.Count > 0)
                {
                    GenerateRadar(mainCam, radarCam, gameManager, player);

                    GenerateCharacterInfoPlayer(mainCam, gameManager, player);
                }
                if (gameManager.CharacterInfoList.Count + 1 > gameManager.BotAIListEnable.Count)
                {
                    for (int i = gameManager.BotAIListEnable.Count + 1; i < gameManager.CharacterInfoList.Count; i++)
                    {
                        gameManager.CharacterInfoList[i].gameObject.SetActive(false);
                    }
                }
                if (gameManager.IndicatorList.Count > gameManager.BotAIListEnable.Count)
                {
                    for (int i = gameManager.BotAIListEnable.Count; i < gameManager.IndicatorList.Count; i++)
                    {
                        gameManager.IndicatorList[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                for (int i = 1; i < gameManager.CharacterInfoList.Count; i++)
                {
                    gameManager.CharacterInfoList[i].gameObject.SetActive(false);
                }
            }

        }
    }
    private void GenerateDetection(int total)
    {
        ClearnDetection();
        for (int i = 0; i < total; i++)
        {
            PooledObject DetectionObject = Spawner(poolObject, poolMaster);
            gameManager.IndicatorList.Add(DetectionObject.GetComponent<Indicator>());
            //Debug.Log("Indicator:" + i);
        }
    }
    private void ClearnDetection()
    {
        for (int i = 0; i < gameManager.IndicatorList.Count; i++)
        {
            gameManager.IndicatorList[i].GetComponent<PooledObject>().Release();
        }
    }
    private void GenerateCharacterInfo(int total)
    {
        ClearnCharacterInfo();
        for (int i = 0; i < total; i++)
        {
            PooledObject CharacterInfoObject = Spawner(poolObjectCharacterInfo, poolMaster);
            gameManager.CharacterInfoList.Add(CharacterInfoObject.GetComponent<CharacterInfo>());
        }
    }
    private void ClearnCharacterInfo()
    {
        for (int i = 0; i < gameManager.CharacterInfoList.Count; i++)
        {
            gameManager.CharacterInfoList[i].GetComponent<PooledObject>().Release();
        }
    }
    private void GenerateCharacterInfoPlayer(Camera mainCam, GameManager gameManager, GameObject player)
    {
        if (mainCam != null)
        {
            showCharacterInfoPlayer(mainCam, gameManager, player);
            showCharacterInfoEnemy(mainCam, gameManager);
        }
    }
    private void showCharacterInfoPlayer(Camera mainCam, GameManager gameManager, GameObject player)
    {
        Vector3 viewPosPlayer = mainCam.WorldToViewportPoint(player.transform.position);
        Vector3 viewPosCharacterInfo = mainCam.WorldToScreenPoint(player.transform.position);
        CharacterInfo characterInfo = gameManager.CharacterInfoList[0];
        
        if (viewPosPlayer.x >= 0 && viewPosPlayer.x <= 1 && viewPosPlayer.y >= 0 && viewPosPlayer.y <= 1 && (viewPosPlayer.z > 0))
        {// Your object is in the range of the mainCam, you can apply your behaviour(.)
            //characterInfo.MainCam = mainCam;
            //characterInfo.Target = player;
            Character character = player.GetComponent<Character>();
            characterInfo.setCharacterName(character.CharacterName);
            characterInfo.setCharacterLevel("" + character.CharacterLevel);
            characterInfo.ChangeColor(character.ColorType, character.ColorData);
            if (character.ColorType == ColorType.Yellow)
            {
                characterInfo.CharacterLevel.color = Color.black;
            }
            else
            {
                characterInfo.CharacterLevel.color = Color.white;
            }
            characterInfo.gameObject.transform.position = new Vector2(viewPosCharacterInfo.x, viewPosCharacterInfo.y + 1.4f * Screen.height / 10);
            characterInfo.gameObject.SetActive(true);

        }
        else
        {
            characterInfo.gameObject.SetActive(false);
        }
    }
    
    private void showCharacterInfoEnemy(Camera mainCam, GameManager gameManager)
    {

        for (int i = 0; i < gameManager.BotAIListEnable.Count; i++)
        {
            Vector3 viewPos = mainCam.WorldToViewportPoint(gameManager.BotAIListEnable[i].gameObject.transform.position);
            Vector3 viewPosCharacterInfoBotAI = mainCam.WorldToScreenPoint(gameManager.BotAIListEnable[i].gameObject.transform.position);
            CharacterInfo characterInfo = gameManager.CharacterInfoList[i + 1];
            // Your object is in the range of the cameraRadar, you can apply your behaviour(.)
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && (viewPos.z > 0))
            {
                //characterInfo.MainCam = mainCam;
                //characterInfo.Target = gameManager.BotAIListEnable[i].gameObject;
                Character character = gameManager.BotAIListEnable[i].gameObject.GetComponent<Character>();
                characterInfo.setCharacterName(character.CharacterName);
                characterInfo.setCharacterLevel("" + character.CharacterLevel);
                characterInfo.ChangeColor(character.ColorType, character.ColorData);
                if (character.ColorType == ColorType.Yellow)
                {
                    characterInfo.CharacterLevel.color = Color.black;
                }
                else
                {
                    characterInfo.CharacterLevel.color = Color.white;
                }
                characterInfo.gameObject.transform.position = new Vector2(viewPosCharacterInfoBotAI.x, viewPosCharacterInfoBotAI.y + 1.4f * Screen.height / 10);
                characterInfo.gameObject.SetActive(true);
            }
            else
            {
                characterInfo.gameObject.SetActive(false);

            }
        }
    }
    private void GenerateRadar(Camera mainCam, Camera radarCam, GameManager gameManager, GameObject player)
    {
        if (mainCam != null && gameManager.BotAIListEnable.Count>0)
        {
            //Debug.Log("BotAI:" + gameManager.BotAIListEnable.Count);
            for (int i = 0; i < gameManager.BotAIListEnable.Count; i++)
            {
                Indicator indicator = gameManager.IndicatorList[i];
                GameObject target = gameManager.BotAIListEnable[i].gameObject;

                //indicator.Player = player;
                //indicator.Target = gameManager.BotAIListEnable[i].gameObject;
                //indicator.MainCam = mainCam;
                //indicator.RadarCam = radarCam;

                Vector3 viewPos = mainCam.WorldToViewportPoint(target.transform.position);
                Vector3 viewPosRadar = radarCam.WorldToViewportPoint(target.transform.position);

                //Debug.Log("target is viewPos.x:" + viewPos.x + " viewPos.y:" + viewPos.y + " viewPos.z:" + viewPos.z);
                if (viewPosRadar.x >= 0 && viewPosRadar.x <= 1 && viewPosRadar.y >= 0 && viewPosRadar.y <= 1 && (viewPosRadar.z > 0))
                {
                    // Your object is in the range of the cameraRadar, you can apply your behaviour(.)
                    if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && (viewPos.z > 0))
                    {
                        // Your object is in the range of the camera, you can apply your behaviour (.)
                        indicator.gameObject.SetActive(false);
                        //TODO  CharacterInfo set active true
                    }
                    else
                    {
                        indicator.updateData(gameManager.BotAIListEnable[i].ColorType, gameManager.BotAIListEnable[i].CharacterLevel);
                        indicator.gameObject.transform.position = OffScreenIndicatorCore.GetScreenPosition(mainCam, target,screenCentre,screenBounds);
                        indicator.gameObject.SetActive(true);
                        Vector3 viewPosPlayer = mainCam.WorldToViewportPoint(player.gameObject.transform.position);
                        Vector2 A = new Vector2(viewPos.x, viewPos.y);
                        Vector2 B = new Vector2(viewPosPlayer.x, viewPosPlayer.y);
                        //Debug.Log("X:" + viewPos.x + " Y:" + viewPos.y + " Z:" + viewPos.z);
                        //Debug.Log("X:"+viewPosPlayer.x+" Y:"+ viewPosPlayer.y+" Z:" + viewPosPlayer.z);
                        float angle1 = Constant.AngleBetween2Vector2Up(B, A);
                        //Debug.Log(angle1);
                        indicator.SetRotation(Quaternion.Euler(0, 0, angle1));
                        indicator.SetTextRotation(Quaternion.identity);
                    }
                }
                else
                {
                    indicator.gameObject.SetActive(false);

                } 
            }
        }
    }
}
