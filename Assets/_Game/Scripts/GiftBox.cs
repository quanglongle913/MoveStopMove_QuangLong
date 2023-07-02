using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : GameUnit
{
    [SerializeField] private List<BuffData> buffDataInGiftBox;
    private GameManager _GameManager;
    private int randomBuff;
    private int bufftype;
    private ParticleSystem newbuffEffectVfx;
    private float timer;
    public int RandomBuff { get => randomBuff; set => randomBuff = value; }
    public float Timer { get => timer; set => timer = value; }

    public virtual void Start()
    {
        _GameManager = GameManager.Instance;
        ShowEffectVfxInGiftBox();
        ParticlePool.Play(ParticleType.Hit, transform.position, Quaternion.identity);
    }
    private void Update()
    {
        //reset giftbuff effext for 10s
        timer += Time.deltaTime;
        if (timer > 10f)
        {
            ShowEffectVfxInGiftBox();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>())
        {
            Character character = Constant.Cache.GetCharacter(other);
            if (!character.IsBuffed)
            {
                character.CharacterBufffCountDown(RandomBuff, buffDataInGiftBox);
                GameManager.Instance.LevelManager().GiftBoxs().Remove(this);
                Destroy(gameObject);
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

    public override void OnInit()
    {
        
    }

    public override void OnDespawn()
    {
       
    }
    public void ShowEffectVfxInGiftBox()
    {
        if (newbuffEffectVfx != null)
        {
            Destroy(newbuffEffectVfx.gameObject);
        }
        int randomNumber = Random.Range(0, buffDataInGiftBox.Count);
        RandomBuff = randomNumber;
        bufftype = (int)buffDataInGiftBox[randomNumber].BuffType;
        bufftype+= (int)ParticleType.ChargeBlue;
        newbuffEffectVfx = Instantiate(ParticlePool.ParticleSystem((ParticleType)bufftype), gameObject.transform.position, gameObject.transform.rotation);
        newbuffEffectVfx.transform.parent = gameObject.transform;
        newbuffEffectVfx.Play();
        Timer = 0;
    }
}
