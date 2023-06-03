using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStateP : IState<Player>
{
    public void OnEnter(Player t)
    {
        t.ChangeAnim("Dead");
    }

    public void OnExecute(Player t)
    {
       
    }

    public void OnExit(Player t)
    {

    }
}

