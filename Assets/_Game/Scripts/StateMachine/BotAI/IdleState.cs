using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<BotAI>
{
    float timer;
    float randomTime;
    public void OnEnter(BotAI t)
    {
        t.MoveStop();
        t.ChangeAnim(nameof(AnimType.Idle));
        timer = 0;
        randomTime = Random.Range(1.0f,2.5f);
        t.MoveTargetPoint = t.RandomNavmeshLocation(t.InGameAttackRange);
    }

    public void OnExecute(BotAI t)
    {
        if (GameManager.Instance.IsState(GameState.InGame))
        {
            timer += Time.deltaTime;
            if (t.IsTargerInRange)
            {
                // Attack
                t.ChangeState(new AttackState());
            }
            else if (timer > randomTime)
            {
                t.ChangeState(new PatrolState());
            }
        }
            
    }

    public void OnExit(BotAI t)
    {
        //t.isStopped(false);
    }

}
