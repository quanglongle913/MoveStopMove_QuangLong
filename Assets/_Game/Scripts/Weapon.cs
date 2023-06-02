using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Weapon : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
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
            //Vector3 newTarget = new Vector3(_rigidbody.position.x + _Direction.x * Time.deltaTime * moveSpeed, _rigidbody.position.y, _rigidbody.position.z + _Direction.z * Time.deltaTime * moveSpeed);
            //transform.position = TargetPoint;
            //if (!Constant.IsDes(weaponMannager.Character.transform.position, transform.position, weaponMannager.Character.GetComponent<Character>().AttackRange))
            //{
            //    gameObject.GetComponent<PooledObject>().Release();
            //    weaponMannager.IsInit = false;
            //}
        }
    }
}
