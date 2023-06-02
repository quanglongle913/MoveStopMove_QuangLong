using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateP : IState<Player>
{
    public void OnEnter(Player t)
    {
        //Debug.Log("IdleStateP");
        t.ChangeAnim("Idle");
    }

    public void OnExecute(Player t)
    {
        if (t.GameManager.GameState == GameState.InGame)
        {
            if (Mathf.Abs(t.Horizontal) >= 0.03 || Mathf.Abs(t.Vertical) >= 0.03)
            {
                t.ChangeState(new PatrolStateP());
            }
            else
            {
                //Check IsTargerInRange 
                if (t.IsTargerInRange)
                {
                    //Debug.Log("Enemy Detected......");
                    t.ChangeState(new AttackStateP());
                }
            }
        }    
    }

    public void OnExit(Player t)
    {

    }
}
