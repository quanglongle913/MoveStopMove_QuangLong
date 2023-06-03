using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : Singleton<IndicatorManager>
{
    private List<Indicator> indicatorList;

    private bool isInit;
    public bool IsInit { get => isInit; set => isInit = value; }
    public List<Indicator> IndicatorList { get => indicatorList; set => indicatorList = value; }

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }
    private void OnInit()
    {
        IsInit = false;
        indicatorList = new List<Indicator>();
    }
}
