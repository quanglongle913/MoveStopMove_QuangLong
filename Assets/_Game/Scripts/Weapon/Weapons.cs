using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using DG.Tweening.Core.Easing;
using UnityEngine.TextCore.Text;

public class Weapons : GameUnit
{
    [SerializeField] private WeaponType weaponType;

    private GameObject _gameObject;
    private float rotationSpeed=800f;
    private float bulletSpeed = 2f;
    private bool isFire;
    private Vector3 target;
    private Vector3 startPoint;
    private Vector3 direction;
    private ParticleSystem VFX_Trail;
    private float rotY;
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public GameObject _GameObject { get => _gameObject; set => _gameObject = value; }
    public bool IsFire { get => isFire; set => isFire = value; }
    public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public Vector3 StartPoint { get => startPoint; set => startPoint = value; }
    public Vector3 Target { get => target; set => target = value; }

    public Character character;

    
    // Update is called once per frame
    private void Start()
    {
        VFX_Trail = Instantiate(ParticlePool.ParticleSystem(ParticleType.Trail));
        VFX_Trail.transform.localPosition = Vector3.zero;
        VFX_Trail.transform.localScale = VFX_Trail.transform.localScale * 2;
        VFX_Trail.gameObject.SetActive(false);
       
    }
    void Update()
    {
        if (isFire)
        {
            Throw();
        }
    }
    private void Throw() {
        VFX_Trail.transform.position = transform.position;
        VFX_Trail.gameObject.SetActive(true);
        if (IsWeaponType(WeaponType.Knife) || IsWeaponType(WeaponType.Arrow))
        {
            //Xoay Weapon to Enemy
            if (direction.x <= 0)
            {
                SetRotation(Vector3.forward);
            }
            else
            {
                SetRotation(Vector3.back);
            }

        }
        else
        {

            rotY += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Euler(90, rotY, 0);
        }

        //Move Weapon with transform.....
        if (Constant.IsDes(startPoint, gameObject.transform.position, character.InGameAttackRange))
        {
            Vector3 TargetPoint = new Vector3(transform.position.x + direction.x * bulletSpeed * Time.deltaTime, transform.position.y, transform.position.z + direction.z * bulletSpeed * Time.deltaTime);
            transform.position = TargetPoint;
        }
        else
        {
            ReleaseWeapon(character);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Character enemy = Constant.Cache.GetCharacter(other);
        //IHit hit = other.GetComponent<IHit>();
        if (enemy && other.gameObject != _GameObject && enemy.GetColorType() != character.GetColorType() && !enemy.IsDeath)
        {
            Player player = Constant.Cache.GetPlayer(other);
            if (player)
            {
                enemy.OnHit(1f);
                character.SetExp(enemy.InGamneExp);
                player.SetKilledByName(character.CharacterName);
                player.SetKillerColorType(character.GetColorType());
             
            }
            else
            {
                enemy.OnHit(1f);
                
                if (Constant.Cache.GetPlayer(_GameObject))
                {
                    Player player1 = Constant.Cache.GetPlayer(_GameObject);
                    player1.SetKilledCount(player1.KilledCount()+1);
                    if (GameManager.Instance.IsMode(GameMode.Normal))
                    {
                        player1.SetExp(enemy.InGamneExp);
                    }
                    else
                    {
                        player1.SetSurvivalExp(enemy.InGamneExp);
                    }
                }
               
                
            }
            ParticlePool.Play(ParticleType.Hit, transform.position, Quaternion.identity);
            ReleaseWeapon(character);
        }
        if (other.GetComponent<TransparentObstacle>())
        {
            if (VFX_Trail) {
                VFX_Trail.gameObject.SetActive(false);
                Destroy(VFX_Trail.gameObject);
            }
           
            
            this.isFire = false;
            character.ShowWeaponIndex((int)WeaponType);
            StartCoroutine(Waiter());
        }
    }
    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }
    private void ReleaseWeapon(Character character)
    {
        VFX_Trail.gameObject.SetActive(false);
        Destroy(VFX_Trail.gameObject);

        character.ShowWeaponIndex((int)WeaponType);
        this.gameObject.SetActive(false);
        this.isFire = false;
    }
    private void SetRotation(Vector3 upwards)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction, upwards);
        transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.eulerAngles += new Vector3(0, 90, 0);
    }

    public override void OnInit()
    { 
      
    }

    public override void OnDespawn()
    {
        Destroy(gameObject);
    }
    private bool IsWeaponType(WeaponType weaponType) 
    { 
        return this.weaponType == weaponType; 
    }
}
