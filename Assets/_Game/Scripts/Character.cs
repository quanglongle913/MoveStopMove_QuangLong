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



    protected float rotationSpeed = 1000f;
    private string currentAnimName;

    protected Collider[] CurrentCubes;
    protected Collider[] cubesInsideZone;
    protected Collider[] cubesOutsideZone;
    
    public ColorData ColorData { get => colorData; set => colorData = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public virtual void FixedUpdate()
    {
        CurrentCubes = Physics.OverlapSphere(this.transform.position, 1000f, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        cubesInsideZone = Physics.OverlapSphere(this.transform.position, AttackRange, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        cubesOutsideZone = CurrentCubes.Except(cubesInsideZone).ToArray();
    }

    private void DetectionCharacter(Collider[] colliders)
    {
        foreach (Collider hitcollider in colliders)
        {
            if (hitcollider.GetComponent<BotAI>() && hitcollider.gameObject != this.gameObject)
            {
                Debug.Log(hitcollider.gameObject.name);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, AttackRange);

    }
    public virtual void OnInit()
    { 
    
    }
    protected void RotateTowards(GameObject gameObject, Vector3 direction)
    {
        // transform.rotation = Quaternion.LookRotation(direction);
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    protected void ChangeAnim(string animName)
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
    protected bool isWall(LayerMask _layerMask)
    {
        RaycastHit hit;
        bool isWall = false;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Constant.RAYCAST_HIT_RANGE_WALL, _layerMask))
        {
            isWall = true;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            //Debug.Log("Did Hit");
        }
        else
        {
            isWall = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
        return isWall;
    }
}
