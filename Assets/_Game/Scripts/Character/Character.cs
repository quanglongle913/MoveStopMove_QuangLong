using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [Header("Character: ")]
    [SerializeField] private SkinnedMeshRenderer mesh;
    [SerializeField] private SkinnedMeshRenderer pantsSkin;
    [SerializeField] private ColorData colorData;
    [SerializeField] private ColorType colorType;
    [SerializeField] private List<Weapons> listWeaponsInHand;
    [Header("---------------BASE------------ ")]
    [SerializeField] private int inGamneExp = 100;
    [SerializeField] private float baseSizeCharacter = 1f;
    [SerializeField] private float baseAttackRange = 7f;
    [SerializeField] private float baseAttackSpeed = 60f;
    [SerializeField] private float baseMoveSpeed = 5.0f;
    [SerializeField] private float baseGoldEarn = 50f;
    [Header("--------------INGANE------------- ")]
    [SerializeField] private float inGameSizeCharacter = 1.0f;
    [SerializeField] private float inGameAttackRange = 7.0f;
    [SerializeField] private float inGameAttackSpeed = 60f;
    [SerializeField] private float inGameMoveSpeed = 5.0f;
    [SerializeField] private float inGameGold = 50f;
    [SerializeField] private float inGameGoldEarn = 50f;
    [SerializeField] private int inGamneZoneExp = 0;

    [SerializeField] private bool isTargerInRange;
    [SerializeField] private bool isAttacking;
    [Header("--------------------------- ")]
    [SerializeField] private GameObject weaponRoot;
    private ObjectPool poolObject;
    [Header("-------------Weapon-------------- ")]
    private WeaponType weaponType;
    private WeaponData weaponData;
    [Header("-------------Skin-------------- ")]
    [SerializeField] private List<GameObject> listHats;
    [SerializeField] private List<GameObject> listSheilds;
    [SerializeField] private List<GameObject> listSetFull;
    [SerializeField] private GameObject Pants;

    [Header("--------------------------- ")]
    [SerializeField] private string characterName;
    [SerializeField] private int characterLevel;


    public float hp;
    private bool isBuffed;
    public bool IsDeath => hp <= 0;
    private GameManager gameManager;

    private Animator anim;
    private Rigidbody rb;
    protected float rotationSpeed = 1000f;
    private string currentAnimName;
    private GameObject target;
    protected Collider[] CurrentCharacters;
    protected Collider[] CharactersInsideZone;
    protected Collider[] CharactersOutsideZone;
    
    

    public bool IsHaveWeapon;

    private List<Weapons> weapons;
    private int weaponIndex;
    public Rigidbody _Rigidbody { get => rb; set => rb = value; }
    public ColorData ColorData { get => colorData; set => colorData = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }

    public int InGamneExp { get => inGamneExp; set => inGamneExp = value; }
    public float InGameSizeCharacter { get => inGameSizeCharacter; set => inGameSizeCharacter = value; }
    public float InGameAttackRange { get => inGameAttackRange; set => inGameAttackRange = value; }
    public float InGameAttackSpeed { get => inGameAttackSpeed; set => inGameAttackSpeed = value; }
    public float InGameMoveSpeed { get => inGameMoveSpeed; set => inGameMoveSpeed = value; }
    public float InGameGold { get => inGameGold; set => inGameGold = value; }

    public bool IsTargerInRange { get => isTargerInRange; set => isTargerInRange = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }

    public GameManager _GameManager { get => gameManager; set => gameManager = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public GameObject Target { get => target; set => target = value; }
    public List<Weapons> Weapons { get => weapons; set => weapons = value; }
    public ObjectPool PoolObject { get => poolObject; set => poolObject = value; }
    public GameObject WeaponRoot { get => weaponRoot; set => weaponRoot = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public List<Weapons> ListWeaponsInHand { get => listWeaponsInHand; set => listWeaponsInHand = value; }
    public WeaponData WeaponData { get => weaponData; set => weaponData = value; }
    public string CharacterName { get => characterName; set => characterName = value; }
    public int WeaponIndex { get => weaponIndex; set => weaponIndex = value; }
    public SkinnedMeshRenderer PantsSkin { get => pantsSkin; set => pantsSkin = value; }
    public int CharacterLevel { get => characterLevel; set => characterLevel = value; }
    public bool IsBuffed { get => isBuffed; set => isBuffed = value; }
    public float InGameGoldEarn { get => inGameGoldEarn; set => inGameGoldEarn = value; }


    // Start is called before the first frame update
    public virtual void Awake()
    {
       
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    public virtual void Start()
    {
        _GameManager = GameManager.Instance;
        Weapons = new List<Weapons>();
        OnInit();
    }
    public virtual void OnInit()
    {
        IsAttacking = false;
        IsTargerInRange = false;
        hp = 1;
        OnReset();
        ChangeColor(gameObject, ColorType);
        this.WeaponData = _GameManager.WeaponData;
    }
    
    public virtual void FixedUpdate()
    {
        //GenerateZone();
        //DetectionCharacter(CharactersInsideZone);
        if (this._GameManager.GameState == GameState.InGame)
        {
            if (Weapons.Count <= 1)
            {
                Weapons a_weapon = _GameManager.WeaponSpawner.GenerateWeapon(_GameManager.WeaponHolder, PoolObject);
                //a_weapon.gameObject.transform.localScale = ListWeaponsInHand[(int)WeaponType].gameObject.transform.localScale;
                Weapons.Add(a_weapon);
            }
            for (int i = 0; i < Weapons.Count; i++)
            {
                Weapons[i].gameObject.transform.localScale = ListWeaponsInHand[(int)WeaponType].gameObject.transform.lossyScale;
            }
        }
    }
    public virtual void Attack() 
    {   
        //SoundEffect
        int randomNum = UnityEngine.Random.Range(0, _GameManager.SoundManager.WeaponThrowSoundEffect.Count);
        _GameManager.SoundManager.WeaponThrowSoundEffect[randomNum].Play();

        ChangeAnim("Attack");
        IsAttacking = true;
        HideAllWeaponsInHand();
        Weapons weaponAttack = Weapons[0];
        weaponAttack.transform.position = WeaponRoot.transform.position;
        weaponAttack._GameObject = gameObject;
        weaponAttack.WeaponType = this.WeaponType;
        Vector3 newTarget = new Vector3(Target.transform.position.x, weaponAttack.transform.position.y, Target.transform.position.z);
        Vector3 _Direction = new Vector3(newTarget.x - weaponAttack.transform.position.x, _Rigidbody.velocity.y, newTarget.z - weaponAttack.transform.position.z);
        RotateTowards(this.gameObject, _Direction);
        weaponAttack.isFire = true;
        weaponAttack.gameObject.SetActive(true);
        weaponAttack.direction = _Direction;
        weaponAttack.target = newTarget;
        weaponAttack.startPoint = gameObject.transform.position;
        //Move Weapon with DOMove
        //moveWeaponWithDOMove(gameObject.GetComponent<Character>(), weaponAttack);
    }
    private void moveWeaponWithDOMove(Character _character, Weapons _weapons)
    {
        _weapons.transform.DOMove(_weapons.target, (float)Math.Round(60 / _character.InGameAttackSpeed, 1))
                        .SetEase(Ease.InSine)
                        .SetLoops(0, LoopType.Restart)
                        .OnComplete(() =>
                        {
                            _character.Weapons.Remove(_weapons);
                            _character.ShowWeaponIndex((int)WeaponType);
                            _character.IsAttacking = false;
                            _weapons.gameObject.SetActive(false);
                            _weapons.gameObject.GetComponent<PooledObject>().Release();
                            _weapons.isFire = false;

                        });
    }
    protected void GenerateZone()
    {
        CurrentCharacters = Physics.OverlapSphere(this.transform.position, 1000f, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersInsideZone = Physics.OverlapSphere(this.transform.position, InGameAttackRange, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersOutsideZone = CurrentCharacters.Except(CharactersInsideZone).ToArray();
    }
    protected  void DetectionCharacter(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++) 
        {
            Character character = colliders[i].GetComponent<Character>();
            if (!character.IsDeath && colliders[i].gameObject != this.gameObject && colorType != character.colorType)
            {
                IsTargerInRange = true;
                Target = colliders[i].gameObject;
                break;
            }
            else
            {
                IsTargerInRange = false;
            }
        }
    }
    public virtual void OnDespawn()
    {

    }
    protected virtual void OnDeath()
    {
        //ChangeAnim("Dead");
        //Invoke(nameof(OnDespawn), 2f);
        int randomNum = UnityEngine.Random.Range(0, _GameManager.SoundManager.DeadSoundEffect.Count);
        _GameManager.SoundManager.DeadSoundEffect[randomNum].Play();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, InGameAttackRange);

    }
    public void RotateTowards(GameObject gameObject, Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
    public void ChangeColor(GameObject a_obj, ColorType colorType)
    {
        this.colorType = colorType;
        a_obj.GetComponent<Character>().mesh.material = colorData.GetMat(colorType);
    }

    public virtual void OnHit(float damage)
    {

        if (!IsDeath)
        {
            int randomNum = UnityEngine.Random.Range(0, _GameManager.SoundManager.WeaponHitSoundEffect.Count);
            _GameManager.SoundManager.WeaponHitSoundEffect[randomNum].Play();

            hp -= damage;
            if (IsDeath)
            {
                hp = 0;
                OnDeath();
            }
        }

    }
    //=====================Weapons===================
   
    //Set Material for Prefabs 
    public void SetWeaponSkinMat(Renderer renderer, WeaponData weaponData, int index)
    {
        var newMaterials = new Material[renderer.materials.Count()];

        for (int i = 0; i < newMaterials.Count(); i++)
        {
            newMaterials[i] = weaponData.Weapon[index].Mat;

        }
        renderer.materials = newMaterials;
    }
    public void SetAccessorisSkinMat(Renderer renderer, AccessoriesData accessoriesData, int index)
    { //For BOTAI
        if (index != 5555)
        {
            var newMaterials = new Material[renderer.materials.Count()];
            for (int i = 0; i < newMaterials.Count(); i++)
            {
                newMaterials[i] = accessoriesData.Accessories[index].Mat;

            }
            renderer.materials = newMaterials;
        }
       
    }
    public void HideAllWeaponsInHand()
    {
        for (int i = 0; i < ListWeaponsInHand.Count; i++)
        {
            listWeaponsInHand[i].gameObject.SetActive(false);
        }
    }
    public void ShowWeaponIndex(int index)
    {
        HideAllWeaponsInHand();
        listWeaponsInHand[index].gameObject.SetActive(true);
    }
    //=======================Skin===================
    public void HideAllSkin()
    {
        for (int i = 0; i < listHats.Count; i++)
        {
            listHats[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < listSheilds.Count; i++)
        {
            listSheilds[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < listSetFull.Count; i++)
        {
            listSetFull[i].gameObject.SetActive(false);
        }
        Pants.SetActive(false);
    }
    public void HideHatsSkin()
    {
        for (int i = 0; i < listHats.Count; i++)
        {
            listHats[i].gameObject.SetActive(false);
        }
    }
    public void ActiveHatsSkin(int index)
    {
        HideHatsSkin();
        listHats[index].gameObject.SetActive(true);
    }
    public void HideSheildsSkin()
    {
        for (int i = 0; i < listSheilds.Count; i++)
        {
            listSheilds[i].gameObject.SetActive(false);
        }
    }
    public void ActiveSheildsSkin(int index)
    {
        HideSheildsSkin();
        listSheilds[index].gameObject.SetActive(true);
    }
    public void HideAllSetFullsSkin()
    {
        for (int i = 0; i < listSetFull.Count; i++)
        {
            listSetFull[i].gameObject.SetActive(false);
        }
    }
    public void ActiveSetFullsSkin(int index)
    {
        HideAllSetFullsSkin();
        listSetFull[index].gameObject.SetActive(true);
        //Set Material
    }
    public void HidePantsSkin()
    {
        Pants.SetActive(false);
    }
    public void ShowPantsSkin()
    {
        Pants.SetActive(true);
    }

    //Set EXP when killed other character
    public void setExp(int exp)
    {
        InGamneExp += exp / CharacterLevel + 40;
        InGameGold += InGameGoldEarn;
        UpdateCharacterLvl();
    }
    public void OnReset()
    {
        InGameGold = 0;
        inGameSizeCharacter = baseSizeCharacter;
        inGameAttackRange = baseAttackRange;
        inGameAttackSpeed = baseAttackSpeed;
        inGameMoveSpeed = baseMoveSpeed;
        inGameGoldEarn = baseGoldEarn;
        UpdateCharacterLvl();
    }
    public void UpdateCharacterLvl()
    {
        CharacterLevel = InGamneExp / 100; //tinh level character
        float offsetSize = 0.05f;
        float offsetAttackSpeed = 0.04f;
        float offsetMoveSpeed = 0.3f;

        if (characterLevel == 1)
        {
            inGameSizeCharacter = baseSizeCharacter;
            inGameAttackRange = baseAttackRange;
            inGameAttackSpeed = baseAttackSpeed;
            inGameMoveSpeed = baseMoveSpeed;
        }
        else if (characterLevel <= 20)
        {
            inGameSizeCharacter = baseSizeCharacter + offsetSize * characterLevel;
            inGameAttackRange = baseAttackRange + offsetSize * characterLevel;
            inGameAttackSpeed = baseAttackSpeed + offsetAttackSpeed * characterLevel;
            inGameMoveSpeed = baseMoveSpeed + offsetMoveSpeed * characterLevel;
        }
        transform.localScale = new Vector3(inGameSizeCharacter, inGameSizeCharacter, inGameSizeCharacter);
    }
    public void UpdateCharacterAcessories()
    {  //Weapons buff
        if (_GameManager.WeaponData.Weapon[weaponIndex].BuffData.BuffType == BuffType.AttackSpeed)
        {
            InGameAttackSpeed = baseAttackSpeed + (baseAttackSpeed * _GameManager.WeaponData.Weapon[weaponIndex].BuffData.BuffIndex / 100);
        }
        else if (_GameManager.WeaponData.Weapon[weaponIndex].BuffData.BuffType == BuffType.Range)
        {
            inGameAttackRange = baseAttackRange + (baseAttackRange * _GameManager.WeaponData.Weapon[weaponIndex].BuffData.BuffIndex / 100);
        }
        //Acessories buff
        int equippedIndexHatsData = GetAccessorisEquippedIndex(_GameManager.HatsData);
        int equippedIndexPantsData = GetAccessorisEquippedIndex(_GameManager.PantsData);
        int equippedIndexShieldData = GetAccessorisEquippedIndex(_GameManager.ShieldData);
        int equippedIndexSetfullData = GetAccessorisEquippedIndex(_GameManager.SetfullData);
        UpdateSkinBuffData(equippedIndexHatsData, _GameManager.HatsData);
        UpdateSkinBuffData(equippedIndexPantsData, _GameManager.PantsData);
        UpdateSkinBuffData(equippedIndexShieldData, _GameManager.ShieldData);
        UpdateSkinBuffData(equippedIndexSetfullData, _GameManager.SetfullData);
    }
    private void UpdateSkinBuffData(int index, AccessoriesData accessoriesData)
    {
        if (index != 99)
        {
            if (accessoriesData.Accessories[index].BuffData.BuffType == BuffType.AttackSpeed)
            {
                InGameAttackSpeed = baseAttackSpeed + (baseAttackSpeed * accessoriesData.Accessories[index].BuffData.BuffIndex / 100);
            }
            else if (accessoriesData.Accessories[index].BuffData.BuffType == BuffType.MoveSpeed)
            {
                InGameMoveSpeed = baseMoveSpeed + (baseMoveSpeed * accessoriesData.Accessories[index].BuffData.BuffIndex / 100);
            }
            else if (accessoriesData.Accessories[index].BuffData.BuffType == BuffType.Range)
            {
                InGameAttackRange = baseAttackRange + (baseAttackRange * accessoriesData.Accessories[index].BuffData.BuffIndex / 100);
            }
            else if (accessoriesData.Accessories[index].BuffData.BuffType == BuffType.Gold)
            {
                InGameGoldEarn = baseGoldEarn + (baseGoldEarn * accessoriesData.Accessories[index].BuffData.BuffIndex / 100);
            }

        }
    }
    public int GetAccessorisEquippedIndex(AccessoriesData accessoriesData)
    {
        int index = 99;
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            if (accessoriesData.Accessories[i].Equipped)
            {
                index = i;
                break;
            }
        }
        if (index == 99)
        {
            //You don't have any Skin
            //Debug.Log("You don't have any Skin");
        }
        return index;
    }
}
