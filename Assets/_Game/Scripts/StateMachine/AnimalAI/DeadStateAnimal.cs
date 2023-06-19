using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateAnimal : IState<AnimalAI>
{
    public void OnEnter(AnimalAI t)
    {
        t.ChangeAnim("Dead");
    }

    public void OnExecute(AnimalAI t)
    {
        if (t._GameManager.GameState == GameState.InGame)
        {

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
