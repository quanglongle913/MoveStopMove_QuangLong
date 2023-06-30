using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Constant
{
    public const string LEVEL = "Level";
    public const string SURVIVAL = "Survival";

    public const string PLAYER_DATA_STATE = "PlayerDataSate";
    public const string PLAYER_MAP = "PlayerMap";
    public const string ZOMBIEDAYS = "ZombieDays";

    public const string BEST_RANK = "BestRank";

    public const string POPUP_COUNTINUE_STATUS_LOSE = "Too bad, try again......";
    public const string POPUP_COUNTINUE_STATUS_WIN = "Coming Soon!...";

    public const string PLAYER_COIN = "PlayerCoin";
    public const string PLAYER_NAME = "PlayerName";
    public const string PLAYER_ZONE_EXP = "PlayerZoneExp";
    public const string PLAYER_ZONE_TYPE = "PlayerZoneType";
    public const string PLAYER_ZOMBIEDAY = "PlayerZombieDay";

    public const string GAME_STATE = "GameState";
    public const string GAME_MODE = "GameMode";
    public const string LAYOUT_WALL = "Wall";
    public const string LAYOUT_CHARACTER = "Character";

    //============UI=============
    public const string SOUND_TOGGLE_STATE = "SoundToggleState";
    public const string VIBRATION_TOGGLE_STATE = "VibrationToggleState";
    //===========================

    public const float RAYCAST_HIT_RANGE_WALL = 1.0f;

   



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
    public static float GetAngleTwoVector2(Vector2 A, Vector2 B, float asixZ)
    {
        float angle1 = 0;
        if (asixZ > 0)
        {
            angle1 = Constant.AngleBetween2Vector2Up(B, A);
        }
        else
        {
            angle1 = Constant.AngleBetween2Vector2Up(A, B);
        }
        return angle1;
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

    public static string GetStreamingAssetsPath(string fileName)
    {
        string dbPath;
        
#if UNITY_EDITOR
        dbPath = string.Format(@"Assets/StreamingAssets/{0}", fileName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, fileName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->
#if UNITY_ANDROID 
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + fileName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + fileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	    var loadDb = Application.dataPath + "/StreamingAssets/" + fileName;  // this is the path to your StreamingAssets in iOS
	    // then save to Application.persistentDataPath
	    File.Copy(loadDb, filepath);

#endif
            Debug.Log("Database written");
        }
        dbPath = filepath;
#endif

        return dbPath;
    }
    public class Cache
    {
        static Dictionary<Collider, Character> m_Characters = new Dictionary<Collider, Character>();
        public static Character GetCharacter(Collider collider)
        {
            if (!m_Characters.ContainsKey(collider))
            {
                m_Characters.Add(collider, collider.GetComponent<Character>());
            }
            return m_Characters[collider];
        }
        static Dictionary<GameObject, Character> m_CharactersObj = new Dictionary<GameObject, Character>();
        public static Character GetCharacter(GameObject gameObject)
        {
            if (!m_CharactersObj.ContainsKey(gameObject))
            {
                m_CharactersObj.Add(gameObject, gameObject.GetComponent<Character>());
            }
            return m_CharactersObj[gameObject];
        }
        static Dictionary<Collider, Player> m_Players = new Dictionary<Collider, Player>();
        public static Player GetPlayer(Collider collider)
        {
            if (!m_Players.ContainsKey(collider))
            {
                m_Players.Add(collider, collider.GetComponent<Player>());
            }
            return m_Players[collider];
        }
        static Dictionary<GameObject, Player> m_PlayersObj = new Dictionary<GameObject, Player>();
        public static Player GetPlayer(GameObject gameObject)
        {
            if (!m_PlayersObj.ContainsKey(gameObject))
            {
                m_PlayersObj.Add(gameObject, gameObject.GetComponent<Player>());
            }
            return m_PlayersObj[gameObject];
        }

        static Dictionary<Collider, BotAI> m_BotAIs = new Dictionary<Collider, BotAI>();
        public static BotAI GetBotAI(Collider collider)
        {
            if (!m_BotAIs.ContainsKey(collider))
            {
                m_BotAIs.Add(collider, collider.GetComponent<BotAI>());
            }
            return m_BotAIs[collider];
        }

        static Dictionary<Collider, TransparentObstacle> m_TransparentObstacle = new Dictionary<Collider, TransparentObstacle>();
        public static TransparentObstacle GetTransparentObstacle(Collider collider)
        {
            if (!m_TransparentObstacle.ContainsKey(collider))
            {
                m_TransparentObstacle.Add(collider, collider.GetComponent<TransparentObstacle>());
            }
            return m_TransparentObstacle[collider];
        }
       
    }
}

