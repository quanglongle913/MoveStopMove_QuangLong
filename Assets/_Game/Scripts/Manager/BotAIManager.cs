using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
public class BotAIManager : Singleton<BotAIManager>
{
    private List<BotAI> botAIList;
    private bool isInit;
    public List<BotAI> BotAIList { get => botAIList; set => botAIList = value; }
    public bool IsInit { get => isInit; set => isInit = value; }

    private void Start()
    {
        OnInit();
    }
    private void OnInit()
    {
        IsInit = false;
        botAIList = new List<BotAI>();
    }
}