using DG.Tweening;
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
        if (this.GameManager.GameState == GameState.InGame)
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
            if (Weapons.Count <= 1)
            {
                Weapons.Add(gameObject.GetComponent<WeaponSpawner>().GenerateWeapon(WeaponMaster, this.PoolObject));
            }
        }
        else
        {
            ChangeState(new IdleState());
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
    public void Attack()
    {
        ////TODO Attack
        ChangeAnim("Attack");
        IsAttacking = true;
        Weapon.SetActive(false);
        Weapon weaponAttack = Weapons[0];
        Vector3 newTarget = new Vector3(Target.transform.position.x, weaponAttack.transform.position.y, Target.transform.position.z);
        Vector3 _Direction = new Vector3(newTarget.x - WeaponMaster.transform.position.x, _Rigidbody.velocity.y, newTarget.z - WeaponMaster.transform.position.z);
        Target.transform.position = newTarget;
        RotateTowards(this.gameObject, _Direction);
        weaponAttack.isFire = true;
        weaponAttack.gameObject.SetActive(true);
        weaponAttack.transform.DOMove(Target.transform.position, (float)Math.Round(60 / AttackSpeed, 1))
                    .SetEase(Ease.InSine)
                    .SetLoops(0, LoopType.Restart)
                    .OnComplete(() =>
                    {
                        Weapons.Remove(weaponAttack);
                        weaponAttack.gameObject.SetActive(false);
                        weaponAttack.gameObject.GetComponent<PooledObject>().Release();
                        weaponAttack.isFire = false;
                        Weapon.SetActive(true);
                        IsAttacking = false;
                    });

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