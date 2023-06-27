using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateA : IState<Animal>
{
    float timer;
    public void OnEnter(Animal t)
    {
        t.SetDestination(t.gameObject.transform.position);
        t.ChangeAnim("Dead");
        timer = 0;
    }

    public void OnExecute(Animal t)
    {
        timer += Time.deltaTime;
        if (timer > 1.5f)
        {
            t.OnDespawn();
        }
    }

    public void OnExit(Animal t)
    {

    }


}