using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateAnimal : IState<AnimalAI>
{
    float timer;
    public void OnEnter(AnimalAI t)
    {
        t.Agent.isStopped = true;
        t.ChangeAnim("Dead");
        timer = 0;
    }

    public void OnExecute(AnimalAI t)
    {
        timer += Time.deltaTime;
        if (timer > 1.5f)
        {
            t.OnDespawn();
        }
    }

    public void OnExit(AnimalAI t)
    {

    }
}
