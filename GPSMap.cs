using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GPSMap : MonoBehaviour 
{
 
  
    
    public RectTransform Marker;
    public ScrollRect ScrollRectParent;
    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;
  

    Vector2 firstTouchPrevPos, secondTouchPrevPos;

    [SerializeField]
    float zoomModifierSpeed = 0.0001f;

    

    

    void Update()
    {
       
        //checks if two fingers are touching the phone screen
        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            //disables the map marker script so it doesn't move wildly while scaling the map
            transform.GetChild(0).transform.GetComponent<MapMarker>().enabled = false;

            //disables the scroll rect so the map can actually scale and not be stopped by it
            ScrollRectParent.enabled = false;

            //measures the difference between the initial touches and where the fingers have moved
            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            //gets the diference between the touches on the previous frame
            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            //gets the distance between the two current touches
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            //used to determine how much to change the zoom
            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed * 0.01f;
            
            //checks if it is scaling the map down since the distance between touches is decreasing
            if (touchesPrevPosDifference > touchesCurPosDifference)
            {
                if (transform.localScale.x > 1)
                {
                    transform.localScale = new Vector3(transform.localScale.x - zoomModifier, transform.localScale.y - zoomModifier, 0);
                    //scales the marker
                    Marker.sizeDelta = new Vector2(160 - (66 * (transform.localScale.x - 1)), 160 - (66 * (transform.localScale.y - 1)));
                }

                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    //scales the marker
                    Marker.sizeDelta = new Vector2(160 - (66 * (transform.localScale.x - 1)), 160 - (66 * (transform.localScale.y - 1)));
                }
            }

            //checks if it is scaling the map up since the distance between touches is increasing
            if (touchesPrevPosDifference < touchesCurPosDifference)
            {
                //checks if the scale has eached his maximum and stops the scaling once it does
                if (transform.localScale.x <= 2.5f)
                {
                    transform.localScale = new Vector3(transform.localScale.x + zoomModifier, transform.localScale.y + zoomModifier, 0);
                    //scales the marker
                    Marker.sizeDelta = new Vector2(160 - (66 * (transform.localScale.x - 1)), 160 - (66 * (transform.localScale.y - 1)));

                }

                //makes sure the scale doesn't go below the minimum size
                if (transform.localScale.x < 1)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    //scales the marker
                    Marker.sizeDelta = new Vector2(160 - (66 * (transform.localScale.x - 1)), 160 - (66 * (transform.localScale.y - 1)));
                }
            }

        }

        else
        {
            //enables the marker script
            transform.GetChild(0).transform.GetComponent<MapMarker>().enabled = true;
            ScrollRectParent.enabled = true;
        }

        
    }




    

  

   

  



    
}
