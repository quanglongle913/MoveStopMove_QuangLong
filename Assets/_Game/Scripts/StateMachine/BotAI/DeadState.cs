using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState<BotAI>
{
    float timer;
    public void OnEnter(BotAI t)
    {
        t.SetDestination(t.gameObject.transform.position);
        t.ChangeAnim("Dead");
        timer = 0;
    }

    public void OnExecute(BotAI t)
    {
        timer += Time.deltaTime;
        if (timer > 1.5f)
        { 
            t.OnDespawn();
        }
    }

    public void OnExit(BotAI t)
    {
       
    }

    
}
