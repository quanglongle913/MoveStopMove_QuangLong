using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AccessoriesData", menuName = "ScriptableObjects/AccessoriesData", order = 1)]
public class AccessoriesData : ScriptableObject
{
    [SerializeField] private SkinType skinType;
    [SerializeField] private List<Accessories> accessories;

    public SkinType SkinType { get => skinType; set => skinType = value; }
    public List<Accessories> Accessories { get => accessories; set => accessories = value; }
}
