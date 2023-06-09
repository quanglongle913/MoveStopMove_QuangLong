using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<BotAI>
{
    Vector3 newTarget;
    public void OnEnter(BotAI t)
    {
        t.ChangeAnim("Run");
        //t.Anim.speed = (float)Math.Round(t.MoveSpeed / 10, 1);
        newTarget = t.generateTargetTransform();
    }

    public void OnExecute(BotAI t)
    {
        t.moveToTarget(newTarget);
        //UNDONE
        /*RaycastHit hit;
        if (Physics.Raycast(t.transform.position, t.transform.TransformDirection(Vector3.forward), out hit, Constant.RAYCAST_HIT_RANGE_WALL, LayerMask.GetMask(Constant.LAYOUT_WALL)))
        {
            Debug.Log("Wall");
            t.ChangeState(new IdleState());
        }else if (Physics.Raycast(t.transform.position, t.transform.TransformDirection(1.5f, 0, 1), out hit, Constant.RAYCAST_HIT_RANGE_WALL, LayerMask.GetMask(Constant.LAYOUT_WALL)))
        {
            Debug.Log("Wall");
            t.ChangeState(new IdleState());
        }else if (Physics.Raycast(t.transform.position, t.transform.TransformDirection(-1.5f, 0, 1), out hit, Constant.RAYCAST_HIT_RANGE_WALL, LayerMask.GetMask(Constant.LAYOUT_WALL)))
        {
            Debug.Log("Wall");
            t.ChangeState(new IdleState());
        }*/
        //

        if (Constant.IsDes(t.transform.position, newTarget,0.1f))
        {
            t.ChangeState(new IdleState());
        }
    }

    public void OnExit(BotAI t)
    {
        t.isStopped(true);
    }

}
