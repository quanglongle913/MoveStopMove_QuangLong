using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAI : Character
{
    [SerializeField] private GameObject circleAttack;
    private IState<BotAI> currentState;
    private NavMeshAgent agent;
    private Transform targetTransform;

    public GameObject CircleAttack { get => circleAttack; set => circleAttack = value; }
    public Transform TargetTransform { get => targetTransform; set => targetTransform = value; }
    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }
    public override void Start()
    {
        base.Start();
    }
    public override void OnInit()
    {
        base.OnInit();
        
        if (CircleAttack.activeSelf)
        {
            CircleAttack.SetActive(false);
        }
        ChangeState(new IdleState());
       
    }

    // Update is called once per frame
    void Update()
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
            if (this.GameManager.GameState == GameState.InGame)
            {
                if (currentState != null)
                {
                    currentState.OnExecute(this);
                }
            }
            else
            {
                ChangeState(new IdleState());
            }
        }
    }
    public override void FixedUpdate() { 
        base.FixedUpdate();
    }

    public void moveToTarget(Vector3 targetTransform)
    {
        agent.SetDestination(targetTransform);
    }
    public Vector3 generateTargetTransform()
    {
        float posX = transform.position.x + UnityEngine.Random.Range(-AttackRange*2, AttackRange * 2);
        float posZ = transform.position.z + UnityEngine.Random.Range(-AttackRange * 2, AttackRange * 2);
        Vector3 target = new Vector3(posX, transform.position.y,posZ);
        return target;
    }
    public override void Attack()
    {
        base.Attack();
    }
    public void ChangeState(IState<BotAI> state)
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
        gameObject.GetComponent<PooledObject>().Release();
        OnInit();

    }
    protected override void OnDeath()
    {
        base.OnDeath();
        if (GameManager.BotAIListEnable.Count > 0)
        {
            /*for (int i = 0; i < GameManager.BotAIListEnable.Count; i++)
            {
                if (GameManager.BotAIListEnable[i].gameObject == gameObject)
                {
                    GameManager.CharacterInfoList[i + 1].GetComponent<PooledObject>().Release();
                    GameManager.IndicatorList[i].GetComponent<PooledObject>().Release();
                }
            }*/
            GameManager.BotAIListEnable.Remove(this.gameObject.GetComponent<BotAI>());
            //GameManager.TotalBotAI_InGame--;
            ChangeState(new DeadState());
        } 
    }
}