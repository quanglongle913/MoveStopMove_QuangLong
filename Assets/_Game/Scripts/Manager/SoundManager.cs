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
    public List<AudioSource> WeaponThrowSoundEffect { get => weaponThrowSoundEffect; set => weaponThrowSoundEffect = value; }
    public List<AudioSource> WeaponHitSoundEffect { get => weaponHitSoundEffect; set => weaponHitSoundEffect = value; }
    public List<AudioSource> CountDownSoundEffect { get => countDownSoundEffect; set => countDownSoundEffect = value; }
    public List<AudioSource> DeadSoundEffect { get => deadSoundEffect; set => deadSoundEffect = value; }
    public List<AudioSource> SizeUpSoundEffect { get => sizeUpSoundEffect; set => sizeUpSoundEffect = value; }
    public AudioSource BtnClickSoundEffect { get => btnClickSoundEffect; set => btnClickSoundEffect = value; }
    public AudioSource EndWinSoundEffect { get => endWinSoundEffect; set => endWinSoundEffect = value; }
    public AudioSource LoseSoundEffect { get => loseSoundEffect; set => loseSoundEffect = value; }

    private void Start()
    {
        audioSource = new List<AudioSource>();
        addSoundEffect(weaponThrowSoundEffect);
        addSoundEffect(weaponHitSoundEffect);
        addSoundEffect(countDownSoundEffect);
        addSoundEffect(deadSoundEffect);
        addSoundEffect(SizeUpSoundEffect);
        audioSource.Add(endWinSoundEffect);
        audioSource.Add(btnClickSoundEffect);
    }
    private void addSoundEffect(List<AudioSource> listSoundEffect)
    {
        for (int i = 0; i < listSoundEffect.Count; i++)
        {
            audioSource.Add(listSoundEffect[i]);
        }
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
}
