using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateA : IState<Animal>
{
    float timer;
    float randomTime;
    public void OnEnter(Animal t)
    {
        t.SetDestination(t.gameObject.transform.position);
        t.ChangeAnim(nameof(AnimType.Idle));
        timer = 0;
        randomTime = Random.Range(0.5f, 1.0f);
    }

    public void OnExecute(Animal t)
    {
        if (GameManager.Instance.IsState(GameState.InGame))
        {
            timer += Time.deltaTime;
            if (t.IsTargerInRange)
            {
                // Attack
                t.ChangeState(new AttackStateA());
                
            }
            else if (timer > randomTime)
            {
                t.ChangeState(new PatrolStateA());
            }
        }

    }

    public void OnExit(Animal t)
    {
        //t.isStopped(false);
    }

}