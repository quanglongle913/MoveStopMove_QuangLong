using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BotAI : Character
{
    [Header("------------BotAI--------------- ")]
    [SerializeField] private GameObject circleAttack;
    private IState<BotAI> currentState;
    private NavMeshAgent agent;
    private Transform targetTransform;
    //public bool IsKilledPlayer=false;
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
        this.WeaponType = WeaponData.Weapon[WeaponIndex].WeaponType;
        SetWeaponSkinMat(ListWeaponsInHand[(int)WeaponType].gameObject.GetComponent<Renderer>(), this.WeaponData, this.WeaponIndex);
        ShowWeaponIndex((int)WeaponType);
        PoolObject = _GameManager.PoolObject[(int)WeaponType];
        PoolObject.GetComponent<ObjectPool>().ObjectToPool.gameObject.GetComponent<Renderer>().material = WeaponData.Weapon[WeaponIndex].Mat;
        UpdateCharacterLvl();
        UpdateCharacterAcessories();
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
            if (this._GameManager.GameState == GameState.InGame)
            {
                if (currentState != null)
                {
                    currentState.OnExecute(this);
                }
                if (Constant.isWall(this.gameObject, LayerMask.GetMask(Constant.LAYOUT_WALL)))
                {
                    ChangeState(new IdleState());
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
    public override void OnHit(float damage)
    {
        base.OnHit(damage);
    }
    public void moveToTarget(Vector3 targetTransform)
    {
        //agent.SetDestination(targetTransform);
        agent.destination = targetTransform;
    }
    public void isStopped(bool check)
    {
        if (this.gameObject.activeSelf)
        {
            agent.isStopped = check;
        }
    }
    public Vector3 generateTargetTransform()
    {
        Vector3 target = transform.position;
        bool isCheck=false;
        while (!isCheck)
        {
            float posX = transform.position.x + UnityEngine.Random.Range(-InGamneAttackRange * 2, InGamneAttackRange * 2);
            float posZ = transform.position.z + UnityEngine.Random.Range(-InGamneAttackRange * 2, InGamneAttackRange * 2);
            target = new Vector3(posX, transform.position.y, posZ);
            isCheck = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, new NavMeshPath());
        }
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
    public void UpdateBotAIInfo(int indexBotAI, GameManager gameManager)
    {
        int randomColor = UnityEngine.Random.Range(0, 5);
        ColorType _colorType = (ColorType)randomColor;
        ColorType = _colorType;
        InGamneExp = 100;
        ChangeColor(gameObject, _colorType);
        BotAIInfo botAIInfo = gameManager.SaveData.BotAIData.BotAIInfo[indexBotAI];
        WeaponIndex = botAIInfo.Weapon;
        CharacterName = botAIInfo.BotAI_name;
        //SKin
        HideAllSetFullsSkin();
        HideAllSkin();
        if (botAIInfo.playerSkinShopState == PlayerSkinShopState.SetFull)
        {
            //Active Setfull Skin
            ActiveSetFullsSkin(botAIInfo.CharacterSkin[(int)SkinType.SetFull].Index);
        }
        else
        {
            ActiveHatsSkin(botAIInfo.CharacterSkin[(int)SkinType.Hat].Index);
            SetAccessorisSkinMat(PantsSkin, gameManager.PantsData, botAIInfo.CharacterSkin[(int)SkinType.Pant].Index);
            ShowPantsSkin();
            ActiveSheildsSkin(botAIInfo.CharacterSkin[(int)SkinType.Sheild].Index);
        }
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnReset();
        gameObject.GetComponent<PooledObject>().Release();
        OnInit();

    }
    protected override void OnDeath()
    {
        base.OnDeath();
        if (_GameManager.BotAIListEnable.Count > 0)
        {
            _GameManager.BotAIListEnable.Remove(this.gameObject.GetComponent<BotAI>());
            ChangeState(new DeadState());
        } 
    }
}