using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateAnimal : IState<AnimalAI>
{
    float timer;
    float timerAttack;
    public void OnEnter(AnimalAI t)
    {
        t.Agent.isStopped = true;
        t.Attack();
        timerAttack = 1f;
    }

    public void OnExecute(AnimalAI t)
    {
        timer += Time.deltaTime;
        //t.RotateTowards(t._GameManager.Player.gameObject, t.Direction);
        if (timer > timerAttack)
        {
            if (Constant.IsDes(t.Target.transform.position, t.Animal.transform.position, 2.0f))
            {
                Debug.Log("Attack");
                t.Attack();
            }
            else
            {
                t.ChangeState(new PatrolStateAnimal());
            }

            timer = 0;
        }
    }

    public void OnExit(AnimalAI t)
    {

    }
}
