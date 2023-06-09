using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private RawImage imageItem;
    [SerializeField] private GameObject frameFocus;
    private int itemID;
    private SkinType skinType;
    public void SetSelected(bool isCheck)
    {
        frameFocus.SetActive(isCheck);
    }

    public GameObject FrameFocus { get => frameFocus; set => frameFocus = value; }
    public int ItemID { get => itemID; set => itemID = value; }
    public RawImage ImageItem { get => imageItem; set => imageItem = value; }
    public SkinType SkinType { get => skinType; set => skinType = value; }
}
