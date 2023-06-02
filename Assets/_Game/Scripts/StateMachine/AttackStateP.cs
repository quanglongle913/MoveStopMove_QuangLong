using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateP : IState<Player>
{
    float timer;
    float randomTime;
    public void OnEnter(Player t)
    {
        timer = 0;
        //randomTime = Random.Range(0.5f, 1.0f);
        randomTime = 0.5f;
        //Debug.Log("AttackStateP......");
        t.ChangeAnim("Attack");
    }

    public void OnExecute(Player t)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            t.Attack();
            timer = 0;
            Debug.Log("AttackStateP......");
            if (Mathf.Abs(t.Horizontal) >= 0.03 || Mathf.Abs(t.Vertical) >= 0.03)
            {
                t.ChangeState(new PatrolStateP());
            }
            else
            {
                if (t.IsTargerInRange)
                {
                    //Debug.Log("Enemy Detected......");
                    t.ChangeState(new AttackStateP());
                }
                else
                {
                    t.ChangeState(new IdleStateP());
                }

            }
        }
        
    }

    public void OnExit(Player t)
    {

    }
}
