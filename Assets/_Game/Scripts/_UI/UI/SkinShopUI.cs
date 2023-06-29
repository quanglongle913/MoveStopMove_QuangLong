using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopUI : UICanvas
{
    [Header("SkinShop: ")]
    [SerializeField] private SkinShop skinShop;

    [SerializeField] private GameObject frame_TopHatSkinShop;
    [SerializeField] private GameObject frame_TopPantsSkinShop;
    [SerializeField] private GameObject frame_TopSheildSkinShop;
    [SerializeField] private GameObject frame_TopSetFullSkinShop;

    [SerializeField] private TMPro.TextMeshProUGUI textSkinBuffInfo;
    [SerializeField] private TMPro.TextMeshProUGUI textSkinShopPriceBtnBuy;
    [SerializeField] private List<GameObject> Buttons;
    Player player;
    
    private void Start()
    {
        GameManager.Instance.ChangeState(GameState.SkinShop);
        player = GameManager.Instance.Player();
        if (player.IsPlayerSkinShopState(PlayerSkinShopState.SetFull))
        {
            OnSlelectedSetFullSkinShop();
        }
        else
        {
            OnSlelectedHatSkinShop();
        }
    }
    public void OnSlelectedHatSkinShop()
    {
        AccessoriesData accessoriesData = GameManager.Instance.GetAccessoriesDatas()[0];
        frame_TopHatSkinShop.SetActive(false);
        frame_TopPantsSkinShop.SetActive(true);
        frame_TopSheildSkinShop.SetActive(true);
        frame_TopSetFullSkinShop.SetActive(true);
        InitSkinShop(accessoriesData);
        player.UpdateAccessoriesSkinShop();
        //UPdate Selected Items
        BtnSkinShopUpdate(accessoriesData, player.GetAccessorisSelectedIndex(accessoriesData));
    }
    public void OnSlelectedPaintSkinShop()
    {
        AccessoriesData accessoriesData = GameManager.Instance.GetAccessoriesDatas()[1];
        frame_TopHatSkinShop.SetActive(true);
        frame_TopPantsSkinShop.SetActive(false);
        frame_TopSheildSkinShop.SetActive(true);
        frame_TopSetFullSkinShop.SetActive(true);
        InitSkinShop(accessoriesData);

        player.UpdateAccessoriesSkinShop();
        //UPdate Selected Items
        BtnSkinShopUpdate(accessoriesData, player.GetAccessorisSelectedIndex(accessoriesData));
    }
    public void OnSlelectedSheildSkinShop()
    {
        AccessoriesData accessoriesData = GameManager.Instance.GetAccessoriesDatas()[2];
        frame_TopHatSkinShop.SetActive(true);
        frame_TopPantsSkinShop.SetActive(true);
        frame_TopSheildSkinShop.SetActive(false);
        frame_TopSetFullSkinShop.SetActive(true);
        InitSkinShop(accessoriesData);

        player.UpdateAccessoriesSkinShop();

        //UPdate Selected Items
        BtnSkinShopUpdate(accessoriesData, player.GetAccessorisSelectedIndex(accessoriesData));
    }
    public void OnSlelectedSetFullSkinShop()
    {
        AccessoriesData accessoriesData = GameManager.Instance.GetAccessoriesDatas()[3];
        frame_TopHatSkinShop.SetActive(true);
        frame_TopPantsSkinShop.SetActive(true);
        frame_TopSheildSkinShop.SetActive(true);
        frame_TopSetFullSkinShop.SetActive(false);
        InitSkinShop(accessoriesData);

        player.UpdateAccessoriesSkinShop();
        BtnSkinShopUpdate(accessoriesData, player.GetAccessorisSelectedIndex(accessoriesData));
    }
    private void InitSkinShop(AccessoriesData accessoriesData)
    {
        skinShop.ClearnItems();
        int index = player.GetAccessorisSelectedIndex(accessoriesData);
        accessoriesData.Accessories[index].Selected = true;
        skinShop.AccessoriesData = accessoriesData;
        skinShop.IsUpdate = true;
        textSkinBuffInfo.text = "+" + accessoriesData.Accessories[index].BuffData.BuffIndex + "% " + accessoriesData.Accessories[index].BuffData.BuffType;
        StartCoroutine(SetButtonCouroutime());
        //Debug.Log(""+ index);
    }
    IEnumerator SetButtonCouroutime()
    {
        yield return new WaitForSeconds(.5f);
        SetSkinShopButtonOnclick();
    }

    private void SetSkinShopButtonOnclick()
    {
        for (int i = 0; i < skinShop.Items.Count; i++)
        {
            skinShop.Items[i].ImageItem.gameObject.GetComponent<Button>().AddEventListener(i, OnShopItemBtnClicked);
        }

    }
    private void OnShopItemBtnClicked(int itemIndex)
    {

        //Sound
        PlaySoundBtnClick();
        skinShop.ItemsOnClicked(itemIndex);

        if (skinShop.Items[0].SkinType == SkinType.Hat)
        {
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[3]);
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[0]);
            GameManager.Instance.GetAccessoriesDatas()[0].Accessories[itemIndex].Selected = true;

            textSkinBuffInfo.text = "+" + GameManager.Instance.GetAccessoriesDatas()[0].Accessories[itemIndex].BuffData.BuffIndex + "% " + GameManager.Instance.GetAccessoriesDatas()[0].Accessories[itemIndex].BuffData.BuffType;

            player.UpdateAccessoriesSkinShop();
            BtnSkinShopUpdate(GameManager.Instance.GetAccessoriesDatas()[0], itemIndex);
        }
        else if (skinShop.Items[0].SkinType == SkinType.Pant)
        {
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[3]);
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[1]);
            GameManager.Instance.GetAccessoriesDatas()[1].Accessories[itemIndex].Selected = true;
            player.SetPantsSkin(GameManager.Instance.GetAccessoriesDatas()[1]);

            textSkinBuffInfo.text = "+" + GameManager.Instance.GetAccessoriesDatas()[1].Accessories[itemIndex].BuffData.BuffIndex + "% " + GameManager.Instance.GetAccessoriesDatas()[1].Accessories[itemIndex].BuffData.BuffType;

            player.UpdateAccessoriesSkinShop();
            BtnSkinShopUpdate(GameManager.Instance.GetAccessoriesDatas()[1], itemIndex);
        }
        else if (skinShop.Items[0].SkinType == SkinType.Sheild)
        {
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[3]);
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[2]);
            GameManager.Instance.GetAccessoriesDatas()[2].Accessories[itemIndex].Selected = true;

            textSkinBuffInfo.text = "+" + GameManager.Instance.GetAccessoriesDatas()[2].Accessories[itemIndex].BuffData.BuffIndex + "% " + GameManager.Instance.GetAccessoriesDatas()[2].Accessories[itemIndex].BuffData.BuffType;

            player.UpdateAccessoriesSkinShop();
            BtnSkinShopUpdate(GameManager.Instance.GetAccessoriesDatas()[2], itemIndex);
        }
        else if (skinShop.Items[0].SkinType == SkinType.SetFull)
        {
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[0]);
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[1]);
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[2]);
            player.SetAllAccessoriesUnSelected(GameManager.Instance.GetAccessoriesDatas()[3]);
            GameManager.Instance.GetAccessoriesDatas()[3].Accessories[itemIndex].Selected = true;

            textSkinBuffInfo.text = "+" + GameManager.Instance.GetAccessoriesDatas()[3].Accessories[itemIndex].BuffData.BuffIndex + "% " + GameManager.Instance.GetAccessoriesDatas()[3].Accessories[itemIndex].BuffData.BuffType;

            player.UpdateAccessoriesSkinShop();
            BtnSkinShopUpdate(GameManager.Instance.GetAccessoriesDatas()[3], itemIndex);
        }
    }

    private void HideAllButtonSkinShop()
    {   
        for(int i = 0;i<Buttons.Count;i++)
        {
            var btn = (Buttons[i]);
            btn.SetActive(false);
        }
    }
    public void BtnSkinShopUpdate(AccessoriesData accessoriesData, int indexItems)
    {
        HideAllButtonSkinShop();
        //player.GetAccessorisBuyedIndex(accessoriesData);
        if (accessoriesData.Accessories[indexItems].Buyed)
        {
            if (accessoriesData.Accessories[indexItems].Equipped)
            {
                Buttons[1].SetActive(true);
            }
            else
            {
                Buttons[0].SetActive(true);
            }
        }
        else
        {
            textSkinShopPriceBtnBuy.text = "" + accessoriesData.Accessories[indexItems].Price;
            Buttons[2].SetActive(true);
        }
    }
    public void OnClickBtnSelelect_EquippedSkinShop()
    {

        if (skinShop.Items[0].SkinType == SkinType.Hat)
        {
            UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[3]);
            setSelected(GameManager.Instance.GetAccessoriesDatas()[0], player.GetAccessorisSelectedIndex(GameManager.Instance.GetAccessoriesDatas()[0]));
        }
        else if (skinShop.Items[0].SkinType == SkinType.Pant)
        {
            UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[3]);
            setSelected(GameManager.Instance.GetAccessoriesDatas()[1], player.GetAccessorisSelectedIndex(GameManager.Instance.GetAccessoriesDatas()[1]));
        }
        else if (skinShop.Items[0].SkinType == SkinType.Sheild)
        {
            UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[3]);
            setSelected(GameManager.Instance.GetAccessoriesDatas()[2], player.GetAccessorisSelectedIndex(GameManager.Instance.GetAccessoriesDatas()[2]));
        }
        else if (skinShop.Items[0].SkinType == SkinType.SetFull)
        {
            UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[0]);
            UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[1]);
            UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[2]);
            setSelected(GameManager.Instance.GetAccessoriesDatas()[3], player.GetAccessorisSelectedIndex(GameManager.Instance.GetAccessoriesDatas()[3]));
        }
    }
    private void setSelected(AccessoriesData accessoriesData, int indexItems)
    {
        UnEquippedAllAccessoriesSkinShop(accessoriesData);
        accessoriesData.Accessories[indexItems].Equipped = true;
        BtnSkinShopUpdate(accessoriesData, indexItems);
        GameManager.Instance.UpdateData();
    }
    public void OnClickBtnBuySkinShop()
    {

        if (skinShop.Items[0].SkinType == SkinType.Hat)
        {
            UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[3]);
            SetBuy(GameManager.Instance.GetAccessoriesDatas()[0], player.GetAccessorisSelectedIndex(GameManager.Instance.GetAccessoriesDatas()[0]), false);
        }
        else if (skinShop.Items[0].SkinType == SkinType.Pant)
        {
            UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[3]);
            SetBuy(GameManager.Instance.GetAccessoriesDatas()[1], player.GetAccessorisSelectedIndex(GameManager.Instance.GetAccessoriesDatas()[1]), false);
        }
        else if (skinShop.Items[0].SkinType == SkinType.Sheild)
        {
            UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[3]);
            SetBuy(GameManager.Instance.GetAccessoriesDatas()[2], player.GetAccessorisSelectedIndex(GameManager.Instance.GetAccessoriesDatas()[2]), false);
        }
        else if (skinShop.Items[0].SkinType == SkinType.SetFull)
        {

            SetBuy(GameManager.Instance.GetAccessoriesDatas()[3], player.GetAccessorisSelectedIndex(GameManager.Instance.GetAccessoriesDatas()[3]), true);
        }
    }
    private void SetBuy(AccessoriesData accessoriesData, int indexItems, bool isSetfull)
    {
        if (PlayerPrefs.GetInt(Constant.PLAYER_COIN) >= accessoriesData.Accessories[indexItems].Price)
        {
            int coin = PlayerPrefs.GetInt(Constant.PLAYER_COIN) - accessoriesData.Accessories[indexItems].Price;
            PlayerPrefs.SetInt(Constant.PLAYER_COIN, coin);
            PlayerPrefs.Save();
            accessoriesData.Accessories[indexItems].Buyed = true;

            UnEquippedAllAccessoriesSkinShop(accessoriesData);
            accessoriesData.Accessories[indexItems].Equipped = true;

            if (isSetfull)
            {
                UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[0]);
                UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[1]);
                UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[2]);
            }
            else
            {
                UnEquippedAllAccessoriesSkinShop(GameManager.Instance.GetAccessoriesDatas()[3]);
            }
            BtnSkinShopUpdate(accessoriesData, indexItems);
            GameManager.Instance.UpdateData();
        }
        else
        {
            Debug.Log("You don't have enough coins to make this");
        }
    }
    private void UnEquippedAllAccessoriesSkinShop(AccessoriesData accessoriesData)
    {
        for (int i = 0; i < accessoriesData.Accessories.Count; i++)
        {
            accessoriesData.Accessories[i].Equipped = false;
        }
    }
    public void CloseButton()
    {
        player.UpdateAccessoriesEquippedAll();
        UIManager.Instance.OpenUI<GameMenu>();
       
        Close();
    }
}
