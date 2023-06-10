using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : MonoBehaviour
{
    [SerializeField] private List<BuffData> buffData;
    private GameManager _GameManager;
    [SerializeField] private int randomBuff;
    public List<BuffData> BuffData { get => buffData; set => buffData = value; }
    public virtual void Start()
    {
        _GameManager = GameManager.Instance;
        //buffData= new List<BuffData>();
        randomBuff = Random.Range(0, buffData.Count);
        //Random buff effect
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
}
