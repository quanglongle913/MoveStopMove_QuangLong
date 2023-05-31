using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{
    [SerializeField] private RawImage image, image2;
    [SerializeField] private ColorData colorData;
    [SerializeField] private ColorType colorType;

    public ColorData ColorData { get => colorData; set => colorData = value; }
    public ColorType ColorType { get => colorType; set => colorType = value; }
    public RawImage Image { get => image; set => image = value; }
    public RawImage Image2 { get => image2; set => image2 = value; }

    // Start is called before the first frame update
    /*   void Start()
       {
           ChangeColor(image,colorType);
           ChangeColor(image2, colorType);
       }*/

    // Update is called once per frame
    /* void Update()
     {

     }*/
    public void ChangeColor( ColorType colorType)
    {
        this.ColorType = colorType;
        image.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
        image2.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
    }
    public void ChangeColor(RawImage a_obj, ColorType colorType)
    {
        this.ColorType = colorType;
        a_obj.GetComponent<RawImage>().color = colorData.GetMat(colorType).color;
    }
}
