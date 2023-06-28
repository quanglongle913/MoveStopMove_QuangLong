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
    public float rotationSpeed;
    public float bulletSpeed=2f;
    public bool isFire;
    public Vector3 target;
    public Vector3 startPoint;
    public Vector3 direction;
    ParticleSystem VFX_Trail;
    float rotY;
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public GameObject _GameObject { get => _gameObject; set => _gameObject = value; }

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

            VFX_Trail.transform.position= transform.position;
            VFX_Trail.gameObject.SetActive(true);
            if (weaponType == WeaponType.Knife || weaponType == WeaponType.Arrow)/////
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
                player.KilledByName = character.CharacterName;
                player.KillerColorType = character.GetColorType();
             
            }
            else
            {
                enemy.OnHit(1f);
                
                if (Constant.Cache.GetPlayer(_GameObject))
                {
                    Player player1 = Constant.Cache.GetPlayer(_GameObject);
                    player1.KilledCount++;
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
            //Debug.Log("Obstacle");
            this.isFire = false;
            character.ShowWeaponIndex((int)WeaponType);
            character.IsAttacking = false;
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
        //character.Weapons.Remove(this);
        VFX_Trail.gameObject.SetActive(false);
        Destroy(VFX_Trail.gameObject);

        character.ShowWeaponIndex((int)WeaponType);
        character.IsAttacking = false;
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
}
