using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<BotAI>
{
    float timer;
    float timerAttack;
    public void OnEnter(BotAI t)
    {
        timer = 0;
        timerAttack = (float)Math.Round(60 / t.AttackSpeedAfterbuff, 1);
        //Debug.Log("AttackStateP......" + timerAttack);
        t.Anim.speed = (float)Math.Round(t.AttackSpeedAfterbuff / 60, 1);
    }

    public void OnExecute(BotAI t)
    {
        timer += Time.deltaTime;

        if (timer > timerAttack)
        {
            if (t.IsTargerInRange)
            {
                t.Attack();
            }
            else
            {
                t.ChangeState(new IdleState());
            }
            timer = 0;
        }
    }

    public void OnExit(BotAI t)
    {

    }

}
