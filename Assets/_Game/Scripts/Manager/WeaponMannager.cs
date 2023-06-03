using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponMannager : MonoBehaviour
{
    public static WeaponMannager Instance;
    [SerializeField ] private ObjectPool[] poolObject;
    [SerializeField] private WeaponData weaponData;

    public ObjectPool[] PoolObject { get => poolObject; set => poolObject = value; }
    public WeaponData WeaponData { get => weaponData; set => weaponData = value; }

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
    }
    /*private void Start()
    {
        OnInit();
    }
    private void OnInit()
    {

    }*/
}
