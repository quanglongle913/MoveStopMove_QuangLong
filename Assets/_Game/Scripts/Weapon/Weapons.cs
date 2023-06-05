using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Weapons : MonoBehaviour
{
    //[SerializeField] Rigidbody _rigidbody;
    [SerializeField] private WeaponType weaponType;
    //Character =_GameObject
    public GameObject _GameObject;
    public float rotationSpeed;
    public bool isFire;
    float rotY;
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            if (weaponType == WeaponType.Knife)
            {

            }
            else
            {
                rotY += Time.deltaTime * rotationSpeed;
                transform.rotation = Quaternion.Euler(90, rotY, 0);
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
                enemy.OnHit(0f);
                characterRoot.setExp(enemy.InGamneExp);
            }
            else
            {
                enemy.OnHit(2f);
                characterRoot.setExp(enemy.InGamneExp);
            }
            gameObject.SetActive(false);
            //OnDespawn();
        }
    }
}
