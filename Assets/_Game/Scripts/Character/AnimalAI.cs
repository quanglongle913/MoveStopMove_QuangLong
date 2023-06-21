using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AnimalAI : MonoBehaviour
{
    [Header("------------AnimalAI--------------- ")]
    [SerializeField] private GameObject circleAttack;
    [SerializeField] private GameObject attackBox;
    [SerializeField] private GameObject animal;
    [SerializeField] private IState<AnimalAI> currentState;
    [SerializeField] private int inGamneExp = 1;
    private NavMeshAgent agent;
    public float hp; 
    private bool isBuffed;
    public bool IsDeath => hp <= 0;

    public GameManager _GameManager { get => gameManager; set => gameManager = value; }
    public GameObject CircleAttack { get => circleAttack; set => circleAttack = value; }
    public GameObject Target { get => target; set => target = value; }
    public GameObject Animal { get => animal; set => animal = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }
    public int InGamneExp { get => inGamneExp; set => inGamneExp = value; }

    private GameManager gameManager;

    private Animator anim;
    private Rigidbody rb;
    protected float rotationSpeed = 1000f;
    private string currentAnimName;
    public GameObject target;
    public bool IsTargerInRange=false;
    public Vector3 Direction;
    public virtual void Awake()
    {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _GameManager = GameManager.Instance;
        OnInit();
    }
    public void OnInit()
    { 
        hp = 1;
        if (CircleAttack.activeSelf)
        {
            CircleAttack.SetActive(false);
        }
        ChangeState(new IdleStateAnimal());
    }
    // Update is called once per frame
    void Update()
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
            if (_GameManager.GameState == GameState.InGame)
            {
                DetectionPlayer(Physics.OverlapSphere(this.transform.position, 1000f, LayerMask.GetMask(Constant.LAYOUT_CHARACTER)));
                Direction = new Vector3(_GameManager.Player.gameObject.transform.position.x - gameObject.transform.position.x,rb.velocity.y, _GameManager.Player.gameObject.transform.position.z - gameObject.transform.position.z);
                if (currentState != null)
                {
                    currentState.OnExecute(this);
                }
            }
            else
            {
                ChangeState(new IdleStateAnimal());
            }
        }

    }
    public void Attack()
    {
        //ChangeState(new AttackStateAnimal());
        ChangeAnim("Attack");
    }
    public void Run(Transform transform)
    {
        if (_GameManager.GameState == GameState.InGame)
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(transform.position);
            }
            
        }
            
    }
    public void Idle(bool check)
    {
        if (this.gameObject.activeSelf)
        {
            agent.isStopped = check;
        }
    }
    private void DetectionPlayer(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Player>())
            {
                IsTargerInRange = true;
                target = colliders[i].gameObject;
                break;
            }
            else
            {
                IsTargerInRange = false;
            }
        }
    }
    public void ChangeState(IState<AnimalAI> state)
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
    public  void OnDespawn()
    {
        //OnReset();
        agent.ResetPath();
        ChangeState(new IdleStateAnimal());
        gameObject.GetComponent<PooledObject>().Release();
        OnInit();

    }
    protected  void OnDeath()
    {
        if (_GameManager.AnimalAIListEnable.Count > 0)
        {
            _GameManager.AnimalAIListEnable.Remove(this.gameObject.GetComponent<AnimalAI>());
            ChangeState(new DeadStateAnimal());
        }
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
    
    public virtual void OnHit(float damage)
    {

        if (!IsDeath)
        {
           /* if (InCamera(_GameManager.MainCam))
            {
                _GameManager.SoundManager.PlayWeaponHitSoundEffect();
            }*/

            hp -= damage;
            if (IsDeath)
            {
                hp = 0;
                OnDeath();
            }
        }

    }
}
