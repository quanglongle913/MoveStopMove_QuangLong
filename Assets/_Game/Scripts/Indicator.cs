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
 
  /*  public void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            Vector3 viewPosPlayer = mainCam.WorldToViewportPoint(Player.gameObject.transform.position);
            Vector3 viewPos = mainCam.WorldToViewportPoint(Target.gameObject.transform.position);
            // Your object isn't in the range of the camera
            Vector2 A = new Vector2(viewPos.x, viewPos.y);
            Vector2 B = new Vector2(viewPosPlayer.x, viewPosPlayer.y);
            float angle1 = Constant.AngleBetween2Vector2Up(B, A);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, angle1);
            gameObject.GetComponent<Indicator>().TextLevel.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }*/
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
}
