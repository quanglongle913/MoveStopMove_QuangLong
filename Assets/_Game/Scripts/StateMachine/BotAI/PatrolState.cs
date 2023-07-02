using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<BotAI>
{
    float timer;
    public void OnEnter(BotAI t)
    {
        
        t.ChangeAnim(nameof(AnimType.Run));
        //t.Anim.speed = (float)Math.Round(t.MoveSpeed / 10, 1);
     
        timer = 0;
        t.SetDestination(t.MoveTargetPoint);
    }

    public void OnExecute(BotAI t)
    {
        timer += Time.deltaTime;
        if (Constant.IsDes(t.transform.position, t.MoveTargetPoint, 0.1f))
        {
            //Debug.Log("timer:" + timer);
            t.ChangeState(new IdleState());
        }
        else if (timer > 8f)
        {
            timer = 0;
            Debug.Log("Error Wall: " + t.gameObject.name);
        }
    }

    public void OnExit(BotAI t)
    {

    }

}
