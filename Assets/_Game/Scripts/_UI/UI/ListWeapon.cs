using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListWeapon : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons;
    public void HideAll()
    { 
        for (int i = 0; i < weapons.Count; i++)
        {
            var weapon = weapons[i];    
            weapon.gameObject.SetActive(false);
        }
    }
    public void Show(int index)
    {
        var weapon = weapons[index];
        weapon.gameObject.SetActive(true);
    }
    public void Hide(int index)
    {
        var weapon = weapons[index];
        weapon.gameObject.SetActive(false);
    }
    public int Count()
    { 
        return weapons.Count;
    }
}
