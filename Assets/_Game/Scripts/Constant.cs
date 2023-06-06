using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{

   
    public static string WEAPONS = "Weapons";
    public static string PLAYER_MAP = "PlayerMap";
    public static string ZOMBIEDAYS = "ZombieDays";

    public static string PLAYER_EXP = "PlayerExp";
    public static string PLAYER_ZONE = "PlayerZone";
    public static string PLAYER_BEST = "PlayerBest";
    public static string PLAYER_COIN = "PlayerCoin";
    public static string PLAYER_NAME = "PlayerName";
    public static string PLAYER_ZOMBIEDAY = "PlayerZombieDay";

    public static string GAME_STATE = "GameState";
    public static string LAYOUT_WALL = "Wall";
    public static string LAYOUT_CHARACTER = "Character";


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
