using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BotAI : Character
{
    [Header("------------BotAI--------------- ")]
    [SerializeField] private GameObject circleAttack;
    [SerializeField] private IState<BotAI> currentState;
    private NavMeshAgent agent;
    private Transform targetTransform;
    //public bool IsKilledPlayer=false;
    public GameObject CircleAttack { get => circleAttack; set => circleAttack = value; }
    public Transform TargetTransform { get => targetTransform; set => targetTransform = value; }
    public bool IsWall;

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
        agent.SetDestination(targetTransform);
        //agent.destination = targetTransform;
    }
    public void isStopped(bool check)
    {
        if (this.gameObject.activeSelf)
        {
            agent.isStopped = check;
        }
    }
    /*public Vector3 generateTargetTransform()
    {
        Vector3 target = transform.position;
        bool isCheck=false;
        while (!isCheck)
        {
            float offset = 2.0f;
            float posX = transform.position.x + UnityEngine.Random.Range(-InGameAttackRange *offset, InGameAttackRange * offset);
            float posZ = transform.position.z + UnityEngine.Random.Range(-InGameAttackRange * offset, InGameAttackRange * offset);
            target = new Vector3(posX, transform.position.y, posZ);
            isCheck = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas ,new NavMeshPath());
        }
        return target;
    }*/
    public Vector3 RandomNavmeshLocation(float radius)
    {
        
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        bool isCheck = false;
        while (!isCheck)
        {
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = new Vector3(hit.position.x, transform.position.y, hit.position.z);
                //Debug.Log(""+ finalPosition);
                isCheck = true;
                //isCheck = NavMesh.CalculatePath(transform.position, finalPosition, NavMesh.AllAreas, new NavMeshPath());
            }
        }
        return finalPosition;
    }
    public bool CheckWall()
    {
        RaycastHit hit;
        bool isWall = false;
        float range = Vector3.Distance(transform.position, Target.transform.position);
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.forward), out hit, range, LayerMask.GetMask(Constant.LAYOUT_WALL)))
        {
            isWall = true;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
        }
        return isWall;
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
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TransparentObstacle>())
        {
            Debug.Log("is Wall");
            ChangeState(new IdleState());
        }
    }*/
}