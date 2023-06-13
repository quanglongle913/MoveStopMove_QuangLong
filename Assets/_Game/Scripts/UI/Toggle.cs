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
    public GameManager gameManager;
 
    private void Start()
    {
        gameManager= GameManager.Instance;
        if (isSound)
        {
            if (PlayerPrefs.GetInt(Constant.SOUND_TOGGLE_STATE, 0) == 0) //default toggle is ON
            {
                SetHandleON();
                gameManager.SoundManager.SetSoundON();
                //Debug.Log("SetHandleON");
            }
            else
            {
                SetHandleOFF();
                gameManager.SoundManager.SetSoundOFF();
                //Debug.Log("SetHandleOFF");
            }
        }
        else
        {
            if (PlayerPrefs.GetInt(Constant.VIBRATION_TOGGLE_STATE, 0) == 0) //default toggle is ON
            {
                SetHandleON();
            }
            else
            {
                SetHandleOFF();
            }
        }
        
    }
    public void OnSoundToggleClicked()
    {
        if (isToggeleON)
        {
            SetHandleOFF();
            gameManager.SoundManager.SetSoundOFF();
            PlayerPrefs.SetInt(Constant.SOUND_TOGGLE_STATE, 1);
            PlayerPrefs.Save();
        }
        else 
        {
            SetHandleON();
            gameManager.SoundManager.SetSoundON();
            PlayerPrefs.SetInt(Constant.SOUND_TOGGLE_STATE, 0);
            PlayerPrefs.Save();
        }
    }
    public void OnVibrationToggleClicked()
    {
        if (isToggeleON)
        {
            SetHandleOFF();
            PlayerPrefs.SetInt(Constant.VIBRATION_TOGGLE_STATE, 1);
            PlayerPrefs.Save();
            //Set Vibration
        }
        else
        {
            SetHandleON();
            PlayerPrefs.SetInt(Constant.VIBRATION_TOGGLE_STATE, 0);
            PlayerPrefs.Save();
        }
    }
    private void SetHandleON()
    {
        imageTogge.color = Color.white;
        imageHandle.transform.DOMoveX(handleON.position.x, 0.5f).SetEase(Ease.Flash);
        isToggeleON = true;
        
    }
    private void SetHandleOFF()
    {
        imageTogge.color = new Color(1,1,1,0.2f);
        imageHandle.transform.DOMoveX(handleOFF.position.x, 0.5f).SetEase(Ease.Flash);
        isToggeleON = false;
    }
}
