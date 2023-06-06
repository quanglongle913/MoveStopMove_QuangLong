using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    [SerializeField] private RawImage image, image2;
    [SerializeField] private TMPro.TextMeshProUGUI textLevel;
    [SerializeField] private ColorData colorData;
    [SerializeField] private ColorType colorType;

    public ColorData ColorData { get => colorData; set => colorData = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public RawImage Image { get => image; set => image = value; }
    public RawImage Image2 { get => image2; set => image2 = value; }
    public TextMeshProUGUI TextLevel { get => textLevel; set => textLevel = value; }

    public void updateData( ColorType colorType,int _level)
    {
        this.ColorType = colorType;
        image.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
        image2.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
        textLevel.text = "" + _level;
    }
    public void ChangeColor(RawImage a_obj, ColorType colorType)
    {
        this.ColorType = colorType;
        a_obj.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
    }
}
