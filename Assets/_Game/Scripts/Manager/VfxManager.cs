using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    [SerializeField] private GameManager _GameManager;
    [Header("-------------InGiftBox Vfx-------------- ")]
    [SerializeField] private List<BuffData> buffDataInGiftBox;
    [SerializeField] private List<GameObject> buffEffectVfxInGiftBox;
    [SerializeField] private List<GameObject> BuffVfxInCharacter;
    [Header("-------------Weapon Vfx-------------- ")]
    [SerializeField] private GameObject bloodVfx;
    [SerializeField] private GameObject FireVfx;

    //==================Weapon OnHit VFX=========================
    public void GenerateFireVfx(Weapons root)
    {
        root.NewFireVfx = Instantiate(FireVfx);
        root.NewFireVfx.transform.parent = root.gameObject.transform;
        root.NewFireVfx.transform.localPosition = Vector3.zero;
        root.NewFireVfx.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        root.NewFireVfx.SetActive(false);
    }

    public void ShowBloodVfx(Weapons root)
    {

        GameObject newBloodVfx = Instantiate(bloodVfx, root.gameObject.transform.position, root.gameObject.transform.rotation);
        newBloodVfx.GetComponent<ParticleSystem>().Play();
        StartCoroutine(BloodVfx(newBloodVfx));
    }
    IEnumerator BloodVfx(GameObject newBloodVfx)
    {
        //Debug.Log("1" + newBloodVfx.name);
        yield return new WaitForSeconds(1.5f);
        //Debug.Log("Destroyed:" + newBloodVfx.name);
        Destroy(newBloodVfx);
    }
    //==================InGiftBox VFX=========================
    public void ShowEffectVfxInGiftBox(GiftBox giftBox)
    {
        if (giftBox.NewbuffEffectVfx!=null)
        {
            Destroy(giftBox.NewbuffEffectVfx);
        }
        int randomNumber = Random.Range(0, buffDataInGiftBox.Count);
        giftBox.RandomBuff = randomNumber;
        giftBox.NewbuffEffectVfx = Instantiate(buffEffectVfxInGiftBox[(int)buffDataInGiftBox[randomNumber].BuffType], giftBox.gameObject.transform.position, giftBox.gameObject.transform.rotation);
        giftBox.NewbuffEffectVfx.transform.parent = giftBox.gameObject.transform;
        giftBox.NewbuffEffectVfx.GetComponent<ParticleSystem>().Play();
        giftBox.Timer = 0;
    }
    public void CharacterBufffCountDown(Character character, int randomBuff)
    {
        if (!character.IsBuffed)
        {
            if (character.InCamera(_GameManager.MainCam))
            {
                _GameManager.SoundManager.PlaySizeUpSoundEffect();
            }
            if (buffDataInGiftBox[randomBuff].BuffType == BuffType.AttackSpeed)
            {
                StartCoroutine(Waiter(character.InGameAttackSpeed, buffDataInGiftBox[randomBuff], character));
                character.InGameAttackSpeed = character.InGameAttackSpeed + (character.InGameAttackSpeed * buffDataInGiftBox[randomBuff].BuffIndex / 100);
            }
            if (buffDataInGiftBox[randomBuff].BuffType == BuffType.MoveSpeed)
            {
                StartCoroutine(Waiter(character.InGameMoveSpeed, buffDataInGiftBox[randomBuff], character));
                character.InGameMoveSpeed = character.InGameMoveSpeed + (character.InGameMoveSpeed * buffDataInGiftBox[randomBuff].BuffIndex / 100);
            }
            if (buffDataInGiftBox[randomBuff].BuffType == BuffType.Range)
            {
                StartCoroutine(Waiter(character.InGameAttackRange, buffDataInGiftBox[randomBuff], character));
                character.InGameAttackRange = character.InGameAttackRange + (character.InGameAttackRange * buffDataInGiftBox[randomBuff].BuffIndex / 100);
            }
        }
        
    }
    IEnumerator Waiter(float indexType, BuffData buffData, Character character)
    {
        float backUp = indexType;
        GameObject newBuffVfx = Instantiate(BuffVfxInCharacter[(int)buffData.BuffType], character.gameObject.transform.position, character.gameObject.transform.rotation);
        newBuffVfx.transform.parent = character.gameObject.transform;
        newBuffVfx.GetComponent<ParticleSystem>().Play();
        character.IsBuffed=true;
        yield return new WaitForSeconds(3f);
        if (buffData.BuffType == BuffType.AttackSpeed)
        {
            character.InGameAttackSpeed = backUp;
            Destroy(newBuffVfx);

        }
        if (buffData.BuffType == BuffType.MoveSpeed)
        {
            character.InGameMoveSpeed = backUp;
            Destroy(newBuffVfx);
        }
        if (buffData.BuffType == BuffType.Range)
        {
            character.InGameAttackRange = backUp;
            Destroy(newBuffVfx);
        }
        character.IsBuffed = false;
    }
}
