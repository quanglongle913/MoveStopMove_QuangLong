using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStateAnimal : IState<AnimalAI>
{
    public void OnEnter(AnimalAI t)
    {
        t.ChangeAnim("Run");
        
    }

    public void OnExecute(AnimalAI t)
    {
        if (t._GameManager.GameState == GameState.InGame)
        {
            //t.RotateTowards(t._GameManager.Player.gameObject, t.Direction);
            if (Constant.IsDes(t.Target.transform.position, t.Animal.transform.position, 2.0f))
            {
                t.ChangeState(new AttackStateAnimal());
            }
            else
            {
                t.Run(t.Target.transform);
            }
        }
        else
        {
            t.ChangeState(new IdleStateAnimal());
        }
    }

    public void OnExit(AnimalAI t)
    {

    }
}
