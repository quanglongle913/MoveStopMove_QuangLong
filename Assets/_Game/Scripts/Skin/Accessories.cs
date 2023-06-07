using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Accessories", menuName = "ScriptableObjects/Accessories", order = 1)]
public class Accessories : ScriptableObject
{
    [SerializeField] private string accessoriesName;
    [SerializeField] private GameObject prefabsAccessories;
    [SerializeField] private Texture imageItem;
    [SerializeField] private Material mat;
    [SerializeField] private BuffData buffData;
    [SerializeField] private bool selected=false;
    [SerializeField] private bool buyed = false;

    public string AccessoriesName { get => accessoriesName; set => accessoriesName = value; }
    public Material Mat { get => mat; set => mat = value; }
    public BuffData BuffData { get => buffData; set => buffData = value; }
    public GameObject PrefabsAccessories { get => prefabsAccessories; set => prefabsAccessories = value; }
    public bool Selected { get => selected; set => selected = value; }
    public bool Buyed { get => buyed; set => buyed = value; }
    public Texture ImageItem { get => imageItem; set => imageItem = value; }
}
