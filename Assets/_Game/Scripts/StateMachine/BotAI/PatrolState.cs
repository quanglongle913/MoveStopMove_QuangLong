using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<BotAI>
{
    Vector3 newTarget;
    float timer;
    public void OnEnter(BotAI t)
    {
        t.isStopped(false);
        t.ChangeAnim("Run");
        //t.Anim.speed = (float)Math.Round(t.MoveSpeed / 10, 1);
        //newTarget = t.generateTargetTransform();
        newTarget = t.RandomNavmeshLocation(t.InGameAttackRange*2);
        timer = 0;
    }

    public void OnExecute(BotAI t)
    {
        timer += Time.deltaTime;
        t.moveToTarget(newTarget);
        if (Constant.IsDes(t.transform.position, newTarget, 0.1f))
        {
            //Debug.Log("timer:" + timer);
            t.ChangeState(new IdleState());
        }
        else if (timer > 4f)
        {
            timer = 0;
            Debug.Log("Error Wall: " + t.gameObject.name);
        }
    }

    public void OnExit(BotAI t)
    {

    }

}
