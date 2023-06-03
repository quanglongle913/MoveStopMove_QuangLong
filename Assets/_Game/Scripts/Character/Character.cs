using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [Header("Character: ")]
    [SerializeField] protected SkinnedMeshRenderer mesh;
    [SerializeField] private ColorData colorData;
    [SerializeField] private ColorType colorType;
    //[SerializeField] private GameObject weaponMasterHand;
    [SerializeField] private List<Weapons> listWeaponsInHand;
    [Header("--------------------------- ")]
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] private float moveSpeed = 5.0f;
    [Header("--------------------------- ")]
    [SerializeField] private bool isTargerInRange;
    [SerializeField] private bool isAttacking;
    [Header("--------------------------- ")]
    [SerializeField] private GameObject weaponMaster;
    [SerializeField] private ObjectPool poolObject;
    //[SerializeField] private List<Weapons> listWeaponsAttack;
    [Header("--------------------------- ")]
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private float attackSpeedAfterbuff;
    [Header("--------------------------- ")]
    private Animator anim;
    private Rigidbody rb;
    protected float rotationSpeed = 1000f;
    private string currentAnimName;
    private GameObject target;
    protected Collider[] CurrentCharacters;
    protected Collider[] CharactersInsideZone;
    protected Collider[] CharactersOutsideZone;
    
    private GameManager gameManager;
    private WeaponMannager weaponMannager;

    public bool IsHaveWeapon;

    private List<Weapons> weapons;

    public Rigidbody _Rigidbody { get => rb; set => rb = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public ColorData ColorData { get => colorData; set => colorData = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public bool IsTargerInRange { get => isTargerInRange; set => isTargerInRange = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public GameManager GameManager { get => gameManager; set => gameManager = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public GameObject Target { get => target; set => target = value; }
    public List<Weapons> Weapons { get => weapons; set => weapons = value; }
    public ObjectPool PoolObject { get => poolObject; set => poolObject = value; }
    public GameObject WeaponMaster { get => weaponMaster; set => weaponMaster = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public List<Weapons> ListWeaponsInHand { get => listWeaponsInHand; set => listWeaponsInHand = value; }
    public WeaponMannager WeaponMannager { get => weaponMannager; set => weaponMannager = value; }
    public float AttackSpeedAfterbuff { get => attackSpeedAfterbuff; set => attackSpeedAfterbuff = value; }

    //public List<Weapons> ListWeaponsAttack { get => listWeaponsAttack; set => listWeaponsAttack = value; }

    // Start is called before the first frame update
    public virtual void Awake()
    {
        
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    public virtual void Start()
    {
        gameManager = GameManager.Instance;
        weaponMannager = WeaponMannager.Instance;
        Weapons = new List<Weapons>();
        //Init Weapons....................
        WeaponType = WeaponType.Arrow;
        ListWeaponsInHand[(int)WeaponType].gameObject.SetActive(true);
        poolObject = weaponMannager.PoolObject[(int)WeaponType];
        OnInit();
    }
    public virtual void OnInit()
    {
        IsAttacking = false;
        IsTargerInRange = false;
        //Debug.Log("OK");
    }
    public virtual void FixedUpdate()
    {
        GenerateZone();
        DetectionCharacter(CharactersInsideZone);
        if (this.GameManager.GameState == GameState.InGame)
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
        CharactersInsideZone = Physics.OverlapSphere(this.transform.position, AttackRange, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersOutsideZone = CurrentCharacters.Except(CharactersInsideZone).ToArray();
    }
    private void DetectionCharacter(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++) 
        {
            if (colliders[i].GetComponent<Character>() && colliders[i].gameObject != this.gameObject)
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
        this.colorType = colorType;
        a_obj.GetComponent<SkinnedMeshRenderer>().material = colorData.GetMat(colorType);
    }
}
