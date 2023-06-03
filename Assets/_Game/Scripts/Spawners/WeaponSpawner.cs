using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : PooledObject
{
    [SerializeField] private GameObject poolMaster;
    [SerializeField] private ObjectPool poolObject;

    [SerializeField] public GameObject Character;


    private bool isInit;
    public bool IsInit { get => isInit; set => isInit = value; }
    void Start()
    {
        //weaponMannager = WeaponMannager.Instance;
    }
    public Weapon GenerateWeapon(GameObject a_root, GameObject character)
    {
        GameObject a_wepaon = Character.GetComponent<Character>().Weapon.gameObject;
        PooledObject weaponObject = Spawner(poolObject, a_root, false);
        weaponObject.transform.position = a_root.transform.position;
        weaponObject.transform.localPosition = a_wepaon.transform.localPosition;
        weaponObject.transform.localRotation = a_wepaon.transform.localRotation;
        weaponObject.transform.localScale = a_wepaon.transform.localScale;
        weaponObject.GetComponent<Weapon>().isFire = false;
        //Character.GetComponent<Character>().Weapon = weaponObject.gameObject;
        //isInit = true;
        return weaponObject.GetComponent<Weapon>();
    }

}