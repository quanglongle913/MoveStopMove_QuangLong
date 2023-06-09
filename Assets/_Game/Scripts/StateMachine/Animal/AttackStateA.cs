using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateA : IState<Animal>
{
    float timer;
    float timerAttack;
    public void OnEnter(Animal t)
    {
        timerAttack = (float)Math.Round(60 / t.InGameAttackSpeed, 1);
        timer = timerAttack + 1;
        t.Anim.speed = (float)Math.Round(t.InGameAttackSpeed / 60, 1);
    }

    public void OnExecute(Animal t)
    {
        timer += Time.deltaTime;

        if (timer > timerAttack)
        {
            if (t.IsTargerInRange && !t.IsAttacking)
            {
                GameManager.Instance.Player().OnHit(1f);
                t.AnimalAttack();
            }
            else
            {
                t.ChangeState(new IdleStateA());
            }
            timer = 0;
        }
    }

    public void OnExit(Animal t)
    {

    }

}