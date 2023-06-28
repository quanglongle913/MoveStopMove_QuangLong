using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingInGame : UICanvas
{

    public void HomeButton()
    {
        if (GameManager.Instance.IsMode(GameMode.Normal))
        {
            GameManager.Instance.LevelManager().OnRetry();
        }
        else
        {
            GameManager.Instance.LevelManager().OnRetrySurvival();
        }
        //UIManager.Instance.OpenUI<Loading>();
        Close();
    }
    public void ContinueButton()
    {
        UIManager.Instance.OpenUI<InGame>();
        Close();
    }
}
