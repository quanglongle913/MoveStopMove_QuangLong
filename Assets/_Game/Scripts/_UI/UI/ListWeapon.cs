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
            weapons[i].gameObject.SetActive(false);
        }
    }
    public void Show(int index)
    {
        weapons[index].gameObject.SetActive(true);
    }
    public void Hide(int index)
    {
        weapons[index].gameObject.SetActive(false);
    }
    public int Count()
    { 
        return weapons.Count;
    }
}
