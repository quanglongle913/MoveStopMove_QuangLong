using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : PooledObject
{
    [SerializeField] Camera mainCam;
    [SerializeField] Camera radarCam;
    [SerializeField] GameObject player;
    [SerializeField] GameObject poolMaster;
    [SerializeField] int total;
    [SerializeField] private ObjectPool poolObject;
    [SerializeField] private float rangeDetection;

    private BotAIManager botAIManager;
    public List<Indicator> indicatorList;

    public static IndicatorManager Instance;
    private bool isInit;

    public bool IsInit { get => isInit; set => isInit = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
       
    }
    // Start is called before the first frame update
    void Start()
    {
        IsInit = false;
        indicatorList = new List<Indicator>();
        botAIManager = BotAIManager.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (botAIManager.IsInit && !IsInit)
        {
            total = botAIManager.TotalBotAI;
            GenerateDetection(total);
        }
        if (IsInit)
        {
            Vector3 newPos = new Vector3(0, 0, -rangeDetection);
            if (radarCam.transform.localPosition != newPos)
            {
                radarCam.transform.localPosition = newPos;
            }
            if (botAIManager != null && indicatorList.Count > 0)
            {
                GenerateRadar(mainCam, radarCam, indicatorList, botAIManager, player);
            }
        }
       
    }
    private void GenerateDetection(int total)
    {
        for (int i = 0; i < total; i++)
        {
            PooledObject DetectionObject = Spawner(poolObject, poolMaster);
            indicatorList.Add(DetectionObject.GetComponent<Indicator>());
        }
        IsInit = true;
    }
    private void GenerateRadar(Camera mainCam, Camera radarCam, List<Indicator> indicatorList,BotAIManager  botAIManager,GameObject player)
    {
        if (mainCam != null)
        {
            for (int i = 0; i < botAIManager.BotAIList.Count; i++)
            {
                Vector3 viewPosPlayer = mainCam.WorldToViewportPoint(player.gameObject.transform.position);
                Vector3 viewPos = mainCam.WorldToViewportPoint(botAIManager.BotAIList[i].gameObject.transform.position);
                Vector3 viewPosRadar = radarCam.WorldToViewportPoint(botAIManager.BotAIList[i].gameObject.transform.position);
                Vector3 viewPosDetection = mainCam.WorldToScreenPoint(botAIManager.BotAIList[i].gameObject.transform.position);
                Vector3 viewPosDetectionRadar = radarCam.WorldToScreenPoint(botAIManager.BotAIList[i].gameObject.transform.position);

                //Debug.Log("target is viewPos.x:" + viewPos.x + " viewPos.y:" + viewPos.y + " viewPos.z:" + viewPos.z);
                if (viewPosRadar.x >= 0 && viewPosRadar.x <= 1 && viewPosRadar.y >= 0 && viewPosRadar.y <= 1 && (viewPosRadar.z > 0))
                {

                    // Your object is in the range of the cameraRadar, you can apply your behaviour(.)
                    if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && (viewPos.z > 0))
                    {
                        // Your object is in the range of the camera, you can apply your behaviour (.)
                        indicatorList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        //Debug.Log("viewPosDetectionRadar:" + viewPosDetectionRadar.x + ":" + viewPosDetectionRadar.y);
                        //Debug.Log("viewPosDetection:" + viewPosDetection.x + ":" + viewPosDetection.y);
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
                        indicatorList[i].gameObject.transform.position = new Vector2(viewPosDetectionRadar.x, viewPosDetectionRadar.y);
                        indicatorList[i].ChangeColor(botAIManager.BotAIList[i].ColorType);
                        indicatorList[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    indicatorList[i].gameObject.SetActive(false);
                }

                // Your object isn't in the range of the camera
                Vector2 A = new Vector2(viewPos.x, viewPos.y);
                Vector2 B = new Vector2(viewPosPlayer.x, viewPosPlayer.y);
                float angle = Constant.AngleBetween2Vector2Up(B, A);
                indicatorList[i].gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}