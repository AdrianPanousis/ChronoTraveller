using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapPointButton : MonoBehaviour
{
    //stores the camera of the scene
    public GlobeRotateCamera globeCameraData;
    //stores the menu for the city of the button
    public CitySights chooseArea;

    public GameObject GPSButton;
    //stores the download bars in the menu
    public RemoveLoadingBars loadingBars;

    [SerializeField] private float latitude;
    [SerializeField] private float longitude;

    //stores the scale of the object when it's zoomed in or not
    private Vector3 InitialPointScale;
    private Vector3 ZoomedPointScale;

    //stores the scale of the collider when it's zoomed in or not. Used to detect taps. The zoomed in values are public since some smaller cities might not be visible unless the camera is zoomed in more
    private Vector3 InitialColliderScale;
    public Vector3 ZoomedColliderScale;

    //stores the position of the collider when it's zoomed in or not. Used to detect taps
    private Vector3 InitialColliderPosition;
    public Vector3 ZoomedColliderPosition;

    //stores the position of the canvas when it's zoomed in or not. Used to detect taps
    private Vector3 InitialCanvasPosition;
    public Vector3 ZoomedCanvasPosition;

    private BoxCollider hitBox;
    
    //stores the text of the button
    public GuiFadeInFadeOut Text;
    public GuiFadeInFadeOut ShadowText;

    //stores how zoomed in the camera needs to be to show the button
    public float camDistanceUntilZoom;

    public RectTransform CanvasRect;



   public void OnMouseDown()
    {
      OpenMenu();
    }

    private void Start()
    {
        //gets the scale of the map point when the camera isn't zoomed in
        InitialPointScale = transform.localScale;
        //gets the scale of the map point when the camera is zoomed in
        ZoomedPointScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        //gets other initial values for the map point
        InitialColliderScale = transform.GetComponent<BoxCollider>().size;
        InitialColliderPosition = transform.GetComponent<BoxCollider>().center;
        InitialCanvasPosition = CanvasRect.localPosition;

        hitBox = transform.GetComponent<BoxCollider>();
        //converts the GPS coordinates into rotation values that can be used by the engine
        latitude = DegreeConverter(latitude);
        longitude = DegreeConverter(longitude);
    }
    private void Update()
    {
        //gets the distance between the camera and globe and turns it into a 0-1 scale between 6-10 with 6 being 0 and 10 being 1
        float distance = (globeCameraData.getDistance() - 6f) / 4f;

        //changes the size of the map point, it's hitbox and it's canvas based on how zoomed in the camera is
        transform.localScale = Vector3.Lerp(ZoomedPointScale, InitialPointScale, distance);
        hitBox.size = Vector3.Lerp( ZoomedColliderScale, InitialColliderScale, distance);
        hitBox.center = Vector3.Lerp( ZoomedColliderPosition, InitialColliderPosition, distance);
        CanvasRect.localPosition = Vector3.Lerp(ZoomedCanvasPosition, InitialCanvasPosition, distance);
        CanvasRect.localScale = Vector3.Lerp((Vector3.one)*0.005f, (Vector3.one) * 0.01f, distance);

        //checks if the camera positon is outside 10 degrees of the map point of either the latitude or longitude and hides the text
        if (Mathf.DeltaAngle(latitude, globeCameraData.GetLatitude()) > 10 || Mathf.DeltaAngle(latitude, globeCameraData.GetLatitude()) < -10)
        {
            if (Mathf.DeltaAngle(longitude, globeCameraData.GetLongitude()) > 10 || Mathf.DeltaAngle(longitude, globeCameraData.GetLongitude()) < -10)
            {
                Text.FadeOut();
                ShadowText.FadeOut();
            }

            //checks if the camera is zoomed in enough to show the text and shows it if it is
            else
            {
                if (camDistanceUntilZoom > globeCameraData.getDistance())
                {
                    Text.FadeIn();
                    ShadowText.FadeIn();
                }

                else
                {
                    Text.FadeOut();
                    ShadowText.FadeOut();
                }
            }
        }

        //checks if the camera is zoomed in enough to show the text and shows it if it is
        else
        {
            if (camDistanceUntilZoom > globeCameraData.getDistance())
            {
                Text.FadeIn();
                ShadowText.FadeIn();
            }

            else
            {
                Text.FadeOut();
                ShadowText.FadeOut();
            }
        }
    }

    //checks if the GPS position is close to the marker and opens the marker if it is
    public void checkGPS(float lat, float lon)
    {
        if (Mathf.Abs(lat-latitude) < 2)
        {
            if (Mathf.Abs(lon - longitude) < 2)
            {  
                OpenMenu();
            }
        }
    }

    //converts the latitude and longitude values into rotation values that can be used by the camera. 
    private float DegreeConverter(float rot)
    {
        if (rot > 180)
        {
            rot = rot - 360;
        }

        else if (rot < 0)
        {
            rot = rot + 360;
        }

        else
        {
            return rot;
        }

        return rot;
    }

    //shows the menu for the city button that was tapped
    private void OpenMenu()
    {
        //shows any download bars if a level is currently downloading
        loadingBars.BarStatus(true);
        //fades in the images for the menu and enables the buttons
        chooseArea.fadeIn();
        //stops the globe camera from moving and makes sure the user can't move around the globe until the menu is closed
        globeCameraData.isMovingToGPS = false;
        globeCameraData.enabled = false;
        //removes the GPS button and makes sure it can't be used until the menu is closed
        GPSButton.GetComponent<Button>().interactable = false;
        GPSButton.transform.GetChild(0).transform.GetChild(2).GetComponent<GuiFadeInFadeOut>().FadeOut();
        
    }


}
