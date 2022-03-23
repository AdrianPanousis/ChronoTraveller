using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterControl : MonoBehaviour
{
    //stores the values for the water material
    private Material WaterMaterial;
    //used to chnage the offset of the texture
    private float OffsetValue;
    //determines which way the texture will move
    public bool OffsetDirection;
    //controls how fast the water moves
    private float waterSpeed;
    
    void Start()
    {
        WaterMaterial = transform.GetComponent<Renderer>().material;
        OffsetValue = 0;
        waterSpeed = 0.02f;
    }

    
    void Update()
    {
        //changes the texture offset over time
        if (OffsetDirection)
        {
            OffsetValue += Time.deltaTime * waterSpeed;
        }

        else
        {
            OffsetValue -= Time.deltaTime * waterSpeed;
        }

        WaterMaterial.mainTextureOffset = new Vector2(OffsetValue, OffsetValue);
    }
}
