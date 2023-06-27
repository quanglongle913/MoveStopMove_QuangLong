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
    }

    public void OnExecute(Animal t)
    {
        target = GameManager.Instance.Player().transform.position;
        t.SetDestination(target);
        if (Constant.IsDes(t.transform.position, target, 0.2f))
        {
            t.ChangeState(new IdleStateA());
        }
    }

    public void OnExit(Animal t)
    {

    }

}
