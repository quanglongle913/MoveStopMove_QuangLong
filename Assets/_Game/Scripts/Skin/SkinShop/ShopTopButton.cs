using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTopButton : MonoBehaviour
{
    [SerializeField] private GameObject frameButton;

    public GameObject FrameButton { get => frameButton; set => frameButton = value; }
    public void Selected()
    {
        frameButton.SetActive(true);
    }
    public void UnSelected()
    {
        frameButton.SetActive(false);
    }
}
