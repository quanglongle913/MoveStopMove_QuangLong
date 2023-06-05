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
    
    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            rotY += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Euler(90, rotY, 0);
        }
    }
    public void OnDespawn()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        Character characterRoot = _GameObject.GetComponent<Character>();
        if (character && other.gameObject != _GameObject && character.ColorType != characterRoot.ColorType)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                other.gameObject.GetComponent<Character>().OnHit(0f);
            }
            else
            {
                other.gameObject.GetComponent<Character>().OnHit(2f);
            }
            gameObject.SetActive(false);
            //OnDespawn();
        }
    }
}
