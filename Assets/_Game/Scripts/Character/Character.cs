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
    [SerializeField] private SkinnedMeshRenderer paintSkin;
    [SerializeField] private ColorData colorData;
    [SerializeField] private ColorType colorType;
    [SerializeField] private List<Weapons> listWeaponsInHand;
    [Header("--------------------------- ")]
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private bool isTargerInRange;
    [SerializeField] private bool isAttacking;
    [Header("--------------------------- ")]
    [SerializeField] private GameObject weaponMaster;
    [SerializeField] private ObjectPool poolObject;
    [Header("-------------Weapon-------------- ")]
    private WeaponType weaponType;
    private WeaponData weaponData;
    [SerializeField] private float attackSpeedAfterbuff;
    [Header("--------------------------- ")]
    [SerializeField] private string characterName;
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
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public ColorData ColorData { get => colorData; set => colorData = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public bool IsTargerInRange { get => isTargerInRange; set => isTargerInRange = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public GameManager _GameManager { get => gameManager; set => gameManager = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public GameObject Target { get => target; set => target = value; }
    public List<Weapons> Weapons { get => weapons; set => weapons = value; }
    public ObjectPool PoolObject { get => poolObject; set => poolObject = value; }
    public GameObject WeaponMaster { get => weaponMaster; set => weaponMaster = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public List<Weapons> ListWeaponsInHand { get => listWeaponsInHand; set => listWeaponsInHand = value; }
    public float AttackSpeedAfterbuff { get => attackSpeedAfterbuff; set => attackSpeedAfterbuff = value; }
    public WeaponData WeaponData { get => weaponData; set => weaponData = value; }
    public string CharacterName { get => characterName; set => characterName = value; }
    public int WeaponIndex { get => weaponIndex; set => weaponIndex = value; }
    public SkinnedMeshRenderer PaintSkin { get => paintSkin; set => paintSkin = value; }

    //public List<Weapons> ListWeaponsAttack { get => listWeaponsAttack; set => listWeaponsAttack = value; }

    // Start is called before the first frame update
    public virtual void Awake()
    {
        
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        this.WeaponIndex = PlayerPrefs.GetInt(Constant.WEAPONS, 0);
    }
    public virtual void Start()
    {
        _GameManager = GameManager.Instance;
        Weapons = new List<Weapons>();
        //Init Weapons....................
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
        AttackSpeedAfterbuff = AttackSpeed + (AttackSpeed * _GameManager.WeaponData.Weapon[weaponIndex].AttackSpeed / 100);

        //Debug.Log("index:"+weaponIndex);
        //Debug.Log("Type:"+ WeaponData.Weapon[weaponIndex].WeaponType);
        //Set Material for Prefabs  when null Material
        /*var newMaterials = new Material[ListWeaponsInHand[(int)WeaponType].gameObject.GetComponent<Renderer>().materials.Count()];

        for (int i = 0; i < newMaterials.Count(); i++)
        {
            newMaterials[i] = WeaponData.Weapon[weaponIndex].Mat;

        }
        ListWeaponsInHand[(int)WeaponType].gameObject.GetComponent<Renderer>().materials = newMaterials;
        */
        setWeaponSkinMat(ListWeaponsInHand[(int)WeaponType].gameObject.GetComponent<Renderer>(), this.WeaponData, this.WeaponIndex);

        //endset 
        ListWeaponsInHand[(int)WeaponType].gameObject.SetActive(true);
        PoolObject = _GameManager.PoolObject[(int)WeaponType];
        PoolObject.GetComponent<ObjectPool>().ObjectToPool.gameObject.GetComponent<Renderer>().material= WeaponData.Weapon[weaponIndex].Mat;
        //Debug.Log("OK");
    }
    //TEST
    public void setWeaponSkinMat(Renderer renderer, WeaponData weaponData, int index) 
    {
        //Set Material for Prefabs  when null Material
        var newMaterials = new Material[renderer.materials.Count()];

        for (int i = 0; i < newMaterials.Count(); i++)
        {
            newMaterials[i] = weaponData.Weapon[index].Mat;

        }
        renderer.materials = newMaterials;
    }
    public void setAccessorisSkinMat(Renderer renderer, AccessoriesData accessoriesData, int index)
    {
        //Set Material for Prefabs  when null Material
        var newMaterials = new Material[renderer.materials.Count()];

        for (int i = 0; i < newMaterials.Count(); i++)
        {
            newMaterials[i] = accessoriesData.Accessories[index].Mat;

        }
        renderer.materials = newMaterials;
    }
    //ENDTEST
    public virtual void FixedUpdate()
    {
        GenerateZone();
        DetectionCharacter(CharactersInsideZone);
        if (this._GameManager.GameState == GameState.InGame)
        {
            if (Weapons.Count <= 1)
            {
                Weapons a_weapon = gameObject.GetComponent<WeaponSpawner>().GenerateWeapon(WeaponMaster, PoolObject);
                a_weapon.gameObject.transform.localScale = ListWeaponsInHand[(int)WeaponType].gameObject.transform.localScale;
                Weapons.Add(a_weapon);
            }
        }
    }
    public virtual void Attack() 
    {
        ChangeAnim("Attack");
        IsAttacking = true;
        ListWeaponsInHand[(int)WeaponType].gameObject.SetActive(false);
        Weapons weaponAttack = Weapons[0];
        weaponAttack._GameObject = gameObject;
        Vector3 newTarget = new Vector3(Target.transform.position.x, weaponAttack.transform.position.y, Target.transform.position.z);
        Vector3 _Direction = new Vector3(newTarget.x - WeaponMaster.transform.position.x, _Rigidbody.velocity.y, newTarget.z - WeaponMaster.transform.position.z);
        RotateTowards(this.gameObject, _Direction);
        weaponAttack.isFire = true;
        weaponAttack.gameObject.SetActive(true);
        weaponAttack.transform.DOMove(newTarget, (float)Math.Round(60 / AttackSpeedAfterbuff, 1))
                    .SetEase(Ease.InSine)
                    .SetLoops(0, LoopType.Restart)
                    .OnComplete(() =>
                    {
                        Weapons.Remove(weaponAttack);
                        weaponAttack.gameObject.SetActive(false);
                        weaponAttack.gameObject.GetComponent<PooledObject>().Release();
                        weaponAttack.isFire = false;
                        ListWeaponsInHand[(int)WeaponType].gameObject.SetActive(true);
                        IsAttacking = false;
                    });
    }
    private void GenerateZone()
    {
        CurrentCharacters = Physics.OverlapSphere(this.transform.position, 1000f, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        //Debug.Log(CurrentCharacters.Length);
        CharactersInsideZone = Physics.OverlapSphere(this.transform.position, AttackRange, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersOutsideZone = CurrentCharacters.Except(CharactersInsideZone).ToArray();
    }
    private void DetectionCharacter(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++) 
        {
            if (!colliders[i].GetComponent<Character>().IsDeath && colliders[i].gameObject != this.gameObject)
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
        Gizmos.DrawWireSphere(this.transform.position, AttackRange);

    }
    public void RotateTowards(GameObject gameObject, Vector3 direction)
    {
        //transform.rotation = Quaternion.LookRotation(direction);
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
        //Debug.Log(gameObject.name+":"+colorType.ToString());
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
}
