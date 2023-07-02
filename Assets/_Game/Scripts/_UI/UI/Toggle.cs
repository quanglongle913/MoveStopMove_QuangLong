using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    [SerializeField] private Image imageTogge;
    [SerializeField] private Image imageHandle;
    [SerializeField] private Transform handleON, handleOFF;
    [SerializeField] private bool isSound;
    [SerializeField] private bool isToggeleON;
 
    private void Start()
    {
        if (isSound)
        {
            if (PlayerPrefs.GetInt(Constant.SOUND_TOGGLE_STATE, 0) == 0) //default toggle is ON
            {
                SetHandleON(0.01f);
            }
            else
            {
                SetHandleOFF(0.01f);
            }
        }
        else
        {
            if (PlayerPrefs.GetInt(Constant.VIBRATION_TOGGLE_STATE, 0) == 0) //default toggle is ON
            {
                SetHandleON(0.01f);
            }
            else
            {
                SetHandleOFF(0.01f);
            }
        }
        
    }
    public void OnSoundToggleClicked()
    {
        if (isToggeleON)
        {
            SetHandleOFF(0.3f);
            GameManager.Instance.SoundManager().SetSoundOFF();
            PlayerPrefs.SetInt(Constant.SOUND_TOGGLE_STATE, 1);
            PlayerPrefs.Save();
        }
        else 
        {
            SetHandleON(0.3f);
            GameManager.Instance.SoundManager().SetSoundON();
            PlayerPrefs.SetInt(Constant.SOUND_TOGGLE_STATE, 0);
            PlayerPrefs.Save();
        }
    }
    public void OnVibrationToggleClicked()
    {
        if (isToggeleON)
        {
            SetHandleOFF(0.3f);
            PlayerPrefs.SetInt(Constant.VIBRATION_TOGGLE_STATE, 1);
            PlayerPrefs.Save();
            //Set Vibration
        }
        else
        {
            SetHandleON(0.3f);
            PlayerPrefs.SetInt(Constant.VIBRATION_TOGGLE_STATE, 0);
            PlayerPrefs.Save();
        }
    }
    private void SetHandleON(float speed)
    {
        imageTogge.color = Color.white;
        imageHandle.transform.DOMoveX(handleON.position.x, speed).SetEase(Ease.Flash);
        isToggeleON = true;
        
    }
    private void SetHandleOFF(float speed)
    {
        imageTogge.color = new Color(1,1,1,0.2f);
        imageHandle.transform.DOMoveX(handleOFF.position.x, speed).SetEase(Ease.Flash);
        isToggeleON = false;
    }
}
