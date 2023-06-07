using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopItem : MonoBehaviour
{
    public float rotX, rotY, rotZ;
    public float rotationSpeedY;
    void Update()
    {
       // rotX += Time.deltaTime * rotationSpeedX;
        rotY += Time.deltaTime * rotationSpeedY;
        //rotZ += Time.deltaTime * rotationSpeedZ;
        transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
    }
}
