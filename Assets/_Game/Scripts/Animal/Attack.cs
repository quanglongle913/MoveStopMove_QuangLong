using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Attack : MonoBehaviour
{
    [SerializeField] GameObject gameObjectRoot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() && !gameObjectRoot.GetComponent<AnimalAI>().IsDeath)
        {
            //Debug.Log("Attack");
            other.gameObject.GetComponent<Player>().GetComponent<Player>().OnHit(1f);
            other.gameObject.GetComponent<Player>().KilledByName = gameObjectRoot.name;
            other.gameObject.GetComponent<Player>().KillerColorType = ColorType.Red;
            other.gameObject.GetComponent<Player>().SetEndGame();
        }
    }
}
