using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : UICanvas
{
    public void CloseButton()
    {
        if (GameManager.Instance.IsState(GameState.InGame))
        {
            UIManager.Instance.OpenUI<InGame>();
            Close();
        }
        if (GameManager.Instance.IsState(GameState.GameMenu))
        {
            UIManager.Instance.OpenUI<GameMenu>();
            //UIManager.Instance.GetUI<GameMenu>().UpdateData();
            Close();
        }
    }

}
