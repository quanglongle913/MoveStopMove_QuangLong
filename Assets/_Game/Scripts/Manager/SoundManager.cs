using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSource;
    [SerializeField] private List<AudioSource> weaponThrowSoundEffect;
    [SerializeField] private List<AudioSource> weaponHitSoundEffect;
    [SerializeField] private List<AudioSource> countDownSoundEffect;
    [SerializeField] private List<AudioSource> deadSoundEffect;
    [SerializeField] private List<AudioSource> sizeUpSoundEffect;
    [SerializeField] private AudioSource endWinSoundEffect;
    [SerializeField] private AudioSource btnClickSoundEffect;
    [SerializeField] private AudioSource loseSoundEffect;
    public List<AudioSource> AudioSource { get => audioSource; set => audioSource = value; }
    private void Start()
    {
        audioSource = new List<AudioSource>();
        AddSoundEffect(weaponThrowSoundEffect);
        AddSoundEffect(weaponHitSoundEffect);
        AddSoundEffect(countDownSoundEffect);
        AddSoundEffect(deadSoundEffect);
        AddSoundEffect(sizeUpSoundEffect);
        audioSource.Add(endWinSoundEffect);
        audioSource.Add(btnClickSoundEffect);
        if (PlayerPrefs.GetInt(Constant.SOUND_TOGGLE_STATE, 0) == 0) //default toggle is ON
        {
            SetSoundON();
            //Debug.Log("SetHandleON");
        }
        else
        {
            SetSoundOFF();
            //Debug.Log("SetHandleOFF");
        }
    }
    private void AddSoundEffect(List<AudioSource> listSoundEffect)
    {
        for (int i = 0; i < listSoundEffect.Count; i++)
        {
            audioSource.Add(listSoundEffect[i]);
        }
    }
    public void SetSoundOFF()
    {
        AudioListener.volume = 0;

    }
    public void SetSoundON()
    {
        AudioListener.volume = 1;
    }
    public void OffVolumeCountDownSoundEffect()
    {
        for (int i = 0; i < countDownSoundEffect.Count; i++)
        {
            countDownSoundEffect[i].volume = 0;
        }
    }
    public void OnVolumeCountDownSoundEffect()
    {
        for (int i = 0; i < countDownSoundEffect.Count; i++)
        {
            countDownSoundEffect[i].volume = 1;
        }
    }
    public void PlaySoundBtnClick()
    {
        btnClickSoundEffect.Play();
    }
    public void PlayWeaponThrowSoundEffect()
    {
        int randomNum = UnityEngine.Random.Range(0, weaponThrowSoundEffect.Count);
        weaponThrowSoundEffect[randomNum].Play();
    }
    public void PlayDeadSoundEffect()
    {
        int randomNum = UnityEngine.Random.Range(0, deadSoundEffect.Count);
        deadSoundEffect[randomNum].Play();
    }
    public void PlayWeaponHitSoundEffect()
    {
        int randomNum = UnityEngine.Random.Range(0, weaponHitSoundEffect.Count);
        weaponHitSoundEffect[randomNum].Play();
    }
    public void PlaySizeUpSoundEffect()
    {
        sizeUpSoundEffect[4].Play();
    }
    public void PlayEndWinSoundEffect()
    {
        endWinSoundEffect.Play();
    }
    public void PlayLoseSoundEffect()
    {
        loseSoundEffect.Play();
    }
    public void PlayCountDownSoundEffect(int index)
    {
        countDownSoundEffect[index].Play();
    }
}
