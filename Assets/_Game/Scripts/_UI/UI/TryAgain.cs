using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryAgain : UICanvas
{
    [SerializeField] private TMPro.TextMeshProUGUI text_CountDown;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player();
        UIManager.Instance.CloseUI<InGame>();
        Show_Popup_Tryagain();

    }

    public void Show_Popup_Tryagain()
    {
        StartCoroutine(Waiter(text_CountDown));
    }

    public void RevieNowButton()
    {
        //IsRevive = true;
        player.OnInit();
        UIManager.Instance.OpenUI<InGame>();
        Close();
        //GameManager.Instance.ChangeState(GameState.InGame);
    }
    IEnumerator Waiter(TMPro.TextMeshProUGUI text_CountDown)
    {
        text_CountDown.text = "5";
        GameManager.Instance.SoundManager().PlayCountDownSoundEffect(0);
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "4";
        GameManager.Instance.SoundManager().PlayCountDownSoundEffect(1);
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "3";
        GameManager.Instance.SoundManager().PlayCountDownSoundEffect(0);
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "2";
        GameManager.Instance.SoundManager().PlayCountDownSoundEffect(1);
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "1";
        GameManager.Instance.SoundManager().PlayCountDownSoundEffect(0);
        yield return new WaitForSeconds(1f);
        text_CountDown.text = "0";
        GameManager.Instance.SoundManager().PlayCountDownSoundEffect(2);
        if (player.IsDeath)
        {
            UIManager.Instance.OpenUI<Lose>();
            Close();
        }
        
    }
    public void ExitButton()
    {
        UIManager.Instance.OpenUI<Lose>();
        Close();
    }
}
