using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : MonoBehaviour
{
    [SerializeField] private List<BuffData> buffData;
    [SerializeField] private List<GameObject> buffEffectVfx;
    private GameManager _GameManager;
    [SerializeField] private int randomBuff;
    GameObject newbuffEffectVfx;
    public List<BuffData> BuffData { get => buffData; set => buffData = value; }
    public virtual void Start()
    {
        _GameManager = GameManager.Instance;
        randomBuff = Random.Range(0, buffData.Count);
        newbuffEffectVfx = Instantiate(buffEffectVfx[randomBuff], gameObject.transform.position, gameObject.transform.rotation);
        newbuffEffectVfx.GetComponent<ParticleSystem>().Play();
    }
    private void FixedUpdate()
    {
        newbuffEffectVfx.transform.position = gameObject.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character)
        {
            character.BufffCountDown(buffData[randomBuff]);

            _GameManager.ListGiftBox.Remove(gameObject.GetComponent<GiftBox>());
            gameObject.GetComponent<PooledObject>().Release();
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
