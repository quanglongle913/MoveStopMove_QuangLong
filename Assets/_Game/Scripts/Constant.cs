using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
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
}
