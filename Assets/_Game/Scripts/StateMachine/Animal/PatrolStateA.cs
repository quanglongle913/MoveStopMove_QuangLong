using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStateA : IState<Animal>
{
    Vector3 target;
    public void OnEnter(Animal t)
    {
        target = GameManager.Instance.Player().transform.position;
        t.ChangeAnim("Run");
        t.SetDestination(target);
    }

    public void OnExecute(Animal t)
    {
        target = GameManager.Instance.Player().transform.position;

        if (Constant.IsDes(t.transform.position, target, t.InGameAttackRange))
        {
            t.ChangeState(new IdleStateA());
        }
        else
        {
            t.SetDestination(target);
        }
    }

    public void OnExit(Animal t)
    {

    }

}
