using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : PooledObject
{
    //[SerializeField] private GameObject poolMaster;
    //[SerializeField] private ObjectPool poolObject;
    //[SerializeField] public GameObject Character;


    private bool isInit;
    public bool IsInit { get => isInit; set => isInit = value; }
    public Weapons GenerateWeapon(GameObject a_root, ObjectPool poolObject)
    {
        PooledObject weaponObject = Spawner(poolObject, a_root, false);
        weaponObject.transform.localPosition = Vector3.zero;
        //weaponObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        weaponObject.GetComponent<Weapons>().isFire = false;
        return weaponObject.GetComponent<Weapons>();
    }

}