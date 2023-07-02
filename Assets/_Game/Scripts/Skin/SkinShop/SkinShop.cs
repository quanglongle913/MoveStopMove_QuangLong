using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShop : MonoBehaviour
{
    [SerializeField] GameObject ItemTemplate;
    AccessoriesData accessoriesData;
    GameObject g;
    [SerializeField] Transform ShopScrollView;
    private List<ShopItem> items =new List<ShopItem>();
    private bool isUpdate=false;
    public AccessoriesData AccessoriesData { get => accessoriesData; set => accessoriesData = value; }
    public List<ShopItem> Items { get => items; set => items = value; }
    public bool IsUpdate { get => isUpdate; set => isUpdate = value; }

    private void Start()
    {
       // items = new List<ShopItem>();
    }
    void Update()
    {
        if (IsUpdate)
        {            
            for (int i = 0; i < accessoriesData.Accessories.Count; i++)
            {
                g = Instantiate(ItemTemplate, ShopScrollView);
                ShopItem shopItem = g.gameObject.GetComponent<ShopItem>();
                shopItem.ImageItem.texture = accessoriesData.Accessories[i].ImageItem;
                shopItem.ItemID = i;
                shopItem.FrameFocus.SetActive(accessoriesData.Accessories[i].Selected);
                shopItem.SkinType = accessoriesData.SkinType;
                items.Add(shopItem);
            }
            IsUpdate = false;
        }
        
    }
    public void ClearnItems()
    {
        for (int i = 0; i < Items.Count; i++)
        { 
            Destroy(Items[i].gameObject);
        }
        Items.Clear();
    }
    public void ItemsOnClicked(int index)
    {
        SetAllItemsUnSelected();
        items[index].SetSelected(true);
        //Debug.Log("Items "+index);
    }

    private void SetAllItemsUnSelected()
    {
        for (int i=0; i<items.Count;i++)
        {
            items[i].SetSelected(false);

        }
    }
}
