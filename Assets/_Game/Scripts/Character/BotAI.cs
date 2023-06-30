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
    }

    // Update is called once per frame
    public override void Update()
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
                base.Update();
                if (currentState != null)
                {
                    currentState.OnExecute(this);
                }
                if (Constant.isWall(this.gameObject, LayerMask.GetMask(Constant.LAYOUT_WALL)))
                {
                    ChangeState(new IdleState());
                }
            }
        }

    }
    /*protected void DetectionCharacter(List<BotAI> botAIListEnable)
    {
        Player player = _GameManager.Player;
        if (!player.IsDeath && ColorType != player.ColorType)
        {
            if (Constant.IsDes(gameObject.transform.position, player.gameObject.transform.position, InGameAttackRange))
            {
                IsTargerInRange = true;
                Target = player.gameObject;
            }
            else
            {
                IsTargerInRange = false;
            }
        }
        if (!IsTargerInRange)
        {
            for (int i = 0; i < botAIListEnable.Count; i++)
            {

                if (!botAIListEnable[i].IsDeath && botAIListEnable[i].gameObject != this.gameObject && ColorType != botAIListEnable[i].ColorType)
                {
                    if (Constant.IsDes(gameObject.transform.position, botAIListEnable[i].gameObject.transform.position, InGameAttackRange))
                    {
                        IsTargerInRange = true;
                        Target = botAIListEnable[i].gameObject;
                        break;
                    }
                }
                else
                {
                    IsTargerInRange = false;
                }
            }
        }
        
        //Debug.Log("DetectionCharacter:"+ IsTargerInRange);
    }*/
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
    public Vector3 RandomNavmeshLocation(float radius)
    {
        
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        bool isCheck = false;
        while (!isCheck)
        {
            /// 3 is nav mesh Areas ID
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 3))
            {
                finalPosition = new Vector3(hit.position.x, transform.position.y, hit.position.z);
                //Debug.Log(""+ finalPosition);
                isCheck = true;
                //isCheck = NavMesh.CalculatePath(transform.position, finalPosition, NavMesh.AllAreas, new NavMeshPath());
            }
        }
        return finalPosition;
    }
    public Vector3 RandomNavmeshLocationToPlayer(float radius)
    {
        Vector3 _DirectionCharacter = new Vector3(GameManager.Instance.Player().transform.position.x - transform.position.x, _Rigidbody.velocity.y, GameManager.Instance.Player().transform.position.z - transform.position.z).normalized;
        Vector3 randomDirection = _DirectionCharacter * radius;
        randomDirection += transform.position;
        return randomDirection;
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
    public void UpdateInfo(BotAIInfo botAIInfo, List<AccessoriesData> accessoriesDatas)
    {
        int randomColor = UnityEngine.Random.Range(0, 5);
        InGamneExp = 100;
        ChangeColor(gameObject, (ColorType)randomColor);

        WeaponIndex = botAIInfo.Weapon;
        WeaponType = GameManager.Instance.GetWeaponData().Weapon[WeaponIndex].WeaponType;
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
            SetAccessorisSkinMat(PantsSkin, accessoriesDatas[1], botAIInfo.CharacterSkin[(int)SkinType.Pant].Index);
            ShowPantsSkin();
            ActiveSheildsSkin(botAIInfo.CharacterSkin[(int)SkinType.Sheild].Index);
        }
        SetWeaponSkinMat();
        ShowWeaponIndex((int)WeaponType);
        UpdateCharacterLvl();
        UpdateCharacterAcessories();
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        
        SimplePool.Despawn(this);
        
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        this.Indicator.Hide();
        this.CharacterInfo.Hide();
        GameManager.Instance.RemoveBotAIs(this);
        ChangeState(new DeadState());
    }
    internal void MoveStop()
    {
        agent.enabled = false;
    }
}