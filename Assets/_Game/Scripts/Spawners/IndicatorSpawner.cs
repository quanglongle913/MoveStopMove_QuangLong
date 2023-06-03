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
       
        if (!gameManager.IsInitIndicator && gameManager.BotAIList.Count>0)
        {
            total = gameManager.BotAIList.Count;

            GenerateDetection(total);
            GenerateCharacterInfo(total+1);
            gameManager.IsInitIndicator=true;
            gameManager.IsInit = true;
        }
        if (gameManager.IsInitIndicator && gameManager.GameState==GameState.InGame && gameManager.BotAIList.Count>0)
        {
            Vector3 newPos = new Vector3(0, 0, -rangeDetection);
            if (radarCam.transform.localPosition != newPos)
            {
                radarCam.transform.localPosition = newPos;
            }
            if (gameManager != null && gameManager.IndicatorList.Count > 0)
            {
                GenerateRadar(mainCam, radarCam, gameManager, player);
                GenerateCharacterInfoPlayer(mainCam,gameManager,player);
            }
        }
    }
    private void GenerateDetection(int total)
    {
        for (int i = 0; i < total; i++)
        {
            PooledObject DetectionObject = Spawner(poolObject, poolMaster);
            gameManager.IndicatorList.Add(DetectionObject.GetComponent<Indicator>());
            Debug.Log("Indicator:" + i);
        }
    }
    private void GenerateCharacterInfo(int total)
    {
        for (int i = 0; i < total; i++)
        {
            PooledObject CharacterInfoObject = Spawner(poolObjectCharacterInfo, poolMaster);
            gameManager.CharacterInfoList.Add(CharacterInfoObject.GetComponent<CharacterInfo>());
            Debug.Log("Indicator:" + i);
        }
    }
    private void GenerateCharacterInfoPlayer(Camera mainCam, GameManager gameManager, GameObject player)
    {
        if (mainCam != null)
        {
            Vector3 viewPosPlayer = mainCam.WorldToViewportPoint(player.transform.position);
            Vector3 viewPosCharacterInfo = mainCam.WorldToScreenPoint(player.transform.position);
            // Your object is in the range of the cameraRadar, you can apply your behaviour(.)
            if (viewPosPlayer.x >= 0 && viewPosPlayer.x <= 1 && viewPosPlayer.y >= 0 && viewPosPlayer.y <= 1 && (viewPosPlayer.z > 0))
            {
                // Your object is in the range of the camera, you can apply your behaviour (.)
                gameManager.CharacterInfoList[0].gameObject.GetComponent<CharacterInfo>().setCharacterName(player.GetComponent<Character>().characterName);
                gameManager.CharacterInfoList[0].gameObject.transform.position = new Vector2(viewPosCharacterInfo.x, viewPosCharacterInfo.y + 1.4f * Screen.height / 10);
                gameManager.CharacterInfoList[0].gameObject.SetActive(true);

            }
            else
            {
                gameManager.CharacterInfoList[0].gameObject.SetActive(false);
            }
            for (int i = 0; i < gameManager.BotAIList.Count; i++)
            {

                Vector3 viewPos = mainCam.WorldToViewportPoint(gameManager.BotAIList[i].gameObject.transform.position);
                Vector3 viewPosCharacterInfoBotAI = mainCam.WorldToScreenPoint(gameManager.BotAIList[i].gameObject.transform.position);
                // Your object is in the range of the cameraRadar, you can apply your behaviour(.)
                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && (viewPos.z > 0))
                {
                    // Your object is in the range of the camera, you can apply your behaviour (.)
                    gameManager.CharacterInfoList[i+1].gameObject.GetComponent<CharacterInfo>().setCharacterName(gameManager.BotAIList[i].GetComponent<Character>().characterName);
                    gameManager.CharacterInfoList[i+1].gameObject.transform.position = new Vector2(viewPosCharacterInfoBotAI.x, viewPosCharacterInfoBotAI.y + 1.4f * Screen.height / 10);
                    gameManager.CharacterInfoList[i + 1].gameObject.SetActive(true);

                    //TODO  CharacterInfo set active true
                }
                else
                {
                    gameManager.CharacterInfoList[i + 1].gameObject.SetActive(false);

                }

            }
        }
    }
    private void GenerateRadar(Camera mainCam, Camera radarCam, GameManager gameManager, GameObject player)
    {
        if (mainCam != null)
        {
            for (int i = 0; i < gameManager.BotAIList.Count; i++)
            {
                Vector3 viewPosPlayer = mainCam.WorldToViewportPoint(player.gameObject.transform.position);
                Vector3 viewPos = mainCam.WorldToViewportPoint(gameManager.BotAIList[i].gameObject.transform.position);
                Vector3 viewPosRadar = radarCam.WorldToViewportPoint(gameManager.BotAIList[i].gameObject.transform.position);
                Vector3 viewPosDetection = mainCam.WorldToScreenPoint(gameManager.BotAIList[i].gameObject.transform.position);
                Vector3 viewPosDetectionRadar = radarCam.WorldToScreenPoint(gameManager.BotAIList[i].gameObject.transform.position);

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
                        gameManager.IndicatorList[i].ChangeColor(gameManager.BotAIList[i].ColorType);
                        gameManager.IndicatorList[i].gameObject.SetActive(true);
                        //TODO active CharacterInfo set active false
                        //gameManager.CharacterInfoList[i].gameObject.SetActive(false);
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
