using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    float rotY;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotY += Time.deltaTime*rotationSpeed;
        transform.rotation= Quaternion.Euler( 90,rotY,0);
    }
}
