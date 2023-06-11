using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    [SerializeField] private RawImage image, image2;
    [SerializeField] private TMPro.TextMeshProUGUI textLevel;
    [SerializeField] private ColorData colorData;
    [SerializeField] private ColorType colorType;

    
    private GameObject player;
    private GameObject target;
    private Camera mainCam;
    private Camera radarCam;
    private int characterLevel;
    public ColorData ColorData { get => colorData; set => colorData = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public RawImage Image { get => image; set => image = value; }
    public RawImage Image2 { get => image2; set => image2 = value; }
    public TextMeshProUGUI TextLevel { get => textLevel; set => textLevel = value; }

    public Camera MainCam { get => mainCam; set => mainCam = value; }
    public Camera RadarCam { get => radarCam; set => radarCam = value; }
    public int CharacterLevel { get => characterLevel; set => characterLevel = value; }
    public GameObject Player { get => player; set => player = value; }
    public GameObject Target { get => target; set => target = value; }

    public void updateData(ColorType colorType,int _level)
    {
        this.ColorType = colorType;
        CharacterLevel = _level;
        image.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
        image2.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
        textLevel.text = "" + _level;
        if (this.ColorType == ColorType.Yellow)
        {
            textLevel.color = Color.black;
        }
        else
        {
            textLevel.color = Color.white;
        }
    }
    public void ChangeColor(RawImage a_obj, ColorType colorType)
    {
        this.ColorType = colorType;
        a_obj.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
    }
    public void SetTextRotation(Quaternion rotation) 
    {
        TextLevel.transform.rotation = rotation;
    }
    public void SetRotation(Quaternion rotation)
    {
        gameObject.transform.rotation = rotation;
    }
}
