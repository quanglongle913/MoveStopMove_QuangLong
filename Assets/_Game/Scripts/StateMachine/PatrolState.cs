using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<BotAI>
{
    Vector3 newTarget;
    public void OnEnter(BotAI t)
    {
        t.ChangeAnim("Run");
        //t.Anim.speed = (float)Math.Round(t.MoveSpeed / 10, 1);
        newTarget = t.generateTargetTransform();
    }

    public void OnExecute(BotAI t)
    {
        t.moveToTarget(newTarget);
        if (Constant.IsDes(t.transform.position, newTarget,0.1f))
        {
            t.ChangeState(new IdleState());
        }
    }

    public void OnExit(BotAI t)
    {

    }

}
