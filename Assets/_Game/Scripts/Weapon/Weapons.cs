using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using DG.Tweening.Core.Easing;
using UnityEngine.TextCore.Text;

public class Weapons : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private GameObject FireVfx;
    
    private GameObject _gameObject;
    public float rotationSpeed;
    public bool isFire;
    public Vector3 target;
    public Vector3 startPoint;
    public Vector3 direction;
    GameManager _GameManager;
    GameObject newFireVfx;
    float rotY;
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    public GameObject NewFireVfx { get => newFireVfx; set => newFireVfx = value; }
    public GameObject _GameObject { get => _gameObject; set => _gameObject = value; }

    private bool isCharacter =false;
    Character character;
    // Update is called once per frame
    private void Start()
    {
        _GameManager= GameManager.Instance;
        _GameManager.VfxManager.GenerateFireVfx(this);
    }
    void Update()
    {
        if (!character)
        {
            character = _GameObject.GetComponent<Character>();
        }
        if (isFire)
        {
            if (weaponType == WeaponType.Knife || weaponType == WeaponType.Arrow)/////
            {
                newFireVfx.SetActive(true);
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
                newFireVfx.SetActive(true);
                rotY += Time.deltaTime * rotationSpeed;
                transform.rotation = Quaternion.Euler(90, rotY, 0);
            }
        
            //Move Weapon with transform.....
            if (Constant.IsDes(startPoint, gameObject.transform.position, character.InGameAttackRange))
            {
                Vector3 TargetPoint = new Vector3(transform.position.x + direction.x * 1f * Time.deltaTime, transform.position.y, transform.position.z + direction.z * 1f * Time.deltaTime);
                transform.position = TargetPoint;
            }
            else
            {
                //Debug.Log("sss");
                ReleaseWeapon(character);
            }
            
        }
    }
    public void OnDespawn()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Character enemy = other.GetComponent<Character>();
        if (enemy && other.gameObject != _GameObject && enemy.ColorType != character.ColorType)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                enemy.OnHit(1f);
                character.setExp(enemy.InGamneExp);
                other.gameObject.GetComponent<Player>().KilledByName = character.CharacterName;
                other.gameObject.GetComponent<Player>().KillerColorType = character.ColorType;
                other.gameObject.GetComponent<Player>().SetEndGame();
            }
            else
            {
                enemy.OnHit(1f);
                character.setExp(enemy.InGamneExp);
                if (_GameObject.GetComponent<Player>())
                {
                    _GameObject.GetComponent<Player>().KilledCount++;
                }
            }
            _GameManager.VfxManager.ShowBloodVfx(this);
  
            ReleaseWeapon(character);
        }
        if (other.GetComponent<TransparentObstacle>())
        {
            //Debug.Log("Obstacle");
            this.isFire = false;
            character.Weapons.Remove(this);
            character.ShowWeaponIndex((int)WeaponType);
            character.IsAttacking = false;
            StartCoroutine(Waiter());
        }
        if (other.GetComponent<AnimalAI>()) 
        {
            other.GetComponent<AnimalAI>().OnHit(1f);
            //character.setExp(other.GetComponent<AnimalAI>().InGamneExp);
            _GameManager.VfxManager.ShowBloodVfx(this);

            ReleaseWeapon(character);
        }
    }
    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(1f);

        this.gameObject.SetActive(false);
        this.GetComponent<PooledObject>().Release();
    }
    private void ReleaseWeapon(Character character)
    {
        character.Weapons.Remove(this);
        character.ShowWeaponIndex((int)WeaponType);
        character.IsAttacking = false;
        this.gameObject.SetActive(false);
        this.isFire = false;
        this.GetComponent<PooledObject>().Release();
    }
    private void SetRotation(Vector3 upwards)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction, upwards);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        newFireVfx.gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        gameObject.transform.eulerAngles += new Vector3(0, 90, 0);
        newFireVfx.gameObject.transform.eulerAngles += new Vector3(0, 90, 0);
    }
}
