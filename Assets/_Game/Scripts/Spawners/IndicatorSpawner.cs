using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IndicatorSpawner : PooledObject
{
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if (!gameManager.IsInitIndicator && gameManager.TotalBotAI_InGame>0)
        {
            total = gameManager.TotalBotAI_InGame;
            GenerateDetection(total);
            GenerateCharacterInfo(total+1);
            gameManager.IsInitIndicator=true;
            gameManager.IsInit = true; //hoan tat IsInit
        }
        if (gameManager.IsInitIndicator && gameManager.GameState==GameState.InGame)
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
                if (gameManager.CharacterInfoList.Count+1 > gameManager.BotAIListEnable.Count)
                { 
                    for (int i = gameManager.BotAIListEnable.Count+1; i < gameManager.CharacterInfoList.Count; i++) 
                    {
                        gameManager.CharacterInfoList[i].gameObject.SetActive(false);
                    }
                }
                if (gameManager.IndicatorList.Count  > gameManager.BotAIListEnable.Count)
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
        // Your object is in the range of the cameraRadar, you can apply your behaviour(.)
        if (viewPosPlayer.x >= 0 && viewPosPlayer.x <= 1 && viewPosPlayer.y >= 0 && viewPosPlayer.y <= 1 && (viewPosPlayer.z > 0))
        {
            // Your object is in the range of the camera, you can apply your behaviour (.)
            CharacterInfo characterInfo = gameManager.CharacterInfoList[0].gameObject.GetComponent<CharacterInfo>();
            Character character = player.GetComponent<Character>();
            characterInfo.setCharacterName(character.CharacterName);
            characterInfo.setCharacterLevel(""+ character.CharacterLevel);
            characterInfo.ChangeColor(character.ColorType, character.ColorData);
            gameManager.CharacterInfoList[0].gameObject.transform.position = new Vector2(viewPosCharacterInfo.x, viewPosCharacterInfo.y + 1.4f * Screen.height / 10);
            gameManager.CharacterInfoList[0].gameObject.SetActive(true);

        }
        else
        {
            gameManager.CharacterInfoList[0].gameObject.SetActive(false);
        }
    }
    private void showCharacterInfoEnemy(Camera mainCam, GameManager gameManager)
    {

        for (int i = 0; i < gameManager.BotAIListEnable.Count; i++)
        {

            Vector3 viewPos = mainCam.WorldToViewportPoint(gameManager.BotAIListEnable[i].gameObject.transform.position);
            Vector3 viewPosCharacterInfoBotAI = mainCam.WorldToScreenPoint(gameManager.BotAIListEnable[i].gameObject.transform.position);
            // Your object is in the range of the cameraRadar, you can apply your behaviour(.)
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && (viewPos.z > 0))
            {
                // Your object is in the range of the camera, you can apply your behaviour (.)
                CharacterInfo characterInfo = gameManager.CharacterInfoList[i + 1].gameObject.GetComponent<CharacterInfo>();

                Character character = gameManager.BotAIListEnable[i].GetComponent<Character>();
                characterInfo.setCharacterName(character.CharacterName);
                characterInfo.setCharacterLevel("" + character.CharacterLevel);
                characterInfo.ChangeColor(character.ColorType, character.ColorData);


                gameManager.CharacterInfoList[i + 1].gameObject.transform.position = new Vector2(viewPosCharacterInfoBotAI.x, viewPosCharacterInfoBotAI.y + 1.4f * Screen.height / 10);
                gameManager.CharacterInfoList[i + 1].gameObject.SetActive(true);
            }
            else
            {
                gameManager.CharacterInfoList[i + 1].gameObject.SetActive(false);

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
                Vector3 viewPosPlayer = mainCam.WorldToViewportPoint(player.gameObject.transform.position);

                Vector3 viewPos = mainCam.WorldToViewportPoint(gameManager.BotAIListEnable[i].gameObject.transform.position);
                Vector3 viewPosRadar = radarCam.WorldToViewportPoint(gameManager.BotAIListEnable[i].gameObject.transform.position);

                Vector3 viewPosDetection = mainCam.WorldToScreenPoint(gameManager.BotAIListEnable[i].gameObject.transform.position);
                Vector3 viewPosDetectionRadar = radarCam.WorldToScreenPoint(gameManager.BotAIListEnable[i].gameObject.transform.position);

                //Debug.Log("target is viewPos.x:" + viewPos.x + " viewPos.y:" + viewPos.y + " viewPos.z:" + viewPos.z);
                if (viewPosRadar.x >= 0 && viewPosRadar.x <= 1 && viewPosRadar.y >= 0 && viewPosRadar.y <= 1 && (viewPosRadar.z > 0))
                {
                    // Your object is in the range of the cameraRadar, you can apply your behaviour(.)
                    if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && (viewPos.z > 0))
                    {
                        // Your object is in the range of the camera, you can apply your behaviour (.)
                        gameManager.IndicatorList[i].gameObject.SetActive(false);
                        //TODO  CharacterInfo set active true
                    }
                    else
                    {
                        if (viewPosDetection.y <= 0)
                        {
                            viewPosDetectionRadar.y = 0;
                            if (viewPosDetection.x >= 0)
                            {
                                if (viewPosDetection.x > Screen.width)
                                {
                                    viewPosDetectionRadar.x = Screen.width;
                                }
                                else
                                {
                                    viewPosDetectionRadar.x = (viewPosDetectionRadar.x + viewPosDetection.x) / 2;
                                    viewPosDetectionRadar.x = Math.Abs(viewPosDetectionRadar.x);
                                }
                            }
                            else
                            {
                                viewPosDetectionRadar.x = 0;
                            }
                        }
                        if (viewPosDetection.y > 0)
                        {
                            if (viewPosDetection.y > Screen.height)
                            {
                                viewPosDetectionRadar.y = Screen.height;
                            }
                            if (viewPosDetection.x >= 0)
                            {
                                if (viewPosDetection.x > Screen.width)
                                {
                                    viewPosDetectionRadar.x = Screen.width;
                                }
                                else
                                {
                                    viewPosDetectionRadar.x = (viewPosDetectionRadar.x + viewPosDetection.x) / 2;
                                    viewPosDetectionRadar.x = Math.Abs(viewPosDetectionRadar.x);
                                }
                            }
                            else
                            {
                                viewPosDetectionRadar.x = 0;
                            }
                        }
                        gameManager.IndicatorList[i].gameObject.transform.position = new Vector2(viewPosDetectionRadar.x, viewPosDetectionRadar.y);
                        gameManager.IndicatorList[i].updateData(gameManager.BotAIListEnable[i].ColorType, gameManager.BotAIListEnable[i].CharacterLevel);
                        gameManager.IndicatorList[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    gameManager.IndicatorList[i].gameObject.SetActive(false);
                    
                }

                // Your object isn't in the range of the camera
                Vector2 A = new Vector2(viewPos.x, viewPos.y);
                Vector2 B = new Vector2(viewPosPlayer.x, viewPosPlayer.y);
                float angle = Constant.AngleBetween2Vector2Up(B, A);
                gameManager.IndicatorList[i].gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}
