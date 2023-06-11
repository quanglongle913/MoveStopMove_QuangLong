using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffVfx : MonoBehaviour
{
    public GameObject _Character;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (_Character != null)
        { 
            gameObject.transform.position = _Character.transform.position;
        }
    }
}
