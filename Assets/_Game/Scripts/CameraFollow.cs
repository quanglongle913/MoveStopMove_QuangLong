using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] private float xAxis, yAxis = 21f, zAxis = -21f, axisX, axisY, axisZ;

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsState(GameState.GameMenu))
        {
            Quaternion target = Quaternion.Euler(20, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 1000);
            transform.position = new Vector3(player.transform.position.x + xAxis, player.transform.position.y + 4.2f, player.transform.position.z - 10.0f);

            Vector3 _Direction = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            player.GetComponent<Player>().RotateTowards(player.gameObject,_Direction);

        } else if (GameManager.Instance.IsState(GameState.InGame))
        {
            Quaternion target = Quaternion.Euler(45, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 1000);
            float size = player.GetComponent<Character>().InGameSizeCharacter;
            transform.position = new Vector3(player.transform.position.x + xAxis, player.transform.position.y + yAxis * size, player.transform.position.z + zAxis * size);
        }
        else if (GameManager.Instance.IsState(GameState.EndGame))
        {
            Quaternion target = Quaternion.Euler(45, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 1000);
            float size = player.GetComponent<Character>().InGameSizeCharacter;
            transform.position = new Vector3(player.transform.position.x + xAxis, player.transform.position.y + yAxis * size, player.transform.position.z + zAxis * size);
        }
        else if (GameManager.Instance.IsState(GameState.SkinShop))
        {
           Quaternion target = Quaternion.Euler(20, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 1000);
            transform.position = new Vector3(player.transform.position.x + xAxis, player.transform.position.y + 1.5f, player.transform.position.z - 7f);

            Vector3 _Direction = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            player.GetComponent<Player>().RotateTowards(player.gameObject,_Direction); 
        }
     }
}