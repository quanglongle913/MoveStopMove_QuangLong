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
        ChangeState(new IdleState());
        ChangeColor(mesh.gameObject,ColorType);
        if (CircleAttack.activeSelf)
        { 
            CircleAttack.SetActive(false);
        }
    }
    public override void OnInit()
    {
        base.OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
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
        float posX = transform.position.x + Random.Range(-AttackRange*2, AttackRange * 2);
        float posZ = transform.position.z + Random.Range(-AttackRange*2, AttackRange*2);
        Vector3 target = new Vector3(posX, transform.position.y,posZ);
        return target;
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

}
