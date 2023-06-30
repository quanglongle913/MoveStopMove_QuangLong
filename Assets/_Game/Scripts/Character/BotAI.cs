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
    private Vector3 moveTargetPoint;
    public Vector3 MoveTargetPoint { get => moveTargetPoint; set => moveTargetPoint = value; }
    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }
    public override void Start()
    {
        base.Start();
        moveTargetPoint = transform.position;
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
                //base.Update();
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
    public void DetectionCharacter(List<GameObject> objCharacters)
    {
        for (int i = 0; i < objCharacters.Count; i++)
        {
            Character character = Constant.Cache.GetCharacter(objCharacters[i]);
            if (!character.IsDeath && objCharacters[i] != this.gameObject && this.GetColorType() != character.GetColorType())
            {
                if (Constant.IsDes(transform.position, objCharacters[i].transform.position, InGameAttackRange))
                {
                    IsTargerInRange = true;
                    Target = objCharacters[i].transform;
                    break;
                }
            }
            else
            {
                IsTargerInRange = false;
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
        //agent.SetDestination(vector);
    }
    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        Vector3 moveDirection = new Vector3(randomDirection.x,0, randomDirection.y);
        moveDirection *= radius;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(transform.position+ moveDirection, out hit, radius, NavMesh.AllAreas))
        {
            finalPosition = new Vector3(hit.position.x, transform.position.y, hit.position.z);
            
        }
        else
        {
            return RandomNavmeshLocation(radius);
        }
        return finalPosition;
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