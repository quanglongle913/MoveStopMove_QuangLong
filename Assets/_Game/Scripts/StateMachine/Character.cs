using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected SkinnedMeshRenderer mesh;
    [SerializeField] private ColorData colorData;
    [SerializeField] private ColorType colorType;
    [SerializeField] private float attackRange = 3f;


    [SerializeField] private bool isTargerInRange;
    [SerializeField] private bool isAttacking;
    protected float rotationSpeed = 1000f;
    private string currentAnimName;

    protected Collider[] CurrentCharacters;
    protected Collider[] CharactersInsideZone;
    protected Collider[] CharactersOutsideZone;
    
    private GameManager gameManager;
    [SerializeField] public GameObject WeaponMaster;
    [SerializeField] public GameObject Weapon;
    [SerializeField] public bool IsHaveWeapon;

    public ColorData ColorData { get => colorData; set => colorData = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public bool IsTargerInRange { get => isTargerInRange; set => isTargerInRange = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public GameManager GameManager { get => gameManager; set => gameManager = value; }

    // Start is called before the first frame update
    public virtual void Start()
    {
        gameManager = GameManager.Instance;
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
    }
    private void GenerateZone()
    {
        CurrentCharacters = Physics.OverlapSphere(this.transform.position, 1000f, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersInsideZone = Physics.OverlapSphere(this.transform.position, AttackRange, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersOutsideZone = CurrentCharacters.Except(CharactersInsideZone).ToArray();
    }
    private void DetectionCharacter(Collider[] colliders)
    {
        foreach (Collider hitcollider in colliders)
        {
            if (hitcollider.GetComponent<Character>() && hitcollider.gameObject != this.gameObject)
            {
                //Debug.Log(hitcollider.gameObject.name + " : Team: " + hitcollider.gameObject.GetComponent<Character>().ColorType);
                IsTargerInRange = true;
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
        gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
    /*protected bool isWall(LayerMask _layerMask)
    {
        RaycastHit hit;
        bool isWall = false;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Constant.RAYCAST_HIT_RANGE_WALL, _layerMask))
        {
            isWall = true;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
        }
        else
        {
            isWall = false;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
        return isWall;
    }*/
}
