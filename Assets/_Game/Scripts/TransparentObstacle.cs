using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransparentObstacle : MonoBehaviour
{
    //Variables
    [SerializeField] private Renderer rend;
    [SerializeField] private List<Material> materials;
    Color[] newMaterialColor;
    private bool transparent = false;
    //Material[] newMaterials;
    private void Start()
    {
        //Get the material color
        newMaterialColor = new Color[rend.materials.Count()];
        for (int i = 0; i < newMaterialColor.Count(); i++)
        {
            newMaterialColor[i] = rend.materials[i].color;
            //newMaterials[i] = rend.materials[i];
        }
      
    }

    public void ChangeTransparency(bool transparent)
    {
        //Avoid to set the same transparency twice
        if (this.transparent == transparent) return;

        //Set the new configuration
        this.transparent = transparent;

        //Check if should be transparent or not
        if (transparent)
        {
            //Change the alpha of the color
            for (int i = 0; i < newMaterialColor.Count(); i++)
            {
                newMaterialColor[i].a = 0.3f;
                //newMaterials[i] = materials[0];
            }
        }
        else
        {
            //Change the alpha of the color
            for (int i = 0; i < newMaterialColor.Count(); i++)
            {
                newMaterialColor[i].a =1.0f;
            }
        }
        
       
    }
    //public void ChangeTransparency(bool transparent)
    //{
    //    //Avoid to set the same transparency twice
    //    if (this.transparent == transparent) return;

    //    //Set the new configuration
    //    this.transparent = transparent;

    //    //Check if should be transparent or not
    //    if (transparent)
    //    {
    //        //Change the alpha of the color
    //        for (int i = 0; i < newMaterialColor.Count(); i++)
    //        {
    //            newMaterialColor[i].a = 0.3f;
    //            //newMaterials[i] = materials[0];
    //        }
    //    }
    //    else
    //    {
    //        //Change the alpha of the color
    //        for (int i = 0; i < newMaterialColor.Count(); i++)
    //        {
    //            newMaterialColor[i].a = 1.0f;
    //        }
    //    }
    //    //Set the new Color
    //    for (int i = 0; i < rend.materials.Count(); i++)
    //    {
    //        rend.materials[i].color = newMaterialColor[i];
    //    }

    //}
}
