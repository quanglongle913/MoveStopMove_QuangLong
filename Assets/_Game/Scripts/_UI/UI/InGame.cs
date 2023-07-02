using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : UICanvas
{
    [SerializeField] private TMPro.TextMeshProUGUI textAlive;

     void Update()
    {
        if (textAlive != null)
        {
            textAlive.text = Constant.STRING_ALIVE+": "+ GameManager.Instance.GetBotCount();
        }
    }
    public void SettingButton()
    {
        UIManager.Instance.OpenUI<SettingInGame>();
        Close();
    }
}
