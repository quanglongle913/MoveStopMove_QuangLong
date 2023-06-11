using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CharacterInfo : MonoBehaviour
{
    public TMPro.TextMeshProUGUI CharacterName;
    public TMPro.TextMeshProUGUI CharacterLevel;
    public RawImage imageLevelBG;
    private GameObject target;
    private Camera mainCam;

    public Camera MainCam { get => mainCam; set => mainCam = value; }
    public GameObject Target { get => target; set => target = value; }

   /* public void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            Character character = target.GetComponent<Character>();
            setCharacterName(character.CharacterName);
            setCharacterLevel("" + character.CharacterLevel);
            ChangeColor(character.ColorType, character.ColorData);
            if (character.ColorType == ColorType.Yellow)
            {
                CharacterLevel.color = Color.black;
            }
            else
            {
                CharacterLevel.color = Color.white;
            }
        }
    }*/

    public void ChangeColor(ColorType colorType, ColorData colorData)
    {
        imageLevelBG.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
        if (colorType == ColorType.Yellow)
        {
            CharacterLevel.color = Color.black;
        }
        else
        {
            CharacterLevel.color = Color.white;
        }
    }
    public void setCharacterName(string _name)
    {
        CharacterName.text = _name;
    }
    public void setCharacterLevel(string _level)
    {
        CharacterLevel.text = _level;
    }
}
