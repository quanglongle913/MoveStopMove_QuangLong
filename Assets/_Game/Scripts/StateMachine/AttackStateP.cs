using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateP : IState<Player>
{
    float timer;
    public void OnEnter(Player t)
    {
        t.timeAttack = (float)Math.Round(60 / t.InGameAttackSpeed, 1);
        timer = t.timeAttack - 0.1f;
        t.Anim.speed = (float)Math.Round(t.InGameAttackSpeed / 60, 1);
    }

    public void OnExecute(Player t)
    {
        timer += Time.deltaTime;
        
        if (timer > t.timeAttack)
        {
            if (t.IsTargerInRange && !t.IsAttacking)
            {
                t.Attack();
            }
            else
            {
                t.ChangeState(new IdleStateP());
            }
            timer = 0;
        }
        if (Mathf.Abs(t.Horizontal) >= 0.03 || Mathf.Abs(t.Vertical) >= 0.03)
        {
            if(t.IsMoving)
            {
                t.ChangeState(new PatrolStateP());
            }
        }
    }

    public void OnExit(Player t)
    {

    }
}
