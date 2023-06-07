using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private GameObject accessoriesPrefabs;
    [SerializeField] private GameObject frameFocus;
    private int itemID;
    public void OnSelected(bool isCheck)
    {
        frameFocus.SetActive(isCheck);
    }

    public GameObject FrameFocus { get => frameFocus; set => frameFocus = value; }
    public GameObject AccessoriesPrefabs { get => accessoriesPrefabs; set => accessoriesPrefabs = value; }
    public int ItemID { get => itemID; set => itemID = value; }
}
