using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingInGame : UICanvas
{

    public void HomeButton()
    {
        GameManager.Instance.LevelManager().OnRetry();
        Close();
    }
    public void ContinueButton()
    {
        UIManager.Instance.OpenUI<InGame>();
        Close();
    }
}
