using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateAnimal : IState<AnimalAI>
{
    float timer;
    public void OnEnter(AnimalAI t)
    {
        //Debug.Log("Idle");
        t.Agent.isStopped = true;
        t.ChangeAnim("Idle");
        timer = 2f;
    }

    public void OnExecute(AnimalAI t)
    {

        if (t._GameManager.GameState == GameState.InGame)
        {

            timer += Time.deltaTime;
            if (timer > 2f)
            {
              
                t.ChangeState(new PatrolStateAnimal());
                timer = 0;
            }
            
        }
    }

    public void OnExit(AnimalAI t)
    {
       
    }
}
