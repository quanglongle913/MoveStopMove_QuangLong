using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShop : MonoBehaviour
{
    [SerializeField] GameObject ItemTemplate;
    AccessoriesData accessoriesData;
    GameObject g;
    [SerializeField] Transform ShopScrollView;
    private List<GameObject> items = new List<GameObject>();
    public bool IsUpdate=false;
    public AccessoriesData AccessoriesData { get => accessoriesData; set => accessoriesData = value; }
    public List<GameObject> Items { get => items; set => items = value; }

    void Update()
    {
        if (IsUpdate)
        {            
            for (int i = 0; i < accessoriesData.Accessories.Length; i++)
            {
                g = Instantiate(ItemTemplate, ShopScrollView);
                ShopItem shopItem = g.gameObject.GetComponent<ShopItem>();
                shopItem.ImageItem.texture = accessoriesData.Accessories[i].ImageItem;
                shopItem.FrameFocus.SetActive(accessoriesData.Accessories[i].Selected);
                items.Add(shopItem.gameObject);
            }
            IsUpdate = false;
        }
        
    }
    public void ClearnItems()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Destroy(Items[i]);
        }
    }
}
