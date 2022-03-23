using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml;
using System.IO;
using System.Globalization;
using System;

[RequireComponent(typeof(BoxCollider2D))]

public class MapMarker : MonoBehaviour, IDragHandler
{
    private RectTransform thisTransform;
    private bool isMarkerMoving = false;

    private double longitude;
    private double latitude;

    public TextAsset RawXMLFile;

    public string SceneName;

    //stores the rectangular bounds of the map in GPS coordinates
    private double WestGPSBounds;
    private double EastGPSBounds;
    private double NorthGPSBounds;
    private double SouthGPSBounds;

    //stores the height value the user is placed at when they chose a point on the map to teleport to
    private float placementHeight;
    private float defaultPlacementHeight;

    //Unity map bounds 
    private float HeightBound;
    private float WidthBound;


    //object reference of bounds box
    private GameObject boundBox;

    //reference to player
    private GameObject player;

    //Out of bounds text
    private GameObject[] OutOfBoundsText;

    //No GPS Signal Text
    private GameObject[] NoGPSText;

    //length and width in GPS values
    private double GPSXLength;
    private double GPSYLength;

    //coordinates on the 2D GUI map
    private float mapX;
    private float mapY;

    private Vector2 mapSize; 

    //executes when tapping and dragging the marker
    public void OnDrag(PointerEventData eventData)
    {
        removeMapWarnings();      
        setMapPositionGUI();
    }

    
    void Start()
    {
        mapX = 0;
        mapY = 0;

        placementHeight = 0;

        //grabs the data from the xml file
        string data = RawXMLFile.text;
        XMLParsing(data);

        thisTransform = transform.GetComponent<RectTransform>();
   
        StartCoroutine(MapDataTimer());
        //gets the bounding box of the 3D map
        boundBox = GameObject.FindGameObjectWithTag("MapBounds");
        player = GameObject.FindGameObjectWithTag("Player");
        //gathers the objects that display that their is an issue with the GPS
        OutOfBoundsText = GameObject.FindGameObjectsWithTag("OutOfBoundsMessage");
        NoGPSText = GameObject.FindGameObjectsWithTag("NoGPS");

    }

    private IEnumerator MapDataTimer()
    {
        yield return new WaitForSeconds(0.7f);
        //gets the size of the map
        mapSize = new Vector2(transform.parent.GetComponent<MapSizeData>().GetInitialMapWidth(), transform.parent.GetComponent<MapSizeData>().GetInitialMapHeight());
        //gets the size of the marker
        mapX = transform.GetComponent<RectTransform>().anchoredPosition.x;
        mapY = transform.GetComponent<RectTransform>().anchoredPosition.y;
    }

    //takes the objects gievn to it and either fades the images in or out of the scene
    private void fadeGroup(GameObject[] a, bool isFadingOut)
    {
        for(int i = 0; i<a.Length;i++)
        {
            if (isFadingOut == false)
            {
                a[i].GetComponent<GuiFadeInFadeOut>().FadeSwitch();
            }

            else
            {
                a[i].GetComponent<GuiFadeInFadeOut>().FadeOut();
            }
        }
    }

    //removes the warnings showing the GPS is out of bounds and that is not receiving GPS data
    public void removeMapWarnings()
    {
        fadeGroup(OutOfBoundsText, true);
        fadeGroup(NoGPSText, true);
    }

    //gets dfata from the XML file
    private void XMLParsing(string data)
    {
        XmlDocument XMLdata = new XmlDocument();
        XMLdata.Load(new StringReader(data));
        //only gets the data is in the same tag as the scene name
        XmlNodeList datalist = XMLdata.GetElementsByTagName(SceneName);
        foreach(XmlNode xn in datalist)
        {
            //gets values of the most east, west, northern and southern points of the map
            WestGPSBounds = double.Parse(xn["West"].InnerText);
            EastGPSBounds = double.Parse(xn["East"].InnerText);
            NorthGPSBounds = double.Parse(xn["North"].InnerText);
            SouthGPSBounds = double.Parse(xn["South"].InnerText);

            //gets the default height the user will be transported to in Unity space
            placementHeight = float.Parse(xn["PlacementHeight"].InnerText);
            defaultPlacementHeight = placementHeight;

            //gets the length of the X and the Y of the bounds by subtracting the bounds and getting a value that is the last 4 decimal places and multiplied to a whole number.
            //for example 36.1756 - 36.0287 = 0.1469. The value would be 1469
            GPSXLength = Math.Abs(EastGPSBounds - WestGPSBounds) * 1000;
            GPSYLength = Math.Abs(NorthGPSBounds - SouthGPSBounds) * 1000;

        }
    }

    //changes the placement height
    public void setPlacementHeight(float h)
    {
        placementHeight = h;
    }

    //reverts the placement height to the original setting
    public void SetDefaultPlacementHeight()
    {
        placementHeight = defaultPlacementHeight;
    }

