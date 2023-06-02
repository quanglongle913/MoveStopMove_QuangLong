using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private GameObject cylinder;
    [SerializeField] private GameObject weaponInHand;
    [SerializeField] private GameObject weaponAttack;

    private IState<Player> currentState;
    
    private float horizontal;
    private float vertical;
    public GameObject target;
    public FloatingJoystick FloatingJoystick { get => floatingJoystick; set => floatingJoystick = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }
    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float Vertical { get => vertical; set => vertical = value; }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ChangeState(new IdleStateP());
        weaponAttack.SetActive(false);
    }
    public override void OnInit()
    {
        base.OnInit();
    }
    void Update()
    {
        if (this.GameManager.GameState == GameState.InGame)
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
        } else
        { 
            ChangeState(new IdleStateP());
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (cylinder != null)
        {
            cylinder.transform.localScale = new Vector3(AttackRange * 2, 0.001f, AttackRange * 2);
        }
        Horizontal = FloatingJoystick.Horizontal;
        Vertical = FloatingJoystick.Vertical;
        EnableHideCircleAttack();
    }
   
    private void EnableHideCircleAttack()
    {
        EnableCircleAttack(CharactersInsideZone, true);
        EnableCircleAttack(CharactersOutsideZone, false);
    }
    public Ease ease;
    public void Attack() 
    {
        //TODO Attack
        weaponInHand.SetActive(false);
        weaponAttack.SetActive(true);
        weaponAttack.transform.position = weaponInHand.transform.position;
        Vector3 newTarget = new Vector3(target.transform.position.x, weaponAttack.transform.position.y, target.transform.position.z);
        weaponAttack.transform.DOMove(newTarget,0.5f)
                //.SetEase(Ease.InElastic)
                .SetEase(ease)
                .SetLoops(0, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    weaponInHand.SetActive(true);
                    weaponAttack.SetActive(false);
                });
    }

    public void Moving()
    {
        if (Mathf.Abs(horizontal) >= 0.03 || Mathf.Abs(vertical) >= 0.03)
        {
            Vector3 _Direction = new Vector3(horizontal * MoveSpeed * Time.fixedDeltaTime, rigidbody.velocity.y, vertical * MoveSpeed * Time.fixedDeltaTime);
            Vector3 TargetPoint = new Vector3(rigidbody.position.x + _Direction.x, rigidbody.position.y, rigidbody.position.z + _Direction.z);
            RotateTowards(this.gameObject, _Direction);
            if (!Constant.isWall(this.gameObject,LayerMask.GetMask(Constant.LAYOUT_WALL)))
            {
                transform.position = TargetPoint;
                //ChangeAnim("Run");
            }
        }
    }
    private void EnableCircleAttack(Collider[] colliders, bool enable)
    {
        foreach (Collider hitcollider in colliders)
        {
            if (hitcollider.GetComponent<BotAI>())
            {
                hitcollider.GetComponent<BotAI>().CircleAttack.SetActive(enable);
                target = hitcollider.gameObject;
            }
        }
    }
    public void ChangeState(IState<Player> state)
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

