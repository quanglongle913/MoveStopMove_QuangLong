using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : MonoBehaviour
{
    private GameManager _GameManager;
    private int randomBuff;
    GameObject newbuffEffectVfx;
    float timer;
    public int RandomBuff { get => randomBuff; set => randomBuff = value; }
    public GameObject NewbuffEffectVfx { get => newbuffEffectVfx; set => newbuffEffectVfx = value; }
    public float Timer { get => timer; set => timer = value; }

    public virtual void Start()
    {
        _GameManager = GameManager.Instance;
        _GameManager.VfxManager.ShowEffectVfxInGiftBox(this);
    }
    private void Update()
    {
        //reset giftbuff effext for 10s
        timer += Time.deltaTime;
        if (timer > 10f)
        {
            _GameManager.VfxManager.ShowEffectVfxInGiftBox(this);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>())
        {
            Character character = other.GetComponent<Character>();
            if (!character.IsBuffed)
            {
                _GameManager.VfxManager.CharacterBufffCountDown(character, randomBuff);
                _GameManager.ListGiftBox.Remove(gameObject.GetComponent<GiftBox>());
                gameObject.GetComponent<PooledObject>().Release();
            }
            
        }
    }
    private void OnDestroy()
    {
        if (newbuffEffectVfx != null)
        { 
            Destroy(newbuffEffectVfx);
        }
    }
}
