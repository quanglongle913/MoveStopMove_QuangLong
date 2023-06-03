using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateP : IState<Player>
{
    float timer;
    float timerAttack;
    public void OnEnter(Player t)
    {
        timer = 0;
        //randomTime = Random.Range(0.5f, 1.0f);
        timerAttack = (float)Math.Round(60 / t.AttackSpeed, 1);
        //Debug.Log("AttackStateP......" + timerAttack);
        t.Anim.speed = (float)Math.Round(t.AttackSpeed / 60, 1);
    }

    public void OnExecute(Player t)
    {
        timer += Time.deltaTime;
        
        if (timer > timerAttack)
        {
            //t.IsAttacking = true;
            t.Attack();
            timer = 0;
            //Debug.Log("AttackStateP......");
        }
        if (Mathf.Abs(t.Horizontal) >= 0.03 || Mathf.Abs(t.Vertical) >= 0.03)
        {
            t.ChangeState(new PatrolStateP());
        }
        //else
        //{
        //    //if (!t.IsTargerInRange)
        //    //{
        //    //    //Debug.Log("Enemy Detected......");
        //    //    t.ChangeState(new AttackStateP());
        //    //}
        //    //else
        //    //{
        //    //    t.ChangeState(new IdleStateP());
        //    //}
        //    t.ChangeState(new IdleStateP());
        //}
    }

    public void OnExit(Player t)
    {

    }
}
