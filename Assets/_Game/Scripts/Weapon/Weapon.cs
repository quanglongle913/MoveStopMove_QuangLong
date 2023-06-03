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
        }
    }
}
