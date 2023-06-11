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

    public ColorType KillerColorType;
    public string KilledByName;
    public int Rank;
    public int KilledCount=0;
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
        ChangeState(new IdleStateP());
        //this.WeaponIndex = PlayerPrefs.GetInt(Constant.WEAPONS_USE, 0);
        this.WeaponIndex = GetWeaponsEquippedIndex(_GameManager.WeaponData);
        this.WeaponType = WeaponData.Weapon[WeaponIndex].WeaponType;

        PoolObject = _GameManager.PoolObject[(int)WeaponType];
        PoolObject.GetComponent<ObjectPool>().ObjectToPool.gameObject.GetComponent<Renderer>().material = WeaponData.Weapon[WeaponIndex].Mat;

        SetWeaponSkinMat(ListWeaponsInHand[(int)WeaponType].gameObject.GetComponent<Renderer>(), this.WeaponData, this.WeaponIndex);
        ShowWeaponIndex((int)WeaponType);
        
        UpdateCharacter();
        
        UpdateAccessoriesSkinShopOnInit();

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
            cylinder.transform.localScale = new Vector3(InGamneAttackRange * 2 / InGamneSizeCharacter, 0.001f, InGamneAttackRange * 2 / InGamneSizeCharacter);
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
        FloatingJoystick.OnReset();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(new DeadStateP());
    }
    public override void OnHit(float damage)
    {
        base.OnHit(damage);
    }
    public void SetEndGame()
    {
        Rank = _GameManager.BotAIListEnable.Count + _GameManager.BotAIListStack.Count + 1;
        if (Rank < PlayerPrefs.GetInt(Constant.BEST_RANK, 99))
        {
            PlayerPrefs.SetInt(Constant.BEST_RANK, Rank);
            PlayerPrefs.Save();
        }
        _GameManager.ZoneData.PlayerZoneExp += _GameManager.NumberOfBotsInGameLvel - Rank + 1;
        ZoneType playerZoneType = _GameManager.ZoneData.PlayerZoneType;

        if (_GameManager.ZoneData.PlayerZoneExp >= _GameManager.ZoneData.Zones[(int)playerZoneType].ZoneExp)
        {
            _GameManager.ZoneData.PlayerZoneExp -= _GameManager.ZoneData.Zones[(int)playerZoneType].ZoneExp;
            int zoneIndex = (int)_GameManager.ZoneData.PlayerZoneType + 1;
            _GameManager.ZoneData.PlayerZoneType = (ZoneType)zoneIndex;
        }
    }
    //Number of AccessorisBuyed in Accessories[]
    public int GetAccessorisBuyedIndex(AccessoriesData accessoriesData)
    {
        int index = 99;
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            if (!accessoriesData.Accessories[i].Buyed)
            {
                index = i;
                break;
            }
        }
        if (index == 99)
        {
            //You don't have any Skin
            Debug.Log("You don't have any Skin");
        }
        return index;
    }
    public int GetWeaponsEquippedIndex(WeaponData weaponData)
    {
        int index = 0;
        for (int i = 0; i < weaponData.Weapon.Length; i++)
        {
            if (weaponData.Weapon[i].Equipped)
            {
                index = i;
                break;
            }

        }
        return index;
    }
    public int GetNumberOfWeaponsHave(WeaponData weaponData)
    {
        int index = 0;
        for (int i = 0; i < weaponData.Weapon.Length; i++)
        {
            if (weaponData.Weapon[i].Buyed)
            {
                index=i;
            }
        }
        return index;
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
    public void UpdateAccessoriesSkinShopOnInit()
    {
        HideAllSkin();
        HideAllSetFullsSkin();
        if (GetAccessoriesEquipped(_GameManager.SetfullData))
        {
            UpdatePlayerAccessoris(_GameManager.SetfullData);
            PlayerSkinShopState = PlayerSkinShopState.SetFull;
        }
        else
        {
            if (!GetAccessoriesEquipped(_GameManager.HatsData) && !GetAccessoriesEquipped(_GameManager.PantsData) && !GetAccessoriesEquipped(_GameManager.ShieldData))
            {
                PlayerSkinShopState = PlayerSkinShopState.None;

            }
            if (GetAccessoriesEquipped(_GameManager.HatsData))
            {
                UpdatePlayerAccessoris(_GameManager.HatsData);

                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
            if (GetAccessoriesEquipped(_GameManager.PantsData))
            {
                UpdatePlayerAccessoris(_GameManager.PantsData);

                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
            if (GetAccessoriesEquipped(_GameManager.ShieldData))
            {
                UpdatePlayerAccessoris(_GameManager.ShieldData);

                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
        }
    }
    public void UpdateAccessoriesSkinShop()
    {
        HideAllSkin();
        HideAllSetFullsSkin();
        if (GetAccessoriesSelected(_GameManager.SetfullData))
        {
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
    public void UpdatePlayerAccessoris(AccessoriesData accessoriesData)
    {
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            if (accessoriesData.Accessories[i].Equipped)
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
    
    private bool GetAccessoriesEquipped(AccessoriesData accessoriesData)
    {
        bool isCheck = false;
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            if (accessoriesData.Accessories[i].Equipped)
            {
                isCheck = true;
                break;
            }

        }
        return isCheck;
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

