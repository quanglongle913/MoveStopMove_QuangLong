using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Rigidbody))]
public class Character : GameUnit,IHit
{
    [Header("Character: ")]
    [SerializeField] private SkinnedMeshRenderer mesh;
    [SerializeField] private SkinnedMeshRenderer pantsSkin;
    [SerializeField] private ColorData colorData;
    [SerializeField] private ColorType colorType;
    [SerializeField] private List<Weapons> listWeaponsInHand;
    [Header("---------------BASE------------ ")]
    [SerializeField] private int inGameExp = 100;
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
    //[SerializeField] private int inGamneZoneExp = 0;

    [SerializeField] private bool isTargerInRange;
    [SerializeField] private bool isAttacking;
    [Header("--------------------------- ")]
    [SerializeField] private GameObject weaponRoot;
    [Header("-------------Weapon-------------- ")]
    private WeaponType weaponType;
    [Header("-------------Skin-------------- ")]
    [SerializeField] private List<GameObject> listHats;
    [SerializeField] private List<GameObject> listSheilds;
    [SerializeField] private List<GameObject> listSetFull;
    [SerializeField] private GameObject Pants;

    [Header("--------------------------- ")]
    [SerializeField] private string characterName;
    [SerializeField] private int characterLevel;

    public Indicator Indicator;
    public CharacterInfo CharacterInfo;
    private float hp;
    private bool isBuffed;
    public bool IsDeath => hp <= 0;

    private Animator anim;
    private Rigidbody rb;
    protected float rotationSpeed = 1000f;
    private string currentAnimName;
    private GameObject target;
    protected Collider[] CurrentCharacters;
    protected Collider[] CharactersInsideZone;
    protected Collider[] CharactersOutsideZone;


    public bool IsHaveWeapon;

    private int weaponIndex;
    public Rigidbody _Rigidbody { get => rb; set => rb = value; }
    public ColorData ColorData { get => colorData; set => colorData = value; }
    //public ColorType ColorType { get => colorType; set => colorType = value; }

    public int InGamneExp { get => inGameExp; set => inGameExp = value; }
    public float InGameSizeCharacter { get => inGameSizeCharacter; set => inGameSizeCharacter = value; }
    public float InGameAttackRange { get => inGameAttackRange; set => inGameAttackRange = value; }
    public float InGameAttackSpeed { get => inGameAttackSpeed; set => inGameAttackSpeed = value; }
    public float InGameMoveSpeed { get => inGameMoveSpeed; set => inGameMoveSpeed = value; }
    public float InGameGold { get => inGameGold; set => inGameGold = value; }

    public bool IsTargerInRange { get => isTargerInRange; set => isTargerInRange = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public GameObject Target { get => target; set => target = value; }
    public GameObject WeaponRoot { get => weaponRoot; set => weaponRoot = value; }
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public string CharacterName { get => characterName; set => characterName = value; }
    public int WeaponIndex { get => weaponIndex; set => weaponIndex = value; }
    public SkinnedMeshRenderer PantsSkin { get => pantsSkin; set => pantsSkin = value; }

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
        OnInit();
    }
    public override void OnInit()
    {
        OnReset();
        ChangeColor(gameObject, colorType);
    }
    
    public virtual void FixedUpdate()
    {
        GenerateZone();
        DetectionCharacter();
    }
    
