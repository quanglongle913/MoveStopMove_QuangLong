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
    //[SerializeField] GameObject player;
    [SerializeField] GameObject poolMaster;
    [SerializeField] int total;
    [SerializeField] private float rangeDetection;
    [Header("------------Indicator--------------- ")]
    [SerializeField] private ObjectPool poolObject;

    [Header("----------CharacterInfo------------- ")]
    [SerializeField] private ObjectPool poolObjectCharacterInfo;
    [SerializeField] private float axisY;
    private GameManager _GameManager;
    private Camera mainCam;
    private Camera radarCam;
    private Vector3 screenCentre;
    private Vector3 screenBounds;
    // Start is called before the first frame update
    void Start()
    {
        _GameManager = GameManager.Instance;
        screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenBounds = screenCentre * screenBoundOffset;
        mainCam = _GameManager.MainCam;
        radarCam= _GameManager.RadarCam;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!_GameManager.IsInitIndicator && _GameManager.TotalBotAI_InGame > 0)
        {
            total = _GameManager.TotalBotAI_InGame;
            GenerateDetection(total);
            GenerateCharacterInfo(total + 1);
            _GameManager.IsInitIndicator = true;
            _GameManager.IsInit = true; //hoan tat IsInit
        }
        if (_GameManager.IsInitIndicator && _GameManager.GameState == GameState.InGame)
        {
            if (_GameManager.BotAIListEnable.Count > 0)
            {
                Vector3 newPos = new Vector3(0, 0, -rangeDetection);
                if (radarCam.transform.localPosition != newPos)
                {
                    radarCam.transform.localPosition = newPos;
                }
                if (_GameManager != null && _GameManager.IndicatorList.Count > 0)
                {
                    GenerateIndicator(mainCam, radarCam, _GameManager.Player.gameObject);

                    GenerateCharacterInfoPlayer(mainCam, _GameManager.Player);
                }
                if (_GameManager.CharacterInfoList.Count + 1 > _GameManager.BotAIListEnable.Count)
                {
                    for (int i = _GameManager.BotAIListEnable.Count + 1; i < _GameManager.CharacterInfoList.Count; i++)
                    {
                        _GameManager.CharacterInfoList[i].gameObject.SetActive(false);
                    }
                }
                if (_GameManager.IndicatorList.Count > _GameManager.BotAIListEnable.Count)
                {
                    for (int i = _GameManager.BotAIListEnable.Count; i < _GameManager.IndicatorList.Count; i++)
                    {
                        _GameManager.IndicatorList[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                for (int i = 1; i < _GameManager.CharacterInfoList.Count; i++)
                {
                    _GameManager.CharacterInfoList[i].gameObject.SetActive(false);
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
            _GameManager.IndicatorList.Add(DetectionObject.GetComponent<Indicator>());
            //Debug.Log("Indicator:" + i);
        }
    }
    private void ClearnDetection()
    {
        for (int i = 0; i < _GameManager.IndicatorList.Count; i++)
        {
            _GameManager.IndicatorList[i].GetComponent<PooledObject>().Release();
        }
    }
    private void GenerateCharacterInfo(int total)
    {
        ClearnCharacterInfo();
        for (int i = 0; i < total; i++)
        {
            PooledObject CharacterInfoObject = Spawner(poolObjectCharacterInfo, poolMaster);
            _GameManager.CharacterInfoList.Add(CharacterInfoObject.GetComponent<CharacterInfo>());
        }
    }
    private void ClearnCharacterInfo()
    {
        for (int i = 0; i < _GameManager.CharacterInfoList.Count; i++)
        {
            _GameManager.CharacterInfoList[i].GetComponent<PooledObject>().Release();
        }
    }
    private void GenerateCharacterInfoPlayer(Camera mainCam, Player player)
    {
        if (mainCam != null)
        {
            ShowCharacterInfoPlayer(mainCam, player);
            ShowCharacterInfoEnemy(mainCam);
        }
    }
    private void ShowCharacterInfoPlayer(Camera mainCam, Player player)
    {
        Vector3 viewPosCharacterInfo = mainCam.WorldToScreenPoint(player.gameObject.transform.position);
        CharacterInfo characterInfo = _GameManager.CharacterInfoList[0];
        if (player.InCamera(mainCam))
        {
            characterInfo.setCharacterName(player.CharacterName);
            characterInfo.setCharacterLevel("" + player.CharacterLevel);
            characterInfo.ChangeColor(player.ColorType, player.ColorData);
            characterInfo.gameObject.transform.position = new Vector2(viewPosCharacterInfo.x, viewPosCharacterInfo.y + 1.4f * Screen.height / 10);
            characterInfo.gameObject.SetActive(true);
        }
        else
        {
            characterInfo.gameObject.SetActive(false);
        }
    }
    
    private void ShowCharacterInfoEnemy(Camera mainCam)
    {

        for (int i = 0; i < _GameManager.BotAIListEnable.Count; i++)
        {
            CharacterInfo characterInfo = _GameManager.CharacterInfoList[i + 1];
            if (_GameManager.BotAIListEnable[i].InCamera(_GameManager.MainCam)) {
                Vector3 viewPosCharacterInfoBotAI = mainCam.WorldToScreenPoint(_GameManager.BotAIListEnable[i].gameObject.transform.position);
                Character character = _GameManager.BotAIListEnable[i].gameObject.GetComponent<Character>();
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
    private void GenerateIndicator(Camera mainCam, Camera radarCam, GameObject player)
    {
        if (mainCam != null && _GameManager.BotAIListEnable.Count>0)
        {
            for (int i = 0; i < _GameManager.BotAIListEnable.Count; i++)
            {
                Indicator indicator = _GameManager.IndicatorList[i];
                if (_GameManager.BotAIListEnable[i].InCamera(radarCam))
                {
                    if (_GameManager.BotAIListEnable[i].InCamera(mainCam))
                    {
                        indicator.gameObject.SetActive(false);
                    }
                    else
                    {
                        GameObject target = _GameManager.BotAIListEnable[i].gameObject;
                        indicator.updateData(_GameManager.BotAIListEnable[i].ColorType, _GameManager.BotAIListEnable[i].CharacterLevel);
                        indicator.gameObject.transform.position = OffScreenIndicatorCore.GetScreenPosition(mainCam, target ,screenCentre,screenBounds);
                        indicator.gameObject.SetActive(true);
                        Vector3 viewPosPlayer = mainCam.WorldToViewportPoint(player.gameObject.transform.position);
                        Vector3 viewPos = mainCam.WorldToViewportPoint(target.transform.position);
                        Vector2 A = new Vector2(viewPos.x, viewPos.y);
                        Vector2 B = new Vector2(viewPosPlayer.x, viewPosPlayer.y);

                        float angle1 = GetAngleTwoVector2(A,B,viewPos.z);
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
    private float GetAngleTwoVector2(Vector2 A, Vector2 B,float asixZ)
    {
        float angle1 = 0;
        if (asixZ > 0)
        {
            angle1 = Constant.AngleBetween2Vector2Up(B, A);
        }
        else
        {
            angle1 = Constant.AngleBetween2Vector2Up(A, B);
        }
        return angle1;
    }
}
