using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Indicator : GameUnit
{
    [SerializeField] private float screenBoundOffset = 0.9f;
    [SerializeField] private RawImage image, image2;
    [SerializeField] private TMPro.TextMeshProUGUI textLevel;
    [SerializeField] private ColorData colorData;
    [SerializeField] private ColorType colorType;

    private int characterLevel;
    
    private Character character;
    
    public void SetCharacter(Character character)
    {
        this.character = character;
        this.colorType = character.GetColorType();
        characterLevel = character.GetLevel();
        ChangeColor(image, colorType);
        ChangeColor(image2, colorType);
        textLevel.text = "" + characterLevel;
        if (this.colorType == ColorType.Yellow)
        {
            textLevel.color = Color.black;
        }
        else
        {
            textLevel.color = Color.white;
        }
    }
    public void UpdateData(Vector3 vector3)
    {
        Camera camera = GameManager.Instance.GetCamera();
        
        //gameObject.transform.position = OffScreenIndicatorCore.GetScreenPosition(camera, character.gameObject, screenCentre, screenBounds);
        gameObject.transform.position = vector3;
        Vector3 viewPosPlayer = camera.WorldToViewportPoint(GameManager.Instance.PlayerTF().position);
        Vector3 viewPos = camera.WorldToViewportPoint(character.gameObject.transform.position);
        Vector2 A = new Vector2(viewPos.x, viewPos.y);
        Vector2 B = new Vector2(viewPosPlayer.x, viewPosPlayer.y);

        float angle1 = Constant.GetAngleTwoVector2(A, B, viewPos.z);
        SetRotation(Quaternion.Euler(0, 0, angle1));
        SetTextRotation(Quaternion.identity);
        Show();
    }
    private void ChangeColor(RawImage a_obj, ColorType colorType)
    {
        this.colorType = colorType;
        a_obj.color = colorData.GetMat(colorType).color;
    }
    private void SetTextRotation(Quaternion rotation) 
    {
        textLevel.transform.rotation = rotation;
    }
    private void SetRotation(Quaternion rotation)
    {
        gameObject.transform.rotation = rotation;
    }
    public void Show()
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
