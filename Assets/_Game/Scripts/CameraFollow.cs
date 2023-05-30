using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] private float xAxis, yAxis, zAxis;
    BotAIManager botAIManager;
    [SerializeField] Camera camera;
    private void Start()
    {
        botAIManager = BotAIManager.instance;
    }
    private void FixedUpdate()
    {
        Quaternion target = Quaternion.Euler(45, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 1000);
        transform.position = new Vector3(player.transform.position.x + xAxis, player.transform.position.y + yAxis, player.transform.position.z + zAxis);

        //Bot detection in camera
        if (camera != null)
        {
            for (int i =0; i<botAIManager.botAIList.Count;i++)
            {
                Vector3 viewPos = camera.WorldToViewportPoint(botAIManager.botAIList[i].gameObject.transform.position);
                if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
                {
                    // Your object is in the range of the camera, you can apply your behaviour
                    Debug.Log("Bot detection in sight");
                }
            }
            
        }
    }
}
