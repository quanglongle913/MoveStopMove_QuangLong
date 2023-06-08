using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;

public class Player : Character
{
    [Header("------------Player--------------- ")]
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private GameObject cylinder;
   
    private IState<Player> currentState;
    
    private float horizontal;
    private float vertical;
    public PlayerSkinShopState PlayerSkinShopState;
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
        
    }
    public override void OnInit()
    {
        base.OnInit();
        this.WeaponIndex = PlayerPrefs.GetInt(Constant.WEAPONS_USE, 14);
        ChangeState(new IdleStateP());
        SetWeaponSkinMat(ListWeaponsInHand[(int)WeaponType].gameObject.GetComponent<Renderer>(), this.WeaponData, this.WeaponIndex);
        UpdateAccessoriesSkinShop();
    }
    
    void Update()
    {
        if (this._GameManager.GameState == GameState.InGame)
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
            cylinder.transform.localScale = new Vector3(InGamneAttackRange * 2, 0.001f, InGamneAttackRange * 2);
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
    public override void Attack()
    {
       base.Attack();
    }

    public void Moving()
    {
        if (Mathf.Abs(horizontal) >= 0.03 || Mathf.Abs(vertical) >= 0.03)
        {
            Vector3 _Direction = new Vector3(horizontal * InGameMoveSpeed * Time.fixedDeltaTime, _Rigidbody.velocity.y, vertical * InGameMoveSpeed * Time.fixedDeltaTime);
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
    public override void OnDespawn()
    {
        base.OnDespawn();
        _GameManager.UIManager.setEndGame(false);
        //OnInit();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(new DeadStateP());
    }
 
    public int GetAccessorisSelectedIndex(AccessoriesData accessoriesData)
    {
        int index = 0;
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            if (accessoriesData.Accessories[i].Selected)
            {
                index = i;
                break;
            }
        }
        if (index == 0) {
            accessoriesData.Accessories[0].Selected = true; 
        }
        return index;
    }
    public void UnSelectedAccessoris(AccessoriesData accessoriesData)
    {
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            accessoriesData.Accessories[i].Selected=false;
        }
    }
    public void SetPantsSkin(AccessoriesData accessoriesData)
    {
        SetAccessorisSkinMat(PantsSkin, accessoriesData, GetAccessorisSelectedIndex(accessoriesData));
    }
    public void UpdateAccessoriesSkinShop()
    {
        HideAllSkin();
        HideAllSetFullsSkin();
        if (GetAccessoriesSelected(_GameManager.SetfullData))
        {
            SetAllAccessoriesUnSelected(_GameManager.HatsData);
            SetAllAccessoriesUnSelected(_GameManager.PantsData);
            SetAllAccessoriesUnSelected(_GameManager.ShieldData);
            UpdateUIAccessoris(_GameManager.SetfullData);
            PlayerSkinShopState = PlayerSkinShopState.SetFull;

        }
        else
        {
            if (!GetAccessoriesSelected(_GameManager.HatsData) && !GetAccessoriesSelected(_GameManager.PantsData) && !GetAccessoriesSelected(_GameManager.ShieldData))
            {
                PlayerSkinShopState = PlayerSkinShopState.None;
                
            }
            if (GetAccessoriesSelected(_GameManager.HatsData))
            {
                UpdateUIAccessoris(_GameManager.HatsData);
                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
            if (GetAccessoriesSelected(_GameManager.PantsData))
            {

                UpdateUIAccessoris(_GameManager.PantsData);
                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
            if (GetAccessoriesSelected(_GameManager.ShieldData))
            {
                UpdateUIAccessoris(_GameManager.ShieldData);
                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
        }
    }
    
    public void UpdateUIAccessoris(AccessoriesData accessoriesData)
    {
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            if (accessoriesData.Accessories[i].Selected)
            {
                if (accessoriesData.SkinType == SkinType.Hat)
                {
                    HideAllSetFullsSkin();
                    ActiveHatsSkin(i);
                }
                else if (accessoriesData.SkinType == SkinType.Pant)
                {
                    HideAllSetFullsSkin();
                    SetPantsSkin(_GameManager.PantsData);
                    ShowPantsSkin();
                }
                else if (accessoriesData.SkinType == SkinType.Sheild)
                {
                    HideAllSetFullsSkin();
                    ActiveSheildsSkin(i);
                }
                else if (accessoriesData.SkinType == SkinType.SetFull)
                {
                    HideAllSkin();
                    ActiveSetFullsSkin(i);
                }
            }
        }
    }
    private bool GetAccessoriesSelected(AccessoriesData accessoriesData)
    {
        bool isCheck = false;
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            if (accessoriesData.Accessories[i].Selected)
            {
                isCheck = true;
                break;
            }

        }
        return isCheck;
    }
    public void SetAllAccessoriesUnSelected(AccessoriesData accessoriesData)
    {
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            accessoriesData.Accessories[i].Selected = false;

        }
    }
    
}

