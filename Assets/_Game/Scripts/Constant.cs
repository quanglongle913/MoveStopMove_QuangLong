using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{

  
    public static string PLAYER_MAP = "PlayerMap";
    public static string ZOMBIEDAYS = "ZombieDays";

    public static string BEST_RANK = "BestRank";

    public static string POPUP_COUNTINUE_STATUS_LOSE = "Too bad, try again......";
    public static string POPUP_COUNTINUE_STATUS_WIN = "Coming Soon!...";

    public static string PLAYER_COIN = "PlayerCoin";
    public static string PLAYER_NAME = "PlayerName";
    public static string PLAYER_ZOMBIEDAY = "PlayerZombieDay";

    public static string GAME_STATE = "GameState";
    public static string GAME_MODE = "GameMode";
    public static string LAYOUT_WALL = "Wall";
    public static string LAYOUT_CHARACTER = "Character";

    //============UI=============
    public static string SOUND_TOGGLE_STATE = "SoundToggleState";
    public static string VIBRATION_TOGGLE_STATE = "VibrationToggleState";
    //===========================

    public static float RAYCAST_HIT_RANGE_WALL = 1.0f;

   



    public static float AngleBetween2Vector2right(Vector2 pos, Vector2 target)
    {
        if (target.y < pos.y)
            return Vector2.Angle(Vector2.right, target - pos) * -1.0f;
        else
            return Vector2.Angle(Vector2.right, target - pos);
    }
    public static float AngleBetween2Vector2Up(Vector2 pos, Vector2 target)
    {
        if (target.y < pos.y)
            return Vector2.Angle(Vector2.right, target - pos) * -1.0f - 90;
        else
            return Vector2.Angle(Vector2.right, target - pos) - 90;
    }
    public static bool isWall(GameObject a, LayerMask _layerMask)
    {
        RaycastHit hit;
        bool isWall = false;
        if (Physics.Raycast(a.transform.position, a.transform.TransformDirection(Vector3.forward), out hit, Constant.RAYCAST_HIT_RANGE_WALL, _layerMask))
        {
            isWall = true;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
        }
        else
        {
            isWall = false;
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
        return isWall;
    }
    public static bool IsDes(Vector3 a, Vector3 b, float range)
    {

        float distance = Vector3.Distance(a, b);
        return distance < range;
    }
}
