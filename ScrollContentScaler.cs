using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollContentScaler : MonoBehaviour
{
   
    private RectTransform child;
    void Start()
    {
        child = transform.GetChild(0).GetComponent<RectTransform>();
    }

    
    void Update()
    {
        //automatically changes the size of the viewport for the description text to match the amount of words along with an extra 70 pixels to avoid the final line getting potentially cutoff
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2 (child.sizeDelta.x,child.sizeDelta.y+70); 
    }
}
