using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class CharacterInfo : GameUnit
{
    [SerializeField] private ColorData colorData;
    [SerializeField] private TMPro.TextMeshProUGUI CharacterName;
    [SerializeField] private TMPro.TextMeshProUGUI CharacterLevel;
    [SerializeField] private RawImage imageLevelBG;
    private Character character;
    public void SetCharacter(Character character)
    {
        this.character = character;
    }
    public void UpdateData()
    {
        Vector3 viewPosCharacterInfo = GameManager.Instance.GetCamera().WorldToScreenPoint(character.gameObject.transform.position);
        
        CharacterName.text = character.CharacterName;
        CharacterLevel.text = ""+ character.GetLevel();
        ChangeColor(character.GetColorType());
        gameObject.transform.position = new Vector2(viewPosCharacterInfo.x, viewPosCharacterInfo.y + 1.4f * Screen.height / 10);
        Show();
    }
    private void ChangeColor(ColorType colorType)
    {
        imageLevelBG.color = colorData.GetMat(colorType).color;
        if (colorType == ColorType.Yellow)
        {
            CharacterLevel.color = Color.black;
        }
        else
        {
            CharacterLevel.color = Color.white;
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public override void OnInit()
    {
       
    }

    public override void OnDespawn()
    {
        
    }
}
