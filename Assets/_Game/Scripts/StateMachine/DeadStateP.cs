using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateP : IState<Player>
{
    float timer;
    public void OnEnter(Player t)
    {
        t.ChangeAnim(nameof(AnimType.Dead));
        timer = 0;
    }

    public void OnExecute(Player t)
    {
        timer += Time.deltaTime;
        if (timer > 1.5f)
        {
            t.OnDespawn();
        }
    }

    public void OnExit(Player t)
    {

    }
}

