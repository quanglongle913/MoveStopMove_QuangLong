using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Accessories", menuName = "ScriptableObjects/Accessories", order = 1)]
public class Accessories : ScriptableObject
{
    [SerializeField] private string accessoriesName;
    [SerializeField] private GameObject prefabsAccessories;
    [SerializeField] private Material mat;
    [SerializeField] private BuffData buffData;

    public string AccessoriesName { get => accessoriesName; set => accessoriesName = value; }
    public Material Mat { get => mat; set => mat = value; }
    public BuffData BuffData { get => buffData; set => buffData = value; }
    public GameObject PrefabsAccessories { get => prefabsAccessories; set => prefabsAccessories = value; }
}
