using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;

public class Player : Character
{
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private GameObject cylinder;
   
    private IState<Player> currentState;
    
    private float horizontal;
    private float vertical;

    public FloatingJoystick FloatingJoystick { get => floatingJoystick; set => floatingJoystick = value; }
   
    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float Vertical { get => vertical; set => vertical = value; }
    

    public override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ChangeState(new IdleStateP());
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
    //public Ease ease;
    public void Attack()
    {
        ////TODO Attack
        ChangeAnim("Attack");
        //Debug.Log("Attack");
        IsAttacking = true;
        ListWeaponsInHand[(int)WeaponType].gameObject.SetActive(false);

        Weapons weaponAttack = Weapons[0];
        Vector3 newTarget = new Vector3(Target.transform.position.x, weaponAttack.transform.position.y, Target.transform.position.z);
        Vector3 _Direction = new Vector3(newTarget.x - WeaponMaster.transform.position.x, _Rigidbody.velocity.y, newTarget.z- WeaponMaster.transform.position.z);

        RotateTowards(this.gameObject,_Direction);
        weaponAttack.isFire = true;
        weaponAttack.gameObject.SetActive(true);
        weaponAttack.transform.DOMove(newTarget, (float)Math.Round(60 / AttackSpeed, 1))
                    .SetEase(Ease.InSine)
                    .SetLoops(0, LoopType.Restart)
                    .OnComplete(() =>
                    {
                        Weapons.Remove(weaponAttack);
                        weaponAttack.gameObject.SetActive(false);
                        weaponAttack.gameObject.GetComponent<PooledObject>().Release();
                        weaponAttack.isFire = false;
                        ListWeaponsInHand[(int)WeaponType].gameObject.SetActive(true);
                        IsAttacking = false;
                    });

    }

    public void Moving()
    {
        if (Mathf.Abs(horizontal) >= 0.03 || Mathf.Abs(vertical) >= 0.03)
        {
            Vector3 _Direction = new Vector3(horizontal * MoveSpeed * Time.fixedDeltaTime, _Rigidbody.velocity.y, vertical * MoveSpeed * Time.fixedDeltaTime);
            Vector3 TargetPoint = new Vector3(_Rigidbody.position.x + _Direction.x, _Rigidbody.position.y, _Rigidbody.position.z + _Direction.z);
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
                //target = hitcollider.gameObject;
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

