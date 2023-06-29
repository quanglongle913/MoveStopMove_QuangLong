using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopUI : UICanvas
{
    [SerializeField] private List<GameObject> Buttons;
    [SerializeField] private ListWeapon WeaponRoot;

    [SerializeField] private TMPro.TextMeshProUGUI textWeaponBuffInfo;
    [SerializeField] private TMPro.TextMeshProUGUI textWeaponType;
    [SerializeField] private TMPro.TextMeshProUGUI textWeaponStatus;
    [SerializeField] private TMPro.TextMeshProUGUI textWeaponPriceBtnBuy;
    [SerializeField] private TMPro.TextMeshProUGUI textWeaponPriceBtnUnBuy;
    
    ListWeapon Weapons;
    int itemSelelected;
    Player player;
    private void Start()
    {
        player = GameManager.Instance.Player();
        itemSelelected = 0;
        itemSelelected = player.GetWeaponsEquippedIndex(GameManager.Instance.GetWeaponData());
        player.ShowWeaponIndex(itemSelelected);
        Weapons = Instantiate(WeaponRoot);
        Weapons.Show(itemSelelected);
        BtnWeaponShopUpdate();
    }
    public void Show()
    {   if (Weapons)
        {
            Weapons.gameObject.SetActive(true);
        }
    }
    public void CloseButton()
    {
        UIManager.Instance.OpenUI<GameMenu>();
        Weapons.gameObject.SetActive(false);
        GameManager.Instance.ShowPlayer();
        Close();
      
    }
    public void NextButton()
    {
       
        if (itemSelelected < Weapons.Count()-1)
        {
            Weapons.Hide(itemSelelected);
            itemSelelected++;
            Weapons.Show(itemSelelected);
            BtnWeaponShopUpdate();
        }
        
    }
    public void PreviewButton()
    {
        if (itemSelelected >0)
        {
            Weapons.Hide(itemSelelected);
            itemSelelected--;
            Weapons.Show(itemSelelected);
            BtnWeaponShopUpdate();
        }
    }
    private void HideAllButtons()
    { 
        for (int i = 0; i < Buttons.Count; i++)
        {
            var button = Buttons[i];
            button.SetActive(false);
        }
    }
    public void Selelect_EquippedButton()
    {
        UnEquippedAllWeapon(GameManager.Instance.GetWeaponData());
        GameManager.Instance.GetWeaponData().Weapon[itemSelelected].Equipped = true;
        player.WeaponIndex = itemSelelected;
        GameManager.Instance.UpdateData();
        BtnWeaponShopUpdate();
        CloseButton();
        //Hide_Popup_WeaponShop();
    }
    public void BuyWeaponButton()
    {
        if (PlayerPrefs.GetInt(Constant.PLAYER_COIN) >= GameManager.Instance.GetWeaponData().Weapon[itemSelelected].WeaponPrice)
        {
            int coin = PlayerPrefs.GetInt(Constant.PLAYER_COIN) - GameManager.Instance.GetWeaponData().Weapon[itemSelelected].WeaponPrice;
            PlayerPrefs.SetInt(Constant.PLAYER_COIN, coin);
            PlayerPrefs.Save();
            GameManager.Instance.GetWeaponData().Weapon[itemSelelected].Buyed = true;
            UnEquippedAllWeapon(GameManager.Instance.GetWeaponData());
            GameManager.Instance.GetWeaponData().Weapon[itemSelelected].Equipped = true;
            player.WeaponIndex = itemSelelected;
            GameManager.Instance.UpdateData();
            BtnWeaponShopUpdate();
            CloseButton();
            //Hide_Popup_WeaponShop();
        }
        else
        {
            Debug.Log("You don't have enough coins to make this");
        }
    }
    private void UnEquippedAllWeapon(WeaponData weaponData)
    {
        for (int i = 0; i < weaponData.Weapon.Count; i++)
        {
            weaponData.Weapon[i].Equipped = false;
        }
    }
    public void BtnWeaponShopUpdate()
    {
        player.ShowWeaponIndex(itemSelelected);
        player.WeaponIndex = itemSelelected;
        player.WeaponType = (WeaponType)itemSelelected;
        player.SetWeaponSkinMat();
        HideAllButtons();
        Weapon weapon = GameManager.Instance.GetWeaponData().Weapon[itemSelelected];
        textWeaponType.text = "" + weapon.WeaponName;
        textWeaponPriceBtnBuy.text = "" + weapon.WeaponPrice;
        textWeaponPriceBtnUnBuy.text = "" + weapon.WeaponPrice;
        textWeaponBuffInfo.text = "+" + weapon.BuffData.BuffIndex + " " + weapon.BuffData.BuffType; //GameManager. WeaponData.Weapon[i].BuffType & BuffInfo

        if (itemSelelected == player.GetWeaponsEquippedIndex(GameManager.Instance.GetWeaponData()))
        {
            Buttons[1].SetActive(true);
            textWeaponType.color = Color.white;
            textWeaponStatus.gameObject.SetActive(false);
        }
        else if (itemSelelected <= player.GetNumberOfWeaponsHave(GameManager.Instance.GetWeaponData()))
        {
            Buttons[0].SetActive(true);
            textWeaponType.color = Color.white;
            textWeaponStatus.gameObject.SetActive(false);
        }
        else if (itemSelelected == player.GetNumberOfWeaponsHave(GameManager.Instance.GetWeaponData()) + 1)
        {
            Buttons[2].SetActive(true);
            textWeaponType.color = Color.black;
            textWeaponStatus.text = "(Lock)";
            textWeaponStatus.gameObject.SetActive(true);
        }
        else
        {
            Buttons[3].SetActive(true);
            textWeaponType.color = Color.black;
            if (itemSelelected > 0)
            {
                textWeaponStatus.text = "UnLock " + GameManager.Instance.GetWeaponData().Weapon[itemSelelected - 1].WeaponName + " First";
            }

            textWeaponStatus.gameObject.SetActive(true);
        }
    }
}
