using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStateP : IState<Player>
{
    public void OnEnter(Player t)
    {
        Debug.Log("PatrolStateP");
        t.ChangeAnim("Run");
    }

    public void OnExecute(Player t)
    {

        if (Mathf.Abs(t.Horizontal) >= 0.03 || Mathf.Abs(t.Vertical) >= 0.03)
        {
            t.Moving();
        }
        else if (t.Horizontal == 0 || t.Vertical == 0)
        {
            t.ChangeState(new IdleStateP());
        }
    }

    public void OnExit(Player t)
    {
        
    } 
}
