using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAI : Character
{
    [SerializeField] private GameObject circleAttack;
    private IState<BotAI> currentState;

    public GameObject CircleAttack { get => circleAttack; set => circleAttack = value; }

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
