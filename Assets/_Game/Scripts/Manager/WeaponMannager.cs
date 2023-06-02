using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMannager : PooledObject
{
    [SerializeField] private GameObject poolMaster;
    [SerializeField] private ObjectPool poolObject;

    [SerializeField] public GameObject Character;
    public static WeaponMannager Instance;

    private bool isInit;
    public bool IsInit { get => isInit; set => isInit = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //isInit = false;
    }
    private void Update()
    {
        //if (!IsInit)
        //{
        //    GenerateWeapon(poolMaster,Character);
        //}
    }
    public Weapon GenerateWeapon(GameObject a_root,GameObject character)
    {
        GameObject a_wepaon = Character.GetComponent<Character>().Weapon.gameObject;
        PooledObject weaponObject = Spawner(poolObject, a_root);
        weaponObject.transform.position = a_root.transform.position;
        weaponObject.transform.localPosition = a_wepaon.transform.localPosition;
        //weaponObject.transform.localRotation = Quaternion.Euler(0, 0, 104.6f);
        weaponObject.transform.localRotation = a_wepaon.transform.localRotation;
        weaponObject.transform.localScale = a_wepaon.transform.localScale;
        weaponObject.GetComponent<Weapon>().isFire = false;
        //Character.GetComponent<Character>().Weapon = weaponObject.gameObject;
        //isInit = true;
        return weaponObject.GetComponent<Weapon>();
    }

}
