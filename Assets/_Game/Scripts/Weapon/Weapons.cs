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
    public GameObject _GameObject;
    public float rotationSpeed;
    public bool isFire;
    public Vector3 target;
    public Vector3 startPoint;
    public Vector3 direction;
    float rotY;
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            if (weaponType == WeaponType.Knife || weaponType == WeaponType.Arrow)/////
            {
                //Xoay Weapon to Enemy
                if (direction.x <= 0)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.forward);
                    gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                    gameObject.transform.eulerAngles += new Vector3(0, 90, 0);
                }
                else
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.back);
                    gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
                    gameObject.transform.eulerAngles += new Vector3(0, 90, 0);
                }

            }
            else
            {
                rotY += Time.deltaTime * rotationSpeed;
                transform.rotation = Quaternion.Euler(90, rotY, 0);
            }
            Character character = _GameObject.GetComponent<Character>();
            //Move Weapon with transform.....
            if (Constant.IsDes(startPoint, gameObject.transform.position, character.InGamneAttackRange))
            {
                Vector3 TargetPoint = new Vector3(transform.position.x + direction.x * 1f * Time.deltaTime, transform.position.y, transform.position.z + direction.z * 1f * Time.deltaTime);
                transform.position = TargetPoint;
            }
            else
            {
                //Debug.Log("sss");
                character.Weapons.Remove(this);
                character.ShowWeaponIndex((int)WeaponType);
                character.IsAttacking = false;
                this.gameObject.SetActive(false);
                this.isFire = false;
                this.GetComponent<PooledObject>().Release();
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
        Character characterRoot = _GameObject.GetComponent<Character>();
        if (enemy && other.gameObject != _GameObject && enemy.ColorType != characterRoot.ColorType)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                enemy.OnHit(1f);
                characterRoot.setExp(enemy.InGamneExp);
                other.gameObject.GetComponent<Player>().KilledByName = characterRoot.CharacterName;
                other.gameObject.GetComponent<Player>().KillerColorType = characterRoot.ColorType;
                other.gameObject.GetComponent<Player>().SetEndGame();
            }
            else
            {
                enemy.OnHit(1f);
                characterRoot.setExp(enemy.InGamneExp);
                if (_GameObject.GetComponent<Player>())
                {
                    _GameObject.GetComponent<Player>().KilledCount++;
                }
            }
            Character character = _GameObject.GetComponent<Character>();
            character.Weapons.Remove(this);
            character.ShowWeaponIndex((int)WeaponType);
            character.IsAttacking = false;
            this.gameObject.SetActive(false);
            this.isFire = false;
            this.GetComponent<PooledObject>().Release();
            //OnDespawn();
        }
        if (other.GetComponent<Obstacle>())
        {
            Debug.Log("Obstacle");
            this.isFire = false;
            Character character = _GameObject.GetComponent<Character>();
            character.Weapons.Remove(this);
            character.ShowWeaponIndex((int)WeaponType);
            character.IsAttacking = false;
            StartCoroutine(Waiter());
        }
    }
    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
        this.GetComponent<PooledObject>().Release();
    }
}