    //
    private void setMapPositionGUI()
    {
        //sets the mapX and mapY values to the current position while dragging
        mapX = transform.GetComponent<RectTransform>().anchoredPosition.x;
        mapY = transform.GetComponent<RectTransform>().anchoredPosition.y;
        //gets the 2d map width before it was scale
        mapSize = new Vector2(transform.parent.GetComponent<MapSizeData>().GetInitialMapWidth(), transform.parent.GetComponent<MapSizeData>().GetInitialMapHeight());
    }

    //gets the GPS data and places the marker on the 2D map to match it
    private IEnumerator StartLocationService()
    {
        string data = RawXMLFile.text;
        XMLParsing(data);

        yield return new WaitForSeconds(2.0f);
        
        //set the marker's position to the most north western point of the map
        longitude =  WestGPSBounds;
        latitude = NorthGPSBounds;

        //checks if the user hasn't allowed the app to get location data
        if (!Input.location.isEnabledByUser)
        {
            //shows the message explaining the GPS data can't be gathered
            fadeGroup(NoGPSText,false);
            //puts the marker in the centre of the 2D map
            thisTransform.anchoredPosition = new Vector2(0, 0);

            yield break;
        }

        Input.location.Start();
        yield return new WaitForSeconds(5.0f);
        //will wait for a maximum of 20 seconds
        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);

            maxWait--;
        }

        //once 20 seconds have passed
        if (maxWait <= 0)
        {
            //shows the message explaining the GPS data can't be gathered
            fadeGroup(NoGPSText, false);
            //puts the marker in the centre of the 2D map
            thisTransform.anchoredPosition = new Vector2(0, 0);
            yield break;
        }

        //if the GPS is not working
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //shows the message explaining the GPS data can't be gathered
            fadeGroup(NoGPSText, false);
            //puts the marker in the centre of the 2D map
            thisTransform.anchoredPosition = new Vector2(0, 0);
            yield break;
        }

        //get GPS coordinates from phone
        longitude = Input.location.lastData.longitude;
        latitude = Input.location.lastData.latitude;

        //checks if the GPS coordinates are within the bounds of the map
        if (checkGPSBounds(longitude, latitude) == true)
        { 
            convertToMapUnits(longitude, latitude);
        }

        //if the GPS coordinates are not in the location
        else
        {
            //shows the message displaying that the user if out of the bounds and puts the marker in the centre
            fadeGroup(OutOfBoundsText,false);
            thisTransform.anchoredPosition = new Vector2(0, 0);
        }
    }

    public void GetGPSPosition()
    {
        StartCoroutine(StartLocationService());
    }

    //returns whether then GPS coordinates are in bounds or not
    private bool checkGPSBounds(double lon, double lat)
    {
        bool inBounds = false ;
        //compares the longitude value to the east and west bounds to check if they are in between them
        if(lon < EastGPSBounds && lon > WestGPSBounds)
        {
            //compares the latitude value to the south and north bounds to check if they are in between them
            if (lat < NorthGPSBounds && lat > SouthGPSBounds)
            {
                inBounds = true;
            }
        }

        else
        {
            inBounds = false;
        }

        return inBounds;
    }

    public void movePlayer()
    {
        //gets the bounds in real world units
        WidthBound =  boundBox.transform.GetComponent<Renderer>().bounds.size.x;
        HeightBound = boundBox.transform.GetComponent<Renderer>().bounds.size.z;

        //converts the values of the 2d position to real world 3d
        float GUIMAPToGameMapX = (((mapX + mapSize.x/2)/mapSize.x) * WidthBound);
        float GUIMAPToGameMapZ = (((mapY + mapSize.y / 2) / mapSize.y) * HeightBound);

        //moves the player to new position
        player.transform.position = new Vector3(GUIMAPToGameMapX, placementHeight, GUIMAPToGameMapZ);

    }

    private void convertToMapUnits(double lon,double lat)
    {
        //gets GPS positions relative GPS bounds as a distance value
        double xRelative = (lon - WestGPSBounds)*1000;
        double yRelative = (lat - SouthGPSBounds)*1000;

        //gets the ratio of the positions relative to the length of the map. 
        //for example: If it is at the eastern bound, the X would be 1 but if it was at the western bound it would be 0
        float xRatio = Convert.ToSingle(xRelative / GPSXLength);
        float yRatio = Convert.ToSingle(yRelative / GPSYLength);

        //uses the ratio number to place the marker on the 2D map. The anchored position of the map is in the centre of the image
        mapX = Mathf.Lerp(-mapSize.x/2,mapSize.x/2,xRatio);
        mapY = Mathf.Lerp(-mapSize.y/2, mapSize.y/2, yRatio);

        thisTransform.anchoredPosition = new Vector2(mapX, mapY);

    }

    void Update()
    {
        if(isMarkerMoving == true)
        {
            moveMarker();
        }
    }

    public void setMarker()
    {
        isMarkerMoving = true;
    }
    public void moveMarker()
    {
        
        //gets the marker to match where the finger is moving
        if (Input.GetMouseButtonDown(1) == true)
        { 
        thisTransform.anchoredPosition = Input.mousePosition;
        }

       
    }

    
    
}