    public virtual void Attack() 
    {
        if (InCamera(GameManager.Instance.GetCamera()))
        {
            GameManager.Instance.SoundManager().PlayWeaponThrowSoundEffect();
        }
        ChangeAnim("Attack");
        IsAttacking = true;
        HideAllWeaponsInHand();
        Vector3 _DirectionCharacter = new Vector3(Target.transform.position.x - gameObject.transform.position.x, _Rigidbody.velocity.y, Target.transform.position.z - gameObject.transform.position.z).normalized;
        Vector3 _DirectionWeapon = new Vector3(Target.transform.position.x - WeaponRoot.transform.position.x, _Rigidbody.velocity.y, Target.transform.position.z - WeaponRoot.transform.position.z).normalized;
        RotateTowards(this.gameObject, _DirectionCharacter);
        
        GenerateWeapon(_DirectionWeapon);
        if (GameManager.Instance.IsMode(GameMode.Survival))
        {
            for (int i = 0; i < GameManager.Instance.Player().Bullets; i++)
            {
                if (i % 2 == 0)
                {
                    GenerateWeapon(transform.TransformDirection(0.5f + i*0.5f, 0, 1).normalized);
                }
                else {
                    GenerateWeapon(transform.TransformDirection(0.5f - i*1.0f, 0, 1).normalized);
                }
                
                
            }
            
        }
    }
    public void AnimalAttack()
    {
        
        Vector3 _DirectionCharacter = new Vector3(Target.transform.position.x - gameObject.transform.position.x, _Rigidbody.velocity.y, Target.transform.position.z - gameObject.transform.position.z).normalized;
        RotateTowards(this.gameObject, _DirectionCharacter);
        ChangeAnim("Attack");
        //IsAttacking = true;
        
        //Invoke(nameof(AnimalAtackWaiter),1.0f);
    }
    public void AnimalAtackWaiter()
    {
        IsAttacking = false;
    }
    private void GenerateWeapon(Vector3 _Direction)
    {
        Weapons weaponAttack2 = SimplePool.Spawn<Weapons>((PoolType)(weaponType+2));
        weaponAttack2.gameObject.transform.localScale = GetWeaponInHand((int)WeaponType).transform.lossyScale;
        Vector3 newTarget = new Vector3(Target.transform.position.x, weaponAttack2.transform.position.y, Target.transform.position.z);
        weaponAttack2.transform.position = WeaponRoot.transform.position;
        weaponAttack2._GameObject = gameObject;
        weaponAttack2.character = Constant.Cache.GetCharacter(gameObject);
        weaponAttack2.WeaponType = this.WeaponType;
        weaponAttack2.direction = _Direction;
        weaponAttack2.gameObject.SetActive(true);
        weaponAttack2.startPoint = gameObject.transform.position;
        weaponAttack2.target = newTarget;
        weaponAttack2.bulletSpeed = InGameAttackSpeed / 10.0f;
        weaponAttack2.isFire = true;
    }
    protected void GenerateZone()
    {
        CurrentCharacters = Physics.OverlapSphere(this.transform.position, 1000f, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersInsideZone = Physics.OverlapSphere(this.transform.position, InGameAttackRange, LayerMask.GetMask(Constant.LAYOUT_CHARACTER));
        CharactersOutsideZone = CurrentCharacters.Except(CharactersInsideZone).ToArray();
    }
    public bool DetectionCharacter()
    {
        //GenerateZone();
        Collider[] colliders = CharactersInsideZone;
        for (int i = 0; i < colliders.Length; i++)
        {
            //Character character = colliders[i].GetComponent<Character>();
            Character character = Constant.Cache.GetCharacter(colliders[i]);
            if (character)
            {
                if (!character.IsDeath && colliders[i].gameObject != this.gameObject && colorType != character.colorType)
                {
                    if (Constant.IsDes(gameObject.transform.position, character.gameObject.transform.position, InGameAttackRange))
                    {
                        IsTargerInRange = true;
                        Target = character.gameObject;
                        break;
                    }
                    else
                    {
                        IsTargerInRange = false;
                    }

                }
                else
                {
                    IsTargerInRange = false;
                }
            }
        }
        return IsTargerInRange;
    }
    public bool DetectionPlayer()
    {
        //GenerateZone();
        Collider[] colliders = CharactersInsideZone;
        for (int i = 0; i < colliders.Length; i++)
        {
            Player player = Constant.Cache.GetPlayer(colliders[i]);
            if (player)
            {
                if (!player.IsDeath)
                {
                    if (Constant.IsDes(gameObject.transform.position, player.gameObject.transform.position, InGameAttackRange))
                    {
                        IsTargerInRange = true;
                        Target = player.gameObject;
                        break;
                    }
                    else
                    {
                        IsTargerInRange = false;
                    }
                }
                else
                {
                    IsTargerInRange = false;
                }
            }
        }
        return IsTargerInRange;
    }
    public bool InCamera(Camera camera)
    {
        bool check=false;
        Vector3 viewPos = camera.WorldToViewportPoint(gameObject.transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && (viewPos.z > 0))
        {
            // Your object is in the range of the camera, you can apply your behaviour(.)
            check =true;
        }
        return check;
    }
    public override void OnDespawn()
    {

    }
    protected virtual void OnDeath()
    {
        if (InCamera(GameManager.Instance.GetCamera()))
        {
            GameManager.Instance.SoundManager().PlayDeadSoundEffect();
        }
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
    public ColorType GetColorType() 
    { 
        return colorType; 
    }
    /*public void SetColorType(ColorType colorType)
    {
       this.colorType = colorType;
    }*/
    public void ChangeColor(GameObject a_obj, ColorType colorType)
    {
        this.colorType = colorType;
        Constant.Cache.GetCharacter(a_obj).mesh.material = colorData.GetMat(colorType);
    }

    public virtual void OnHit(float damage)
    {

        if (!IsDeath)
        {
            if (InCamera(GameManager.Instance.GetCamera()))
            {
                GameManager.Instance.SoundManager().PlayWeaponHitSoundEffect();
            }
            hp -= damage;
            if (IsDeath)
            {
                hp = 0;
                OnDeath();
            }
        }

    }
    //=====================Weapons===================
    public GameObject GetWeaponInHand(int index)
    {
        return listWeaponsInHand[index].gameObject;
    }
    //Set Material for Prefabs 
    public void SetWeaponSkinMat()
    {
        Renderer renderer = GetWeaponInHand((int)WeaponType).GetComponent<Renderer>();
        var newMaterials = new Material[renderer.materials.Count()];

        for (int i = 0; i < newMaterials.Count(); i++)
        {
            newMaterials[i] = GameManager.Instance.GetWeaponData().Weapon[weaponIndex].Mat;

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
        for (int i = 0; i < listWeaponsInHand.Count; i++)
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
    public int GetLevel()
    {
        return characterLevel;
    }
    public void SetLevel(int characterLevel)
    {
         this.characterLevel= characterLevel;
    }
    public void LevelUp()
    {
        this.characterLevel++;
    }
    public void SetExp(int exp)
    {
        InGamneExp += exp / characterLevel + 40;
        InGameGold += InGameGoldEarn;
        UpdateCharacterLvl();
    }
    private void OnReset()
    {
        IsAttacking = false;
        IsTargerInRange = false;
        hp = 1;
        WeaponIndex = 0;
        //SetInGameExp(100);
        inGameExp = 100;
        GetWeaponInHand(WeaponIndex).SetActive(true);
        InGameGold = 0;
        
        inGameSizeCharacter = baseSizeCharacter;
        inGameAttackRange = baseAttackRange;
        inGameAttackSpeed = baseAttackSpeed;
        inGameMoveSpeed = baseMoveSpeed;
        inGameGoldEarn = baseGoldEarn;
        UpdateCharacterLvl();
    }
    public void OnResetAnimal()
    {
        IsAttacking = false;
        IsTargerInRange = false;
        hp = 1;
        InGameGold = 0;
        InGamneExp = 10;
        inGameSizeCharacter = baseSizeCharacter;
        inGameAttackRange = baseAttackRange;
        inGameAttackSpeed = baseAttackSpeed;
        inGameMoveSpeed = baseMoveSpeed;
        inGameGoldEarn = baseGoldEarn;
    }
    public void UpdateCharacterLvl()
    {
        characterLevel = InGamneExp / 100; //tinh level character
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
        WeaponData weaponData = GameManager.Instance.GetWeaponData();
        if (weaponData.Weapon[weaponIndex].BuffData.BuffType == BuffType.AttackSpeed)
        {
            InGameAttackSpeed = baseAttackSpeed + (baseAttackSpeed * weaponData.Weapon[weaponIndex].BuffData.BuffIndex / 100);
        }
        else if (weaponData.Weapon[weaponIndex].BuffData.BuffType == BuffType.Range)
        {
            inGameAttackRange = baseAttackRange + (baseAttackRange * weaponData.Weapon[weaponIndex].BuffData.BuffIndex / 100);
        }
        //Acessories buff
        int equippedIndexHatsData = GetAccessorisEquippedIndex(GameManager.Instance.GetAccessoriesDatas()[0]);
        int equippedIndexPantsData = GetAccessorisEquippedIndex(GameManager.Instance.GetAccessoriesDatas()[1]);
        int equippedIndexShieldData = GetAccessorisEquippedIndex(GameManager.Instance.GetAccessoriesDatas()[2]);
        int equippedIndexSetfullData = GetAccessorisEquippedIndex(GameManager.Instance.GetAccessoriesDatas()[3]);
        UpdateSkinBuffData(equippedIndexHatsData, GameManager.Instance.GetAccessoriesDatas()[0]);
        UpdateSkinBuffData(equippedIndexPantsData, GameManager.Instance.GetAccessoriesDatas()[1]);
        UpdateSkinBuffData(equippedIndexShieldData, GameManager.Instance.GetAccessoriesDatas()[2]);
        UpdateSkinBuffData(equippedIndexSetfullData, GameManager.Instance.GetAccessoriesDatas()[3]);
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
        for (int i = 0; i < accessoriesData.Accessories.Count; i++)
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
    public void SetHp(float hp)
    { 
        this.hp=hp; ;
    }
    public float Hp()
    {
        return hp;
    }
    public void SetInGameExp(int exp)
    {
        this.inGameExp = exp; ;
    }
    public void CharacterBufffCountDown(int randomBuff, List<BuffData> buffDataInGiftBox)
    {
        if (!IsBuffed)
        {
            if (InCamera(GameManager.Instance.GetCamera()))
            {
                GameManager.Instance.SoundManager().PlaySizeUpSoundEffect();
            }
            if (buffDataInGiftBox[randomBuff].BuffType == BuffType.AttackSpeed)
            {
                StartCoroutine(Waiter(InGameAttackSpeed, buffDataInGiftBox[randomBuff]));
                InGameAttackSpeed = InGameAttackSpeed + (InGameAttackSpeed * buffDataInGiftBox[randomBuff].BuffIndex / 100);
            }
            if (buffDataInGiftBox[randomBuff].BuffType == BuffType.MoveSpeed)
            {
                StartCoroutine(Waiter(InGameMoveSpeed, buffDataInGiftBox[randomBuff]));
                InGameMoveSpeed = InGameMoveSpeed + (InGameMoveSpeed * buffDataInGiftBox[randomBuff].BuffIndex / 100);
            }
            if (buffDataInGiftBox[randomBuff].BuffType == BuffType.Range)
            {
                StartCoroutine(Waiter(InGameAttackRange, buffDataInGiftBox[randomBuff]));
                InGameAttackRange = InGameAttackRange + (InGameAttackRange * buffDataInGiftBox[randomBuff].BuffIndex / 100);
            }
        }

    }
    IEnumerator Waiter(float indexType, BuffData buffData)
    {
        int bufftype = (int)buffData.BuffType;
        bufftype += (int)ParticleType.AuraBlue;
        float backUp = indexType;
        ParticleSystem newBuffVfx = Instantiate(ParticlePool.ParticleSystem((ParticleType) bufftype), gameObject.transform.position, gameObject.transform.rotation);
        newBuffVfx.transform.parent = gameObject.transform;
        newBuffVfx.Play();
        IsBuffed = true;
        yield return new WaitForSeconds(3f);
        if (buffData.BuffType == BuffType.AttackSpeed)
        {
            InGameAttackSpeed = backUp;
            Destroy(newBuffVfx.gameObject);

        }
        if (buffData.BuffType == BuffType.MoveSpeed)
        {
            InGameMoveSpeed = backUp;
            Destroy(newBuffVfx.gameObject);
        }
        if (buffData.BuffType == BuffType.Range)
        {
            InGameAttackRange = backUp;
            Destroy(newBuffVfx.gameObject);
        }
        IsBuffed = false;
    }
}
