using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UnityEngine.EventSystems;

public class MarkerMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //stores the original mouse position
    private Vector2 lastMousePosition;
    //stores the map marker
    public MapMarker Marker;
    //stores the move player button's object, button valeu and image
    public GameObject movePlayerObject;
    private Button movePlayerButton;
    private Image movePlayerImage;
    //stores the data for the zones
    public GameObject[] dropZone;
    public GameObject[] noGoZone;
    //stores values, either a 0  or a 1, for each zone determining whether it is empty or not
    private int[] emptyDropZones;
    private int[] emptyNoGoZones;
    //stores whether the object is dragging or not
    private bool dragging;

    
    private void Start()
    {
        //determines the size of the int arrays
        emptyDropZones = new int[dropZone.Length];
        emptyNoGoZones = new int[noGoZone.Length];

        //gets the components of the move player button
        movePlayerButton = movePlayerObject.transform.GetComponent<Button>();
        movePlayerImage = movePlayerObject.transform.GetComponent<Image>();

        //delays so the value is set properly
        StartCoroutine(DelayTimer());
        
        dragging = false;
    }

    private IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(0.5f);
        

    }

    //executes when the user touches the marker
    public void OnBeginDrag(PointerEventData eventData)
    {
        //stores the original mouse position
        lastMousePosition = eventData.position;
        dragging = true;
        
        
    }

    public bool isDragging()
    {
        return dragging;
    }

    //executes while dragging the marker
    public void OnDrag(PointerEventData eventData)
    {
        //gets the current position of thge drag
        Vector2 currentMousePosition = eventData.position;
        //gets the different between the original position and the current one
        Vector2 diff = currentMousePosition - lastMousePosition;
        //gets the transform
        RectTransform rect = transform.GetComponent<RectTransform>();
        
        //calculates and gets the new position
        Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
        //stgores the original position
        Vector3 oldPos = rect.position;
        //changes the position
        rect.position = newPosition;
       

        
        //checks if the marker is inside a no go zone and stops it
        for (int i = 0; i < noGoZone.Length; i++)
        {
            if (isRectInsideDropPoint(noGoZone[i].transform.GetComponent<RectTransform>()))
            {
                //checks if there is collision and reverts it back to the original position if there is
                if (noGoZone[i].transform.GetComponent<NoGoZone>().hasCollision)
                {
                    rect.position = oldPos;
                    emptyNoGoZones[i] = 1;
                }
                //doesn't change the position but turns off interactivity of the move player button so the user can't transport to that point
                else
                {
                    movePlayerButton.interactable = false;
                    //changes the color so it is more faded to signify it can't be tapped
                    movePlayerImage.color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
                    emptyNoGoZones[i] = 0;
                    
                }
            }

            //says the NoGoZone is empty
            else
            {
                emptyNoGoZones[i] = 1;
                
            }

            //checks if all of the NoGoZones are empty. If all of the 1s add up to the same amount of NoGoZones in the scene it means they are all empty
            if (DropzoneCheck(noGoZone, emptyNoGoZones) == noGoZone.Length)
            {
                movePlayerButton.interactable = true;
                movePlayerImage.color = new Color(1, 1, 1, 1);

            }
        }

            

        //checks if marker is inside a dropzone and changes the placement height
        for (int i = 0; i < dropZone.Length; i++)
        {
            //checks if the marker is inside a dropzone and changes the placement height to that zones value
            if (isRectInsideDropPoint(dropZone[i].transform.GetComponent<RectTransform>()))
            {
                Marker.setPlacementHeight(dropZone[i].transform.GetComponent<Dropzone>().placementHeight);
                emptyDropZones[i] = 0;

            }

            //says the dropzone is empty
            else
            {
                emptyDropZones[i] = 1;
            }

            //checks if the dropzones are empty and changes the placement height to the default one. 
            if(DropzoneCheck(dropZone,emptyDropZones) == dropZone.Length)
            {
                Marker.SetDefaultPlacementHeight();
                
            }

        }

        //print("Is inside the drop zone" + isRectInsideDropPoint(dropZone.transform.GetComponent<RectTransform>()));
        lastMousePosition = currentMousePosition;
    }

    //the NoGoZones will be set to 1 if empty and 0 if the marker is in them. This function adds all of the 0s and 1s together and returns the total value
    private int DropzoneCheck(GameObject[] DZone,int[] empty)
    {
        int emptyZones = 0;
        for(int i = 0; i<DZone.Length;i++)
        {
            emptyZones += empty[i];
        }
        return emptyZones;
    }

    //executes when the user releases their drag
    public void OnEndDrag(PointerEventData eventData)
    {
        
        dragging = false;
        //gets the values of the bounds of the map
        Vector2 MaxBounds = BoundCalculator(true);
        Vector2 MinBounds = BoundCalculator(false);
       
        //gets the current position
        RectTransform curentPos = transform.GetComponent<RectTransform>();
        //gets the transform of the map
        RectTransform Map = transform.parent.GetComponent<RectTransform>();

        //checks of the marker is outside X values the map 
        if (curentPos.anchoredPosition.x > MaxBounds.x | curentPos.anchoredPosition.x < MinBounds.x)
        {
            //gets the values for the centre of the map
            float adjustedMapPositionX = (Map.anchoredPosition.x / 2) * -1;
            float adjustedMapPositionY = (Map.anchoredPosition.y / 2) * -1;

            //places the marker in the centre of the map
            curentPos.anchoredPosition = new Vector2(adjustedMapPositionX, adjustedMapPositionY);
        }

        //checks of the marker is outside Y values the map 
        if (curentPos.anchoredPosition.y > MaxBounds.y | curentPos.anchoredPosition.y < MinBounds.y)
        {
            //gets the values for the centre of the map
            float adjustedMapPositionX = (Map.anchoredPosition.x / 2) * -1;
            float adjustedMapPositionY = (Map.anchoredPosition.y / 2) * -1;

            //places the marker in the centre of the map
            curentPos.anchoredPosition = new Vector2(adjustedMapPositionX, adjustedMapPositionY);
        }
        
    }

    //calculates the bounds and returns them
    private Vector2 BoundCalculator(bool isMax)
    {
        //gets the transform of the map
        RectTransform Map = transform.parent.GetComponent<RectTransform>();
        //gets the default values regardless of scale and position
        float DefaultMaxX = Map.sizeDelta.x / 2;
        float DefaultMaxY = Map.sizeDelta.y / 2;

        //stores the values of the bounds
        float NewMaxX = 0;
        float NewMaxY = 0;

        //gets the maximum bounds
        if (isMax)
        {
            //uses the anchored position and the scale for an accurate number of the maximum bounds
            NewMaxX = ((Map.anchoredPosition.x / 2) * -1) + (DefaultMaxX / Map.localScale.x);
            NewMaxY = ((Map.anchoredPosition.y / 2) * -1) + (DefaultMaxY / Map.localScale.y);
        }

        //gets the minimum bounds
        else
        {
            //uses the anchored position and the scale for an accurate number of the minimum bounds
            NewMaxX = ((Map.anchoredPosition.x / 2) * -1) - (DefaultMaxX / Map.localScale.x);
            NewMaxY = ((Map.anchoredPosition.y / 2) * -1) - (DefaultMaxY / Map.localScale.y);
        }

        //sets the new bounds
        Vector2 bounds = new Vector2(NewMaxX,NewMaxY);
        
        return bounds;


    }


    private bool isRectInsideDropPoint(RectTransform rectTransform)
    {
        //gets the transform of the marker
        RectTransform rect = GetComponent<RectTransform>();
        //stores the four corners of the dropzone
        Vector3[] otherCorners = new Vector3[4];
        //sets the corners to the real world position of the corners of the Dropzone/NoGoZone
        rectTransform.GetWorldCorners(otherCorners);
       
        //gets the length of the sides of the square the marker is in
        float bax = otherCorners[1].x - otherCorners[0].x;
        float bay = otherCorners[1].y - otherCorners[0].y;
        float dax = otherCorners[3].x - otherCorners[0].x;
        float day = otherCorners[3].y - otherCorners[0].y;

        //when inside the drop point, from the marker their will be four triangles if you have lines from each corner end at the marker. Those four triangles will add up to the area
        //of the square drop zone, but if the marker is outside the drop zone the triangles will add up to a value greater than the square. If the area is the same, it returns true. If not, false.
        if ((rect.position.x - otherCorners[0].x) * bax + (rect.position.y - otherCorners[0].y) * bay < 0.0) return false;
        if ((rect.position.x - otherCorners[1].x) * bax + (rect.position.y - otherCorners[1].y) * bay > 0.0) return false;
        if ((rect.position.x - otherCorners[0].x) * dax + (rect.position.y - otherCorners[0].y) * day < 0.0) return false;
        if ((rect.position.x - otherCorners[3].x) * dax + (rect.position.y - otherCorners[3].y) * day > 0.0) return false;

        return true;

        
    }

}
