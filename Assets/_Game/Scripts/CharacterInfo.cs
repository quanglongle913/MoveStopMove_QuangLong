using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    public TMPro.TextMeshProUGUI CharacterName;
    public TMPro.TextMeshProUGUI CharacterLevel;
    public RawImage imageLevelBG;
    public void ChangeColor(ColorType colorType, ColorData colorData)
    {
        imageLevelBG.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
    }
    public void setCharacterName(string _name)
    {
        CharacterName.text = _name;
    }
}
