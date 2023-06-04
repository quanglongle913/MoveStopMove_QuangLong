using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    [SerializeField] private string levelName;
    [SerializeField] private Texture texture;
    [SerializeField] private int levelExp;

    public string LevelName { get => levelName; set => levelName = value; }
    public Texture Texture { get => texture; set => texture = value; }
    public int LevelExp { get => levelExp; set => levelExp = value; }
}
