using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : Character,IHit
{
    [SerializeField] private IState<Animal> currentState;
    private NavMeshAgent agent;
    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }
    public override void Start()
    {
        //base.Start();
        OnResetAnimal();
    }
    public override void OnInit()
    {
        //base.OnInit();
        OnResetAnimal();
        ChangeState(new IdleStateA());
    }
    public override void  Update()
    {

        if (IsDeath)
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }

        }
        else
        {
            if (GameManager.Instance.IsState(GameState.InGame))
            {
                //base.Update();
                GenerateZone();
                DetectionPlayer();
                if (currentState != null)
                {
                    currentState.OnExecute(this);
                }
                if (Constant.isWall(this.gameObject, LayerMask.GetMask(Constant.LAYOUT_WALL)))
                {
                    ChangeState(new IdleStateA());
                }
            }
        }

    }
    public override void OnHit(float damage)
    {
        base.OnHit(damage);
    }
    public void SetDestination(Vector3 vector)
    {
        agent.enabled = true;
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(vector);
        }
    }
    public void ChangeState(IState<Animal> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        SimplePool.Despawn(this);
    }
    protected override void OnDeath()
    {
        //base.OnDeath();
        ChangeState(new DeadStateA());
        GameManager.Instance.RemoveAnimals(this);
    }
    internal void MoveStop()
    {
        agent.enabled = false;
    }
}
