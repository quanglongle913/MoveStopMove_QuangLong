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
    [SerializeField] private GameObject cylinder;
   
    private IState<Player> currentState;
    
    private float horizontal;
    private float vertical;
    public PlayerSkinShopState PlayerSkinShopState;

    public ColorType KillerColorType;
    public string KilledByName;
    public int Rank;
    public int KilledCount=0;
   
    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float Vertical { get => vertical; set => vertical = value; }

    private int maxHP;
    public int MaxHp() { return maxHP; }
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
        WeaponIndex = 0;
        this.WeaponIndex = GetWeaponsEquippedIndex(GameManager.Instance.GetWeaponData());
        this.WeaponType = GameManager.Instance.GetWeaponData().Weapon[WeaponIndex].WeaponType;
        SetWeaponSkinMat();
        ShowWeaponIndex((int)WeaponIndex);
        
        UpdateCharacterLvl();
        UpdateCharacterAcessories();
        UpdateAccessoriesEquippedAll();
    }
    public void OnInitSurvival()
    {
        base.OnInit();
        SetHp(100);
        SetInGameExp(0);
        ChangeState(new IdleStateP());
        WeaponIndex = 0;
        this.WeaponIndex = GetWeaponsEquippedIndex(GameManager.Instance.GetWeaponData());
        this.WeaponType = GameManager.Instance.GetWeaponData().Weapon[WeaponIndex].WeaponType;
        SetWeaponSkinMat();
        ShowWeaponIndex((int)WeaponIndex);

        UpdateCharacterLvl();
        UpdateCharacterAcessories();
        UpdateAccessoriesEquippedAll();
        SetLevel(1);
    }
    void Update()
    {
        if (GameManager.Instance.IsState(GameState.InGame))
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }

        }
        else
        { 
            ChangeState(new IdleStateP());
        }
        
    }
    public override void FixedUpdate()
    {
        
        //DetectionCharacter(CharactersInsideZone);
        base.FixedUpdate();
        if (cylinder != null)
        {
            cylinder.transform.localScale = new Vector3(InGameAttackRange * 2 / InGameSizeCharacter, 0.001f, InGameAttackRange * 2 / InGameSizeCharacter);
        }
        Horizontal = JoystickControl.direct.x; 
        Vertical = JoystickControl.direct.z;
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
        // Open UI Revice
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        UIManager.Instance.GetUI<InGame>().Close();
        UIManager.Instance.OpenUI<TryAgain>();
        UIManager.Instance.OpenUI<TryAgain>().Show_Popup_Tryagain();
        ChangeState(new DeadStateP());
    }
    public override void OnHit(float damage)
    {
        base.OnHit(damage);
    }
    public void SetTransformPosition(Transform transform)
    {
        gameObject.transform.position = transform.position;
    }
    public int GetWeaponsEquippedIndex(WeaponData weaponData)
    {
        int index = 0;
        for (int i = 0; i < weaponData.Weapon.Count; i++)
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
        for (int i = 0; i < weaponData.Weapon.Count; i++)
        {
            if (weaponData.Weapon[i].Buyed)
            {
                index = i;
            }
        }
        return index;
    }
    public int GetAccessorisSelectedIndex(AccessoriesData accessoriesData)
    {
        int index = 0;
        for (int i = 0; i < accessoriesData.Accessories.Count; i++)
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
        for (int i = 0; i < accessoriesData.Accessories.Count; i++)
        {
            accessoriesData.Accessories[i].Selected = false;
        }
    }
    public void SetPantsSkin(AccessoriesData accessoriesData)
    {
        SetAccessorisSkinMat(PantsSkin, accessoriesData, GetAccessorisSelectedIndex(accessoriesData));
    }
    public void UpdateAccessoriesEquippedAll()
    {
        HideAllSkin();
        HideAllSetFullsSkin();
        if (GetAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[3]))
        {
            UpdateAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[3]);
            PlayerSkinShopState = PlayerSkinShopState.SetFull;
        }
        else
        {
            if (!GetAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[0]) && !GetAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[1]) && !GetAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[2]))
            {
                PlayerSkinShopState = PlayerSkinShopState.None;

            }
            if (GetAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[0]))
            {
                UpdateAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[0]);

                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
            if (GetAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[1]))
            {
                UpdateAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[1]);

                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
            if (GetAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[2]))
            {
                UpdateAccessoriesEquipped(GameManager.Instance.GetAccessoriesDatas()[2]);

                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
        }
    }
    public void UpdateAccessoriesSkinShop()
    {
        HideAllSkin();
        HideAllSetFullsSkin();
        if (GetAccessoriesSelected(GameManager.Instance.GetAccessoriesDatas()[3]))
        {
            UpdateUIAccessoris(GameManager.Instance.GetAccessoriesDatas()[3]);

            PlayerSkinShopState = PlayerSkinShopState.SetFull;

        }
        else
        {
            if (!GetAccessoriesSelected(GameManager.Instance.GetAccessoriesDatas()[0]) && !GetAccessoriesSelected(GameManager.Instance.GetAccessoriesDatas()[1]) && !GetAccessoriesSelected(GameManager.Instance.GetAccessoriesDatas()[2]))
            {
                PlayerSkinShopState = PlayerSkinShopState.None;

            }
            if (GetAccessoriesSelected(GameManager.Instance.GetAccessoriesDatas()[0]))
            {
                UpdateUIAccessoris(GameManager.Instance.GetAccessoriesDatas()[0]);

                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
            if (GetAccessoriesSelected(GameManager.Instance.GetAccessoriesDatas()[1]))
            {

                UpdateUIAccessoris(GameManager.Instance.GetAccessoriesDatas()[1]);

                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
            if (GetAccessoriesSelected(GameManager.Instance.GetAccessoriesDatas()[2]))
            {
                UpdateUIAccessoris(GameManager.Instance.GetAccessoriesDatas()[2]);

                PlayerSkinShopState = PlayerSkinShopState.UnSetFull;
            }
        }
    }
    public void UpdateAccessoriesEquipped(AccessoriesData accessoriesData)
    {
        for (int i = 0; i < accessoriesData.Accessories.Count; i++)
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
                    SetPantsSkin(GameManager.Instance.GetAccessoriesDatas()[1]);
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
        //
        UpdateCharacterAcessories();
    }
    public void UpdateUIAccessoris(AccessoriesData accessoriesData)
    {
        for (int i = 0; i < accessoriesData.Accessories.Count; i++)
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
                    SetPantsSkin(GameManager.Instance.GetAccessoriesDatas()[1]);
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
        for (int i = 0; i < accessoriesData.Accessories.Count; i++)
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
        for (int i = 0; i < accessoriesData.Accessories.Count; i++)
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
        for (int i = 0; i < accessoriesData.Accessories.Count; i++)
        {
            accessoriesData.Accessories[i].Selected = false;

        }
    }
}

