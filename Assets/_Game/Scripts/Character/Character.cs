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
    [Header("--------------INGANE------------- ")]
    [SerializeField] private float inGamneSizeCharacter = 1.0f;
    [SerializeField] private float inGamneAttackRange = 7.0f;
    [SerializeField] private float inGamneAttackSpeed = 60f;
    [SerializeField] private float inGameMoveSpeed = 5.0f;
    [SerializeField] private float inGamneGold = 50f;

    [SerializeField] private bool isTargerInRange;
    [SerializeField] private bool isAttacking;
    [Header("--------------------------- ")]
    [SerializeField] private GameObject weaponRoot;
    [SerializeField] private ObjectPool poolObject;
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
    public bool IsDeath => hp <= 0;

    private Animator anim;
    private Rigidbody rb;
    protected float rotationSpeed = 1000f;
    private string currentAnimName;
    private GameObject target;
    protected Collider[] CurrentCharacters;
    protected Collider[] CharactersInsideZone;
    protected Collider[] CharactersOutsideZone;
    
    private GameManager gameManager;

    public bool IsHaveWeapon;

    private List<Weapons> weapons;
    private int weaponIndex;
    public Rigidbody _Rigidbody { get => rb; set => rb = value; }
    public ColorData ColorData { get => colorData; set => colorData = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }

    public int InGamneExp { get => inGamneExp; set => inGamneExp = value; }
    public float InGamneSizeCharacter { get => inGamneSizeCharacter; set => inGamneSizeCharacter = value; }
    public float InGamneAttackRange { get => inGamneAttackRange; set => inGamneAttackRange = value; }
    public float InGamneAttackSpeed { get => inGamneAttackSpeed; set => inGamneAttackSpeed = value; }
    public float InGameMoveSpeed { get => inGameMoveSpeed; set => inGameMoveSpeed = value; }
    public float InGamneGold { get => inGamneGold; set => inGamneGold = value; }

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
        ChangeColor(gameObject, ColorType);
        //Get Weapon info buff.... etc
        this.WeaponData = _GameManager.WeaponData;
        this.WeaponType = WeaponData.Weapon[weaponIndex].WeaponType;
        setWeaponSkinMat(ListWeaponsInHand[(int)WeaponType].gameObject.GetComponent<Renderer>(), this.WeaponData, this.WeaponIndex);
        //ListWeaponsInHand[(int)WeaponType].gameObject.SetActive(true);
        ShowWeaponIndex((int)WeaponType);
        PoolObject = _GameManager.PoolObject[(int)WeaponType];
        PoolObject.GetComponent<ObjectPool>().ObjectToPool.gameObject.GetComponent<Renderer>().material= WeaponData.Weapon[weaponIndex].Mat;
        updateCharacter();
    }
    //Set Material for Prefabs 
    public void setWeaponSkinMat(Renderer renderer, WeaponData weaponData, int index) 
    {
        var newMaterials = new Material[renderer.materials.Count()];

        for (int i = 0; i < newMaterials.Count(); i++)
        {
            newMaterials[i] = weaponData.Weapon[index].Mat;

        }
        renderer.materials = newMaterials;
    }

    public virtual void FixedUpdate()
    {
        GenerateZone();
        DetectionCharacter(CharactersInsideZone);
        if (this._GameManager.GameState == GameState.InGame)
        {
            if (Weapons.Count <= 1)
            {
                Weapons a_weapon = gameObject.GetComponent<WeaponSpawner>().GenerateWeapon(gameManager.WeaponManager, PoolObject);
                a_weapon.gameObject.transform.localScale = ListWeaponsInHand[(int)WeaponType].gameObject.transform.localScale;
                Weapons.Add(a_weapon);
            }
        }
    }
    public virtual void Attack() 
    {
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
        _weapons.transform.DOMove(_weapons.target, (float)Math.Round(60 / _character.InGamneAttackSpeed, 1))
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
    private void GenerateZone()
    {
        CurrentCharacters = Physics.OverlapSphere(this.transform.position, 1000f, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersInsideZone = Physics.OverlapSphere(this.transform.position, InGamneAttackRange, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersOutsideZone = CurrentCharacters.Except(CharactersInsideZone).ToArray();
    }
    private void DetectionCharacter(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++) 
        {

            if (!colliders[i].GetComponent<Character>().IsDeath && colliders[i].gameObject != this.gameObject && colorType != colliders[i].GetComponent<Character>().colorType)
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
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, InGamneAttackRange);

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

    public void OnHit(float damage)
    {

        if (!IsDeath)
        {
            hp -= damage;

            if (IsDeath)
            {
                hp = 0;
                OnDeath();
            }
        }

    }
   //=====================Weapons===================
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
        listHats[index].gameObject.SetActive(false);
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
        listSheilds[index].gameObject.SetActive(false);
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
        listSetFull[index].gameObject.SetActive(false);
    }
    public void HidePantsSkin()
    {
        Pants.SetActive(false);
    }
    public void ActivePantsSkin()
    {
        if (Pants != null)
        {
            HidePantsSkin();
            Pants.SetActive(true);
        }
        else
        {
            Debug.LogError("Pants is Null");
        }
    }

    //Set EXP when killed other character
    public void setExp(int exp)
    {
        InGamneExp += exp / CharacterLevel + 40;
        InGamneGold += 50;
        updateCharacter();
    }
    public void updateCharacter()
    {
        CharacterLevel = InGamneExp / 100; //tinh level character
        float offsetSize = 0.02f;
        float offsetAttackSpeed = 0.04f;
        float offsetMoveSpeed = 0.3f;

        if (characterLevel == 1)
        {
            inGamneSizeCharacter = baseSizeCharacter;
            inGamneAttackRange = baseAttackRange;
            inGamneAttackSpeed = baseAttackSpeed;
            inGameMoveSpeed = baseMoveSpeed;
        }
        else if (characterLevel <= 10)
        {
            inGamneSizeCharacter = baseSizeCharacter + offsetSize * characterLevel;
            //inGamneAttackRange = baseAttackRange + offset * characterLevel;
            inGamneAttackSpeed = baseAttackSpeed + offsetAttackSpeed * characterLevel;
            inGameMoveSpeed = baseMoveSpeed + offsetMoveSpeed * characterLevel;
        }
        else
        {
            inGamneSizeCharacter = baseSizeCharacter + offsetSize * 10;
            //inGamneAttackRange = baseAttackRange + offset * 10;
            inGamneAttackSpeed = baseAttackSpeed + offsetAttackSpeed * 10;
            inGameMoveSpeed = baseMoveSpeed + offsetMoveSpeed * 10;
        }
        //Debug.Log(""+ _GameManager.WeaponData.Weapon[weaponIndex].BuffData.BuffType);
        if (_GameManager.WeaponData.Weapon[weaponIndex].BuffData.BuffType == BuffType.AttackSpeed)
        {
            InGamneAttackSpeed = InGamneAttackSpeed + (InGamneAttackSpeed * _GameManager.WeaponData.Weapon[weaponIndex].BuffData.BuffIndex / 100);
        } else if (_GameManager.WeaponData.Weapon[weaponIndex].BuffData.BuffType == BuffType.Range)
        {
            inGamneAttackRange = InGamneAttackRange + (InGamneAttackRange * _GameManager.WeaponData.Weapon[weaponIndex].BuffData.BuffIndex / 100);
        }
        //AttackSpeedAfterbuff = InGamneAttackSpeed;
        transform.localScale = new Vector3(inGamneSizeCharacter, inGamneSizeCharacter, inGamneSizeCharacter);
    }
}
