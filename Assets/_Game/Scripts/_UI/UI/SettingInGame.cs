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
            GameManager.Instance.SurvivalManager().OnRetry();
        }
        Close();
    }
    public void ContinueButton()
    {
        if (GameManager.Instance.IsMode(GameMode.Normal))
        {
            UIManager.Instance.OpenUI<InGame>();
        }
        else
        {
            UIManager.Instance.OpenUI<InGameSurvival>();
        }
        
        Close();
    }
}
