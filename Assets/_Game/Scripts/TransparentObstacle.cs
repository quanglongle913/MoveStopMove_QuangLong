using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransparentObstacle : MonoBehaviour
{
    //Variables
    [SerializeField] private Renderer rend;
    Color[] newMaterialColor;
    private bool transparent = false;
    private void Start()
    {
        //Get the material color
        newMaterialColor = new Color[rend.materials.Count()];
        for (int i = 0; i < newMaterialColor.Count(); i++)
        {
            newMaterialColor[i] = rend.materials[i].color;
        
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

        //Set the new Color
        for (int i = 0; i < rend.materials.Count(); i++)
        {
            rend.materials[i].color = newMaterialColor[i];
        }
    }
}
