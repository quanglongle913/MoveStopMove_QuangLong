using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> Levels = new List<Level>();

    public List<Level> Levels1 { get => Levels; set => Levels = value; }
}
[System.Serializable]
public class Level
{
    [SerializeField] string levelName;
    [SerializeField] int inGameExp;

}
