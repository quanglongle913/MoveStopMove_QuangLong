using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState<BotAI>
{
    float timer;
    void IState<BotAI>.OnEnter(BotAI t)
    {
        t.moveToTarget(t.gameObject.transform.position);
        t.ChangeAnim("Dead");
        timer = 0;
    }

    void IState<BotAI>.OnExecute(BotAI t)
    {
        timer += Time.deltaTime;
        if (timer > 1.5f)
        { 
            t.OnDespawn();
        }
    }

    void IState<BotAI>.OnExit(BotAI t)
    {
       
    }

    
}
