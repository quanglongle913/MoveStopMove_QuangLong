using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState<BotAI>
{
    float timer;
    void IState<BotAI>.OnEnter(BotAI t)
    {
        t.ChangeAnim("Dead");
        timer = 0;
    }

    void IState<BotAI>.OnExecute(BotAI t)
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        { 
            t.GetComponent<PooledObject>().Release();
        }
    }

    void IState<BotAI>.OnExit(BotAI t)
    {
       
    }

    
}
